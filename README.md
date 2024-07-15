## AWS RDS KMS Console App

This project is a console application built using .NET 8.0, demonstrating how to securely retrieve secrets from AWS Secrets Manager and read the caller's identity from AWS Security Token Service (STS). The application is designed to help developers integrate AWS SDK into their .NET applications for managing secrets and accessing AWS resources securely.

### Features

- **AWS Secrets Manager Integration**: Securely retrieve secrets stored in AWS Secrets Manager.
- **AWS Security Token Service (STS) Integration**: Retrieve and display the caller's identity ARN.
- **Configuration Management**: Utilize `appsettings.json` and environment variables for configuration.

### Technologies Used

- .NET 8.0
- AWS SDK for .NET
  - `AWSSDK.SecretsManager`
  - `AWSSDK.SecurityToken`
- Microsoft.Extensions.Configuration
  - `Microsoft.Extensions.Configuration.Json`
  - `Microsoft.Extensions.Configuration.EnvironmentVariables`

### Prerequisites

- .NET 8.0 SDK
- AWS account with appropriate permissions
- AWS CLI configured with the necessary credentials

### Setup Instructions

1. **Clone the Repository**:
    ```sh
    git clone https://github.com/yourusername/AwsRdsKmsConsoleApp.git
    cd AwsRdsKmsConsoleApp
    ```

2. **Configure the Application**:
    Update the `appsettings.json` file with your AWS region and secret name:
    ```json
    {
      "AWS": {
        "Region": "your-region"
      },
      "SecretsManager": {
        "SecretName": "your-secret-name"
      }
    }
    ```

3. **Build and Run the Application**:
    ```sh
    dotnet build
    dotnet run
    ```

### Usage

Upon running the application, it will retrieve a secret from AWS Secrets Manager and print it to the console. Additionally, it includes a utility method to get the caller's identity ARN from AWS STS.

### Project Structure

```
AwsRdsKmsConsoleApp/
├── AwsRdsKmsConsoleApp.csproj
├── Program.cs
├── Extensions.cs
├── appsettings.json
└── README.md
```

### Contributions

Contributions are welcome! Please feel free to submit a pull request or open an issue if you have any suggestions or improvements.

### License

This project is licensed under the MIT License. See the LICENSE file for details.

### Contact

For any questions or feedback, please contact [Hriday Sheth](mailto:shethhriday2907@gmail.com).
