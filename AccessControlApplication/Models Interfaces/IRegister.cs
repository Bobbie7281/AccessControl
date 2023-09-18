using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace AccessControlApplication.Models
{
    public interface IRegister
    {
        
        public int Id { get; set; }

        
        public string? IdCardNum { get; set; }

        
        public string? FullName { get; set; }

        
        public string? Address { get; set; }

        
        public string? ContactNumber { get; set; }

        
        public string? EmailAddress { get; set; }

       
        public bool Administrator { get; set; }
    }
}
