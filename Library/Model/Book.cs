using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Model
{
    public class Book
    {
        [Key]
        public int ID_Book { get; set; }
        public string Name_Book { get; set; }
        public string Author_Name { get; set; }

        [Required]
        [ForeignKey("Genre")]
        public int GenreID { get; set; }
        public DateOnly Year_Public { get; set; }
        public int Count_Copy { get; set; }

    }
}
