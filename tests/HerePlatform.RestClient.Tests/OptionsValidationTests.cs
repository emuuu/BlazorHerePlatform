namespace HerePlatform.RestClient.Tests;

[TestFixture]
public class OptionsValidationTests
{
    [Test]
    public void Validate_NoAuthMethod_Throws()
    {
        var options = new HereRestClientOptions();
        var ex = Assert.Throws<InvalidOperationException>(() => options.Validate());
        Assert.That(ex!.Message, Does.Contain("No authentication method"));
    }

    [Test]
    public void Validate_ApiKeyOnly_Succeeds()
    {
        var options = new HereRestClientOptions { ApiKey = "test-key" };
        Assert.DoesNotThrow(() => options.Validate());
    }

    [Test]
    public void Validate_OAuthOnly_Succeeds()
    {
        var options = new HereRestClientOptions
        {
            AccessKeyId = "key-id",
            AccessKeySecret = "key-secret"
        };
        Assert.DoesNotThrow(() => options.Validate());
    }

    [Test]
    public void Validate_TokenProviderOnly_Succeeds()
    {
        var options = new HereRestClientOptions
        {
            TokenProvider = _ => Task.FromResult("token")
        };
        Assert.DoesNotThrow(() => options.Validate());
    }

    [Test]
    public void Validate_OAuthMissingSecret_Throws()
    {
        var options = new HereRestClientOptions { AccessKeyId = "key-id" };
        var ex = Assert.Throws<InvalidOperationException>(() => options.Validate());
        Assert.That(ex!.Message, Does.Contain("AccessKeyId and AccessKeySecret"));
    }

    [Test]
    public void Validate_OAuthMissingId_Throws()
    {
        var options = new HereRestClientOptions { AccessKeySecret = "key-secret" };
        var ex = Assert.Throws<InvalidOperationException>(() => options.Validate());
        Assert.That(ex!.Message, Does.Contain("AccessKeyId and AccessKeySecret"));
    }

    [Test]
    public void Validate_MultipleAuthMethods_Throws()
    {
        var options = new HereRestClientOptions
        {
            ApiKey = "test-key",
            AccessKeyId = "key-id",
            AccessKeySecret = "key-secret"
        };
        var ex = Assert.Throws<InvalidOperationException>(() => options.Validate());
        Assert.That(ex!.Message, Does.Contain("Multiple authentication methods"));
    }

    [Test]
    public void Validate_ApiKeyAndTokenProvider_Throws()
    {
        var options = new HereRestClientOptions
        {
            ApiKey = "test-key",
            TokenProvider = _ => Task.FromResult("token")
        };
        var ex = Assert.Throws<InvalidOperationException>(() => options.Validate());
        Assert.That(ex!.Message, Does.Contain("Multiple authentication methods"));
    }

    [Test]
    public void Validate_DefaultTimeout_Is30Seconds()
    {
        var options = new HereRestClientOptions { ApiKey = "k" };
        Assert.That(options.Timeout, Is.EqualTo(TimeSpan.FromSeconds(30)));
    }
}
