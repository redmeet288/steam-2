using SteamApp.Interfaces;

namespace SteamApp
{
    public abstract class LibraryItem : ISearchable
    {
        public string Title { get; set; }
        public int Year { get; set; }
        public string ISBN { get; set; }

        public abstract string GetInfo();
        public abstract bool SearchByKeyword(string keyword);
    }
}