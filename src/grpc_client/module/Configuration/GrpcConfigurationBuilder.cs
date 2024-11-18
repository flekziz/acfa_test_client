namespace grpc_client.module.Configuration;

public class GrpcConfigurationBuilder
{
    private GrpcConfiguration _configuration;

    public GrpcConfigurationBuilder()
    {
        _configuration = new GrpcConfiguration();
    }

    public GrpcConfigurationBuilder Endpoint(string endpoint)
    {
        _configuration.endpoint = endpoint;
        return this;
    }

    public GrpcConfigurationBuilder WithLanguage(string language)
    {
        _configuration.lang = language;
        return this;
    }

    internal GrpcConfiguration GetConfiguration() => _configuration;

}
