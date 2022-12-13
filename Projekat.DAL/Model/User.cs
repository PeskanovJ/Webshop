using Projekat.Shared.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekat.DAL.Model
{
    public class User : Entity
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

        public byte[] Password { get; set; }
        public byte[] Salt { get; set; }
        public ICollection<Item> PostedItems { get; set; }
        public ICollection<Item> FollowedItems { get; set; }
        public ICollection<Message> Messages { get; set; }
        public SD.Roles Role { get; set; }
        public DateTime Created { get; set; }
        public Guid ActivationGuid { get; set; }
        public Guid PasswordGuid { get; set; }
        public string ProfileUrl { get; set; }




        //public string? StreetAddress { get; set; }
        //public string? City { get; set; }
        //public string? State { get; set; }
        //public string? PostalCode { get; set; }

        //FirstName
        //LastName
        //Email 			    //unique
        //Password 				//hashed
        //PhoneNo 				//unique
        //ICollection<Item>		//svi oglasi
        //ICollection<Item>		//oglasi koje pratimo
        //ICollection<Message>
        //Role					//enumeracija
        //Created					//datum signupa
        //ActivationGuid			//guid koji cemo koristiti za aktivaciju naloga
        //PasswordGuid			//guid koji cemo koristiti za reset passworda
        //ProfileUrl				//putanja do profilne slike
    }
}
