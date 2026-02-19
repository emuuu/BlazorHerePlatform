using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using OneOf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace HerePlatform.Blazor;

internal static partial class Helper
{
    internal static async Task MyInvokeAsync(
        this IJSRuntime jsRuntime,
        string identifier,
        params object?[] args)
    {
        var jsFriendlyArgs = MakeArgJsFriendly(jsRuntime, args);
        await jsRuntime.InvokeVoidAsync(identifier, jsFriendlyArgs);
    }

    internal static async Task MyInvokeAsync(
        this IJSRuntime jsRuntime,
        string identifier,
        IList<IDisposable>? disposables,
        params object?[] args)
    {
        var jsFriendlyArgs = MakeArgJsFriendly(jsRuntime, args, disposables);
        await jsRuntime.InvokeVoidAsync(identifier, jsFriendlyArgs);
    }

    internal static Task<TRes?> MyInvokeAsync<TRes>(
        this IJSRuntime jsRuntime,
        string identifier,
        IList<IDisposable>? disposables,
        params object?[] args)
    {
        return jsRuntime.MyInvokeAsyncCore<TRes>(identifier, args, disposables);
    }

    internal static Task<TRes?> MyInvokeAsync<TRes>(
        this IJSRuntime jsRuntime,
        string identifier,
        params object?[] args)
    {
        return jsRuntime.MyInvokeAsyncCore<TRes>(identifier, args, null);
    }

    private static async Task<TRes?> MyInvokeAsyncCore<TRes>(
        this IJSRuntime jsRuntime,
        string identifier,
        object?[] args,
        IList<IDisposable>? disposables)
    {
        var jsFriendlyArgs = MakeArgJsFriendly(jsRuntime, args, disposables);

        if (typeof(IJsObjectRef).IsAssignableFrom(typeof(TRes)))
        {
            var guid = await jsRuntime.InvokeAsync<string?>(identifier, jsFriendlyArgs);
            if (guid == null) return default;
            try
            {
                return (TRes)JsObjectRefInstances.GetInstance(guid);
            }
            catch (KeyNotFoundException)
            {
                return default;
            }
        }

        if (typeof(IOneOf).IsAssignableFrom(typeof(TRes)))
        {
            var resultObject = await jsRuntime.InvokeAsync<string>(identifier, jsFriendlyArgs);
            object? result = null;

            if (resultObject is string someText)
            {
                try
                {
                    var jo = JsonDocument.Parse(someText);
                    var typeToken = jo.RootElement.GetProperty("dotnetTypeName").GetString();
                    if (typeToken != null)
                    {
                        var oneOfTypeArgs = typeof(TRes).GetGenericArguments();
                        var resolvedType = ResolveOneOfType(typeToken, oneOfTypeArgs);
                        if (resolvedType is not null)
                            result = DeSerializeObject(someText, resolvedType);
                        else
                            throw new InvalidOperationException(
                                $"OneOf type '{typeToken}' could not be resolved to any of [{string.Join(", ", oneOfTypeArgs.Select(t => t.FullName))}].");
                    }
                    else
                    {
                        result = someText;
                    }
                }
                catch (JsonException)
                {
                    // Not valid JSON — treat the raw string as the result value.
                    result = someText;
                }
                catch (KeyNotFoundException)
                {
                    // No dotnetTypeName property — treat the raw string as the result value.
                    result = someText;
                }
            }

            return (TRes?)result;
        }
        else
        {
            return await jsRuntime.InvokeAsync<TRes>(identifier, jsFriendlyArgs);
        }
    }

    internal static async Task<object> MyAddListenerAsync(
        this IJSRuntime jsRuntime,
        string identifier,
        params object[] args)
    {
        var jsFriendlyArgs = MakeArgJsFriendly(jsRuntime, args);

        return await jsRuntime.InvokeAsync<object>(identifier, jsFriendlyArgs);
    }

    internal static async Task<object> MyAddListenerAsync(
        this IJSRuntime jsRuntime,
        string identifier,
        IList<IDisposable>? disposables,
        params object[] args)
    {
        var jsFriendlyArgs = MakeArgJsFriendly(jsRuntime, args, disposables);

        return await jsRuntime.InvokeAsync<object>(identifier, jsFriendlyArgs);
    }

    internal static Task<OneOf<T, U>> MyInvokeAsync<T, U>(
        this IJSRuntime jsRuntime,
        string identifier,
        IList<IDisposable>? disposables,
        params object[] args)
    {
        return jsRuntime.MyInvokeAsyncOneOf<T, U>(identifier, disposables, args);
    }

    internal static Task<OneOf<T, U>> MyInvokeAsync<T, U>(
        this IJSRuntime jsRuntime,
        string identifier,
        params object[] args)
    {
        return jsRuntime.MyInvokeAsyncOneOf<T, U>(identifier, null, args);
    }

    private static async Task<OneOf<T, U>> MyInvokeAsyncOneOf<T, U>(
        this IJSRuntime jsRuntime,
        string identifier,
        IList<IDisposable>? disposables,
        object[] args)
    {
        var resultObject = await jsRuntime.MyInvokeAsync<object>(identifier, disposables, args);
        object? result = null;

