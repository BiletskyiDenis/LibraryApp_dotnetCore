namespace LibraryApp.Models
{
    public class BookViewModel : LibraryAssetViewModel
    {
        public string Author { get; set; }
        public string ISBN { get; set; }
        public int Pages { get; set; }
    }
}