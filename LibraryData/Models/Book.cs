using System.ComponentModel.DataAnnotations;

namespace LibraryData.Models
{
    public class Book:LibraryAsset
    {
        [Required]
        public string Author { get; set; }
        public string ISBN { get; set; }
        public int Pages { get; set; }
    }
}