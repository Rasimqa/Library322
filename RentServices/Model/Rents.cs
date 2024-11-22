using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Model
{
    public class Rents
    {

        [Key]
        public int ID_Rental { get; set; }

        [Required]
        [ForeignKey("Reader")]
        public int ID_Reader { get; set; }

        [Required]
        [ForeignKey("Book")]
        public int ID_Book { get; set; }
        public DateOnly Rental_Date { get; set; }
        public DateOnly Return_Date { get; set; }
        public bool Returned { get; internal set; }
    }
}
