using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

namespace AzureKeyVaultExplorer.Lib;

public class Explorer : IExplorer
{
    private readonly SecretClient _secretClient;

    public Explorer(string vaultUrl, string tenantId, string clientId, string clientSecret)
    {
        var vaultUri = new Uri(vaultUrl);
        var credential = new ClientSecretCredential(tenantId, clientId, clientSecret);
        _secretClient = new SecretClient(vaultUri, credential);
    }

    IEnumerable<string> IExplorer.SearchKeys(string partialKeyName)
    {
        var allSecretPropertiesPages = _secretClient.GetPropertiesOfSecrets();

        if (!allSecretPropertiesPages.Any())
        {
            throw new InvalidOperationException("No key found");
        }

        var matchedKeyNames = allSecretPropertiesPages
            .ToList()
            .Where(secretProperty => secretProperty.Name.Contains(partialKeyName))
            .Select(secretProperty => secretProperty.Name);

        if (!matchedKeyNames.Any())
        {
            throw new InvalidOperationException("No key found");
        }

        return matchedKeyNames;
    }

    async Task<Secret> IExplorer.GetSecretAsync(string keyName)
    {
        var secretPropertiesPages = _secretClient.GetPropertiesOfSecretVersions(keyName);

        if (!secretPropertiesPages.Any())
        {
            throw new KeyNotFoundException("Key not found");
        }

        var secretProperties = secretPropertiesPages.ToList();
        var firstSecretProperty = secretProperties.First();
        var secret = new Secret { Key = firstSecretProperty.Name };

        foreach (var secretProperty in secretProperties)
        {
            var secretResponse = await _secretClient
                .GetSecretAsync(secretProperty.Name, secretProperty.Version)
                .ConfigureAwait(false);

            var secretValue = secretResponse.Value.Value;
            secret.AddDetail(secretValue, secretProperty.Version, secretProperty.UpdatedOn, secretProperty.Enabled);
        }

        return secret;
    }

    async Task IExplorer.SetSecretAsync(string keyName, string value) =>
        await _secretClient.SetSecretAsync(keyName, value).ConfigureAwait(false);
}