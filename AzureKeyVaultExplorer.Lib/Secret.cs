using System.Collections;
using System.Text;

namespace AzureKeyVaultExplorer.Lib;

public class Secret : IEnumerable<SecretDetail>
{
    private readonly List<SecretDetail> _secretDetail = new();

    public required string Key { get; set; }

    public void AddDetail(string value, string version, DateTimeOffset? lastUpdatedOn, bool? isEnable) =>
        _secretDetail.Add(new SecretDetail(value, version, lastUpdatedOn, isEnable));

    IEnumerator<SecretDetail> IEnumerable<SecretDetail>.GetEnumerator() =>
        _secretDetail.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() =>
        _secretDetail.GetEnumerator();

    public override string ToString()
    {
        var printableOutputBuilder = new StringBuilder();

        printableOutputBuilder.AppendLine("----");
        printableOutputBuilder.Append("Key:");
        printableOutputBuilder.AppendLine(Key);
        printableOutputBuilder.AppendLine();

        var orderedSecretDetails = this.OrderByDescending(secretDetail => secretDetail.LastUpdatedOn);

        foreach (var secretDetail in orderedSecretDetails)
        {
            printableOutputBuilder.Append("Value: ");
            printableOutputBuilder.AppendLine(secretDetail.Value);
            printableOutputBuilder.AppendLine();

            printableOutputBuilder.Append("Version: ");
            printableOutputBuilder.AppendLine(secretDetail.Version);
            printableOutputBuilder.AppendLine();

            printableOutputBuilder.Append("Last updated on: ");
            printableOutputBuilder.AppendLine(secretDetail.DisplayLastUpdatedOn);
            printableOutputBuilder.AppendLine();

            printableOutputBuilder.Append("Enabled: ");
            printableOutputBuilder.AppendLine(secretDetail.IsEnable);
            printableOutputBuilder.AppendLine();

            printableOutputBuilder.AppendLine("----");
        }

        return printableOutputBuilder.ToString();
    }
}