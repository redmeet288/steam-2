using SteamApp.Enums;
using SteamApp.Utilities;
using System.Text.Json;

namespace SteamApp
{
    public class GameStore : IDisposable
    {
        public List<Game> Games { get; set; } = new();
        public List<Transaction> Transactions { get; set; } = new();
        public User User { get; set; }

        public event Action<Game> OnGamePurchased;

        //public Game this[string title] => Games.FirstOrDefault(g => g.Title == title);

        public void Run()
        {
            bool exit = false;
            while (!exit)
            {
                Console.WriteLine($"\nБаланс: {User.Balance}р");
                Console.WriteLine("1. Список игр\n2. Купить игру\n3. Пополнить баланс\n4. Показать все игры\n5. Выход");
                var input = Console.ReadLine();
                switch (input)
                {
                    case "1": ShowGames(); break;
                    case "2": BuyGame(); break;
                    case "3": AddBalance(); break;
                    case "4": ShowAllGames(); break;
                    case "5": exit=true; break;
                }
            }
        }

        private void ShowGames()
        {
            var sortedGames = Games.OrderBy(g => g.Year).ToList();
            foreach (var game in sortedGames)
                Console.WriteLine(game.GetInfo());
        }



        private void ShowAllGames()
        {
            Console.WriteLine("\\nСписок всех игр:");
            foreach (var game in Games)
                Console.WriteLine(game.GetInfo());
        }

        private void BuyGame()
        {
            Console.Write("Введите название игры: ");
            string title = Console.ReadLine();
            Game game = Games.FirstOrDefault(g => g.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
            if (game == null)
            {
                Console.WriteLine("Игра не найдена.");
                return;
            }

            if (User.Balance >= game.Price && game.Status == GameStatus.Available)
            {
                User.Balance -= game.Price;
                game.Status = GameStatus.Borrowed;
                Transactions.Add(new Transaction(DateTime.Now, User, game));
                OnGamePurchased?.Invoke(game);
                Logger.Log($"Куплена игра: {game.Title}");
                Console.WriteLine("Покупка успешна.");
            }
            else Console.WriteLine("Недостаточно средств или игра недоступна.");
        }

        private void AddBalance()
        {
            Console.Write("Введите сумму: ");
            if (decimal.TryParse(Console.ReadLine(), out decimal amount))
            {
                User.Balance += amount;
                Logger.Log($"Баланс пополнен на {amount}р.");
            }
            else Console.WriteLine("Неверный ввод.");
        }

        public void LoadData()
        {
            Games = FileManager.Load<List<Game>>("C:\\Users\\admin\\source\\repos\\ConsoleApp2\\ConsoleApp2\\Data\\games.json") ?? new List<Game>();
        }

        public void SaveData()
        {
            FileManager.Save("Data/games.json", Games);
        }

        public void Dispose()
        {
            SaveData();
        }
    }
}