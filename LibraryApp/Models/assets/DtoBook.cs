namespace LibraryApp.Models
{
    public class DtoBook : DtoLibraryAsset
    {
        public string Author { get; set; }
        public string ISBN { get; set; }
        public int Pages { get; set; }
    }
}