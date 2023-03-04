# Azure Key Vault explorer

This is a console app to manage secret values in Azure Key Vault.

To run the app you need to provide four command line arguments `vault url`, `tenant id` `client id` and `client secret`. You can provide those in `launchSettings.json` when you are running from Visual Studio or you can publish the app and provide the command line arguments at the time of running the published app.

```
dotnet AzureKeyVaultExplorer.ConsoleApp.dll "<vault url>" "<tenant id>" "<client id>" "<client secret>"
```
