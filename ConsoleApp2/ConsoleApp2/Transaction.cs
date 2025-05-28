namespace SteamApp
{
    public struct Transaction
    {
        public DateTime Date { get; set; }
        public User User { get; set; }
        public Game Game { get; set; }

        public Transaction(DateTime date, User user, Game game)
        {
            Date = date;
            User = user;
            Game = game;
        }
    }
}