using Amazon;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using Microsoft.Extensions.Configuration;
using Amazon.SecurityToken;
using Amazon.SecurityToken.Model;

namespace AwsRdsKmsConsoleApp;

class Program
{
    static async Task Main(string[] args)
    {
        // Setup configuration
        var builder = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables();

        var configuration = builder.Build();

        var secret = await GetSecret(configuration);

        Console.WriteLine(secret);

        /*
        // Retrieve database credentials and get connection
        var dbConnection = new DatabaseConnection(configuration);
        using var connection = await dbConnection.GetConnectionAsync();
        Console.WriteLine("Connecting to the database.");
        await connection.OpenAsync();
        Console.WriteLine($"Connection State: {connection.State}");
    */
    }

    static async Task<string> GetSecret(IConfiguration configuration)
    {
        var secretName = configuration["SecretsManager:SecretName"];
        var regionName = configuration["AWS:Region"];

        Console.WriteLine($"Region: {regionName}, SecretName: {secretName}");

        var region = RegionEndpoint.GetBySystemName(regionName);
        var client = new AmazonSecretsManagerClient(region);

        GetSecretValueRequest request = new()
        {
            SecretId = secretName,
            VersionStage = "AWSCURRENT", // VersionStage defaults to AWSCURRENT if unspecified.
        };

        GetSecretValueResponse response;

        response = await client.GetSecretValueAsync(request);

        string secret = response.SecretString;

        return secret;
    }
}

// Class to read the caller's identity.
public static class Extensions
{
    public static async Task<string> GetCallerIdentityArn(this IAmazonSecurityTokenService stsClient)
    {
        var response = await stsClient.GetCallerIdentityAsync(new GetCallerIdentityRequest());
        return response.Arn;
    }
}
