using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekat.Shared.DTOs
{
    public class ProfileDTO
    {
        [Required]
        [DisplayName("First name")]
        public string FirstName { get; set; }
        [Required]
        [DisplayName("Last name")]
        public string LastName { get; set; }
        
        [EmailAddress]
        [MaxLength(250)]
        public string Email { get; set; }
        [Phone]
        [DisplayName("Phone number")]
        public string PhoneNumber { get; set; }

        public DateTime Created { get; set; }
        [ValidateNever]
        public string ProfileUrl { get; set; }

    }
}
