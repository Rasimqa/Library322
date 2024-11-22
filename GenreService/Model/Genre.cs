using System.ComponentModel.DataAnnotations;

namespace Library.Model
{
    public class Genre
    {
        [Key]
        public int ID_Genre { get; set; }
        [Required]
        public string Name_Genre { get; set; } 
    }
}
