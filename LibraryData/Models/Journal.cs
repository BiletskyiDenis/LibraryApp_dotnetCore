using System.ComponentModel.DataAnnotations;

namespace LibraryData.Models
{
    public class Journal : LibraryAsset
    {
        [Required]
        public string Frequency { get; set; }
    }
}
