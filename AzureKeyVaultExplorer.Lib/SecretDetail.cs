namespace AzureKeyVaultExplorer.Lib;

public class SecretDetail
{
    public string Value { get; init; }

    public string Version { get; init; }

    public DateTimeOffset? LastUpdatedOn { get; init; }

    public string? IsEnable { get; init; }

    public string DisplayLastUpdatedOn =>
        LastUpdatedOn.HasValue ? LastUpdatedOn.Value.ToString() : string.Empty;

    public SecretDetail(string value, string version, DateTimeOffset? lastUpdatedOn, bool? isEnable)
    {
        Value = value;
        Version = version;
        LastUpdatedOn = lastUpdatedOn;
        IsEnable = isEnable.HasValue ? isEnable.Value switch { true => "Yes", false => "No" } : null;
    }
}