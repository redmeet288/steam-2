using SteamApp.Enums;

namespace SteamApp
{
    public class Game : LibraryItem
    {
        public string Developer { get; set; }
        public decimal Price { get; set; }
        public GameStatus Status { get; set; }

        public override string GetInfo() =>
            $"{Title} от {Developer}, {Year}. Цена: {Price}р. Статус: {Status}";

        public override bool SearchByKeyword(string keyword) =>
            Title.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
            Developer.Contains(keyword, StringComparison.OrdinalIgnoreCase);

        //public static bool operator ==(Game a, Game b) => a.Title == b.Title;
        //public static bool operator !=(Game a, Game b) => !(a == b);
        public override bool Equals(object obj) => obj is Game g && g == this;
        public override int GetHashCode() => ISBN.GetHashCode();
    }
}