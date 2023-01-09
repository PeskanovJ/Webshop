using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekat.Shared.DTOs
{
    public class AdminDTO
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        [EmailAddress]
        [MaxLength(250)]
        public string Email { get; set; }
        [Phone]
        public string PhoneNumber { get; set; }
        
        
    }
}
