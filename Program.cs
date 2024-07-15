using Amazon;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using Microsoft.Extensions.Configuration;
using Amazon.SecurityToken;
using Amazon.SecurityToken.Model;
using System.Text.Json.Serialization;
using Microsoft.Data.SqlClient;
using System.Diagnostics;

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

        var watch = new Stopwatch();
        watch.Start();

        var secret = await GetSecret(configuration);

        Console.WriteLine($"Secret: {secret} in {TimeSpan.FromMilliseconds(watch.ElapsedMilliseconds).TotalSeconds}");

        var dbInstance = configuration["AWS:DatabaseInstance"];
        var dbName = configuration["AWS:DatabaseName"];

        Console.WriteLine($"DB Instance: {dbInstance}, DB Name: {dbName} in {watch.ElapsedMilliseconds}");

        // Retrieve database credentials and get connection
        var dbConnection = new SqlConnection($"Server={dbInstance},1433;Database={dbName};User Id={secret.Username};Password={secret.Password}; Encrypt=false; TrustServerCertificate=true;");
        await dbConnection.OpenAsync();
        Console.WriteLine($"Connection State: {dbConnection.State} in {watch.ElapsedMilliseconds}");
        var cmd = new SqlCommand("select 1", dbConnection);
        cmd.ExecuteNonQuery();
        await dbConnection.CloseAsync();
        Console.WriteLine($"Completed in {watch.ElapsedMilliseconds}");
        watch.Stop();
    }

    static async Task<UserAndPassword> GetSecret(IConfiguration configuration)
    {
        var regionName = configuration["AWS:Region"];
        var secretName = configuration["AWS:SecretName"];

        Console.WriteLine($"Region: {regionName}, SecretName: {secretName}");

        var region = RegionEndpoint.GetBySystemName(regionName);
        var client = new AmazonSecretsManagerClient(region);

        Console.WriteLine($"Region: {region}");

        GetSecretValueRequest request = new()
        {
            SecretId = secretName,
            VersionStage = "AWSCURRENT", // VersionStage defaults to AWSCURRENT if unspecified.
        };

        GetSecretValueResponse response;

        response = await client.GetSecretValueAsync(request);

        var cred = System.Text.Json.JsonSerializer.Deserialize<UserAndPassword>(response.SecretString);

        return cred!;
    }

    public class UserAndPassword
    {
        [JsonPropertyName("username")]
        public required string Username { get; set; }

        [JsonPropertyName("password")]
        public required string Password { get; set; }

        public override string ToString()
        {
            return $"User: {Username}, Password: {Password}";
        }
    }
}
