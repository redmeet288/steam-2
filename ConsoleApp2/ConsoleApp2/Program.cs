using SteamApp.Utilities;
using SteamApp;

class Program
{
    static void Main(string[] args)
    {
        Logger.Log("Приложение запущено.");
        GameStore store = new GameStore();
        store.LoadData();

        User user = new User("Иван", 10000);
        store.User = user;

        store.Run();

        store.SaveData();
        Logger.Log("Приложение завершено.");
    }
}