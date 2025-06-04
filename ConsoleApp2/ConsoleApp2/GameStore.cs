using SteamApp.Enums;
using SteamApp.Utilities;
using System.Reflection;
using System.Text.Json;

namespace SteamApp
{
    public class GameStore : IDisposable
    {
        public List<Game> Games { get; set; } = new();
        public List<Transaction> Transactions { get; set; } = new();
        public User User { get; set; }

        public List<string> Games_user = new List<string>();

        public event Action<Game> OnGamePurchased;

        //public Game this[string title] => Games.FirstOrDefault(g => g.Title == title);

        public void Run()
        {
            bool exit = false;
            while (!exit)
            {
                Console.WriteLine($"\nБаланс: {User.Balance}р");
                Console.WriteLine("1. Список игр\n2. Купить игру\n3. Пополнить баланс\n4. Показать купленые игры\n5. Добавить новую игру\n6. Выход");
                var input = Console.ReadLine();
                switch (input)
                {
                    case "1": ShowGames(); break;
                    case "2": BuyGame(); break;
                    case "3": AddBalance(); break;
                    case "4": ShowAllGames(); break;
                    case "5": AddNewGame(); break;
                    case "6": exit=true; break;
                }
            }
        }

        private void ShowGames()
        {
            var sortedGames = Games.OrderBy(g => g.Year).ToList();
            foreach (var game in sortedGames)
                Console.WriteLine(game.GetInfo());
        }



        private void AddNewGame()
        {
            Games = FileManager.Load<List<Game>>("games.json") ?? new List<Game>();
            Console.Write("название?: ");
            string title = Console.ReadLine();
            Game game = Games.FirstOrDefault(g => g.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
            if (game != null)
            {
                Console.WriteLine("Игра уже есть.");
                return;
            }
            Console.Write("создатель?: ");
            string prod = Console.ReadLine();
            Console.Write("год?: ");
            int year = Convert.ToInt32(Console.ReadLine());
            Console.Write("ISDN?: ");
            string isbn = Console.ReadLine();
            Console.Write("цена?: ");
            int cost = Convert.ToInt32(Console.ReadLine());


            var newGame = new Game
            {
                Title = title,
                Developer = prod,
                Year = year,
                ISBN = isbn,
                Price = cost,
                Status = 0
            };

            Games.Add(newGame);
            var options = new JsonSerializerOptions { WriteIndented = true };
            string updatedJson = JsonSerializer.Serialize(Games, options);
            File.WriteAllText("games.json", updatedJson);

            Console.WriteLine("Игра добавлена в JSON.");
        }

        private void ShowAllGames()
        {
            Console.WriteLine("\\nСписок купленых игр:");
            foreach (var game in Games_user)
                Console.WriteLine(game);
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
                Games_user.Add(game.Title);
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
            Games = FileManager.Load<List<Game>>("games.json") ?? new List<Game>();
        }

        public void SaveData()
        {
            FileManager.Save("games.json", Games);
        }

        public void Dispose()
        {
            SaveData();
        }
    }
}
