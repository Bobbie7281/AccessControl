using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AccessControlApplication.Models
{
    public class Register
    {
        [Key]
        [Required]
        [DisplayName("User Id")]
        public int Id { get; set; }
       
        [Required]
        [DisplayName("Id Card Number")]
        [MaxLength(15)]
        public string? IdCardNum { get; set; }

        [Required]
        [DisplayName("Full Name")]
        [MaxLength(50)]
        public string? FullName { get; set; }

        [Required]
        [DisplayName("Residence Address")]
        [MaxLength(150)]
        public string? Address { get; set; }

        [Required]
        [DisplayName("Contact Number")]
        [MaxLength(20)]
        public string? ContactNumber { get; set; }

        [Required]
        [DisplayName("Email Address")]
        [MaxLength(30)]
        public string? EmailAddress { get; set; }
    }
}
