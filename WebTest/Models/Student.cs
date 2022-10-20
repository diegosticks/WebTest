using System.ComponentModel.DataAnnotations;

namespace WebTest.Models
{
    public class Student
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public DateTime Enrolled { get; set; }
        public DateTime Updated { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
    }
}
