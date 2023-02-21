using AzureKeyVaultExplorer.Lib;
using Microsoft.Extensions.DependencyInjection;
using static System.Console;

var serviceProvider = GetServiceProvider();

try
{
    var inputMode = GetTopLevelInput();

    switch (inputMode)
    {
        case "1":
            SearchSecrets();
            break;

        case "2":
            await ReadSecretAsync();
            break;

        case "3":
            await CreateOrUpdateSecretAsync();
            break;
    }
}
catch (InvalidOperationException ex)
{
    WriteLine(ex.Message);
}
catch (KeyNotFoundException ex)
{
    WriteLine(ex.Message);
}

IServiceProvider GetServiceProvider()
{
    var serviceCollection = new ServiceCollection();
    serviceCollection.AddTransient<IExplorer, Explorer>(o =>
    {
        var vaultUrl = args[0];
        var tenantId = args[1];
        var clientId = args[2];
        var clientSecret = args[3];

        return new Explorer(vaultUrl, tenantId, clientId, clientSecret);
    });
    var serviceProvider = serviceCollection.BuildServiceProvider();
    return serviceProvider;
}

string GetTopLevelInput()
{
    WriteLine("What you want to do?");
    WriteLine();
    WriteLine("1. Search secret names");
    WriteLine("2. Read a secret");
    WriteLine("3. Create or update a secret");
    WriteLine();
    Write("Provide input: ");

    var inputMode = ReadLine();

    if (inputMode != "1" && inputMode != "2" && inputMode != "3")
    {
        throw new InvalidOperationException("Invalid input.");
    }

    return inputMode;
}

void SearchSecrets()
{
    Write("Enter a secret key: ");
    var key = ReadLine();

    if (string.IsNullOrWhiteSpace(key))
    {
        throw new InvalidOperationException("Invalid input.");
    }

    var explorer = serviceProvider.GetRequiredService<IExplorer>();
    var machedKeyNames = explorer.SearchKeys(key);

    foreach (string machedKeyName in machedKeyNames)
    {
        WriteLine(machedKeyName);
    }
}

async Task ReadSecretAsync()
{
    Write("Enter a secret key: ");
    var key = ReadLine();

    if (string.IsNullOrWhiteSpace(key))
    {
        throw new InvalidOperationException("Invalid input.");
    }

    var explorer = serviceProvider.GetRequiredService<IExplorer>();
    var secret = await explorer.GetSecretAsync(key);
    WriteLine(secret);
}

async Task CreateOrUpdateSecretAsync()
{
    Write("Enter a secret key: ");
    var inputKeyName = ReadLine();

    Write("Enter secret value: ");
    var inputSecretValue = ReadLine();

    if (string.IsNullOrWhiteSpace(inputKeyName) || string.IsNullOrWhiteSpace(inputSecretValue))
    {
        throw new InvalidOperationException("Invalid input");
    }

    var explorer = serviceProvider.GetRequiredService<IExplorer>();
    await explorer.SetSecretAsync(inputKeyName, inputSecretValue);
}