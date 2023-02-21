namespace AzureKeyVaultExplorer.Lib;

public interface IExplorer
{
    public IEnumerable<string> SearchKeys(string partialKeyName);

    public Task<Secret> GetSecretAsync(string keyName);

    public Task SetSecretAsync(string keyName, string value);
}