using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class Journal : LibraryAsset
    {
        [Required]
        public string Frequency { get; set; }
    }
}
