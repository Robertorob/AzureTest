using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace AzureTest.Services
{
    public class AzureKeyVaultService
    {
        private readonly IConfiguration configuration;
        private readonly KeyVaultClient keyVaultClient;
        public AzureKeyVaultService(IConfiguration configuration)
        {
            this.configuration = configuration;
            AzureServiceTokenProvider azureServiceTokenProvider = new AzureServiceTokenProvider();
            this.keyVaultClient = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(azureServiceTokenProvider.KeyVaultTokenCallback));
        }

        public string GetConnectionString(string connectionStringName)
        {
            var keyVaultSection = this.configuration.GetSection("KeyVault");
            string keyVaultName = keyVaultSection["Name"];
            string secretKey = keyVaultSection.GetSection("SecretKeys")[connectionStringName]; ;
#if RELEASE
            
            string connectionString = this.keyVaultClient.GetSecretAsync($"https://{keyVaultName}.vault.azure.net/secrets/{secretKey}").Result.Value;
#endif
#if DEBUG
            string connectionString = this.configuration.GetConnectionString(connectionStringName);
#endif
            return connectionString;
        }
    }
}
