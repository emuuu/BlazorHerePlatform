using System.Collections.Concurrent;
using System.Collections.Generic;

namespace HerePlatform.Blazor;

internal static class JsObjectRefInstances
{
    private static readonly ConcurrentDictionary<string, IJsObjectRef> Instances = new();

    internal static void Add(IJsObjectRef instance)
    {
        Instances.TryAdd(instance.Guid.ToString(), instance);
    }

    internal static void Remove(string guid)
    {
        Instances.TryRemove(guid, out _);
    }

    internal static IJsObjectRef GetInstance(string guid)
    {
        if (!Instances.TryGetValue(guid, out var instance))
            throw new KeyNotFoundException($"JsObjectRef with guid '{guid}' not found in registry.");
        return instance;
    }
}
