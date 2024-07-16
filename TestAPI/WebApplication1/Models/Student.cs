using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Student
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "Student Name is required")]
        [StringLength(30)]
        public string Name { get; set; }
         [Required(ErrorMessage = "Student Age is required")]
        public int Age { get; set; }
    }
}
