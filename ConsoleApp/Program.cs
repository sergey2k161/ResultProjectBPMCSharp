namespace ConsoleApp;

internal static class Program
{
    public static async Task Main(string[] args)
    {
        const string apiUrl = "https://localhost:7134";
        
        Console.WriteLine("Введите UserDomainName:");
        string userDomainName = Console.ReadLine();
        //var userDomainName = "Lanit\\Tany.Babkina";
        string token = await QueryManager.LoginAsync(apiUrl, userDomainName);
        await QueryManager.GetLatestTradeAsync(apiUrl, userDomainName, token);
    }
}