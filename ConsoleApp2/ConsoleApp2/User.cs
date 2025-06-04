namespace SteamApp
{
    public class User
    {
        public string Name { get; set; }
        public decimal Balance { get; set; }
        

        public User(string name, decimal balance)
        {
            Name = name;
            Balance = balance;

        }
    }
}