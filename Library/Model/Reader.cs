using System.ComponentModel.DataAnnotations;

namespace Library.Model
{
    public class Reader
    {
        [Key]
        public int ID_Reader { get; set; }
        public string Name_Reader { get; set; }
        public string FName_Reader { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public int number_phone { get; set; }
    }

}