        if (resultObject is JsonElement jsonElement)
        {
            string? json;
            if (jsonElement.ValueKind == JsonValueKind.Number)
            {
                json = jsonElement.GetRawText();
            }
            else if (jsonElement.ValueKind == JsonValueKind.String)
            {
                json = jsonElement.GetString();
                if (typeof(T) == typeof(string))
                {
                    result = json ?? "";
                    return (T)result;
                }

                if (typeof(U) == typeof(string))
                {
                    result = json ?? "";
                    return (U)result;
                }
            }
            else
            {
                json = jsonElement.GetString();
            }

            var propArray = Helper.DeSerializeObject<Dictionary<string, object>>(json);
            if (propArray?.TryGetValue("dotnetTypeName", out var typeName) ?? false)
            {
                var typeNameString = typeName.ToString()!;
                var type = ResolveOneOfType(typeNameString, typeof(T), typeof(U));
                if (type is not null)
                    result = Helper.DeSerializeObject(json, type);
            }
        }

        switch (result)
        {
            case T t:
                return t;
            case U u:
                return u;
            default:
                return default;
        }
    }

    internal static Task<OneOf<T, U, V>> MyInvokeAsync<T, U, V>(
        this IJSRuntime jsRuntime,
        string identifier,
        IList<IDisposable>? disposables,
        params object[] args)
    {
        return jsRuntime.MyInvokeAsyncOneOf<T, U, V>(identifier, disposables, args);
    }

    internal static Task<OneOf<T, U, V>> MyInvokeAsync<T, U, V>(
        this IJSRuntime jsRuntime,
        string identifier,
        params object[] args)
    {
        return jsRuntime.MyInvokeAsyncOneOf<T, U, V>(identifier, null, args);
    }

    private static async Task<OneOf<T, U, V>> MyInvokeAsyncOneOf<T, U, V>(
        this IJSRuntime jsRuntime,
        string identifier,
        IList<IDisposable>? disposables,
        object[] args)
    {
        var resultObject = await jsRuntime.MyInvokeAsync<object>(identifier, disposables, args);
        object? result = null;

        if (resultObject is JsonElement jsonElement)
        {
            var json = jsonElement.GetString();
            var propArray = Helper.DeSerializeObject<Dictionary<string, object>>(json);
            if (propArray?.TryGetValue("dotnetTypeName", out var typeName) ?? false)
            {
                var typeNameString = typeName.ToString()!;
                var type = ResolveOneOfType(typeNameString, typeof(T), typeof(U), typeof(V));
                if (type is not null)
                    result = Helper.DeSerializeObject(json, type);
            }
        }

        switch (result)
        {
            case T t:
                return t;
            case U u:
                return u;
            case V v:
                return v;
            default:
                return default;
        }
    }

    private static IEnumerable<object?> MakeArgJsFriendly(IJSRuntime jsRuntime, IEnumerable<object?> args)
    {
        return MakeArgJsFriendly(jsRuntime, args, null);
    }

    private static IEnumerable<object?> MakeArgJsFriendly(IJSRuntime jsRuntime, IEnumerable<object?> args, IList<IDisposable>? disposables)
    {
        var jsFriendlyArgs = args
            .Select(arg =>
            {
                if (arg == null)
                {
                    return arg;
                }

                if (arg is IOneOf oneof)
                {
                    arg = oneof.Value;
                }

                var argType = arg.GetType();

                switch (arg)
                {
                    case Enum: return GetEnumValue(arg);
                    case ElementReference _:
                    case string _:
                    case int _:
                    case long _:
                    case double _:
                    case float _:
                    case decimal _:
                    case DateTime _:
                    case bool _:
                        return arg;
                    case Action action:
                        {
                            var dotNetRef = DotNetObjectReference.Create(new JsCallableAction(jsRuntime, action));
                            disposables?.Add(dotNetRef);
                            return dotNetRef;
                        }
                    default:
                        {
                            if (argType.IsGenericType)
                            {
                                var typeDefinition = argType.GetGenericTypeDefinition();
                                if (typeDefinition == typeof(Action<>))
                                {
                                    var genericArguments = argType.GetGenericArguments();
                                    var dotNetRef = DotNetObjectReference.Create(new JsCallableAction(jsRuntime, (Delegate)arg, genericArguments));
                                    disposables?.Add(dotNetRef);
                                    return dotNetRef;
                                }

                                if (typeDefinition == typeof(Func<,>))
                                {
                                    var genericArguments = argType.GetGenericArguments();
                                    var dotNetRef = DotNetObjectReference.Create(new JsCallableFunc((Delegate)arg, genericArguments));
                                    disposables?.Add(dotNetRef);
                                    return dotNetRef;
                                }
                            }

                            switch (arg)
                            {
                                case JsCallableAction _:
                                case JsCallableFunc _:
                                    {
                                        var dotNetRef = DotNetObjectReference.Create(arg);
                                        disposables?.Add(dotNetRef);
                                        return dotNetRef;
                                    }
                                case IJsObjectRef jsObjectRef:
                                    {
                                        var guid = jsObjectRef.Guid;
                                        return SerializeObject(new JsObjectRefDto(guid));
                                    }
                                default:
                                    return SerializeObject(arg);
                            }
                        }
                }
            });

        return jsFriendlyArgs;
    }
}
