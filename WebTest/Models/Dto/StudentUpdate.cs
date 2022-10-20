using System.ComponentModel.DataAnnotations;

namespace WebTest.Models.Dto
{
    public class StudentUpdate
    {
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public DateTime Enrolled { get; set; }
        public DateTime Updated { get; set; }
    }
}
