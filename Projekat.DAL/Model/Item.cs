using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekat.DAL.Model
{
    public class Item:Entity
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Make { get; set; }
        [Required]
        public string Model { get; set; }
        public int? Price { get; set; }
        public string City { get; set; }
        public string Description { get; set; }
        ICollection<Image>? Images { get; set; }
        public DateTime Created { get; set; }
        ICollection<Review>? Reviews { get; set; }
        ICollection<Comment>? Comments { get; set; }

        [Required]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }
        [ValidateNever]
        public Category Category { get; set; }


        //Title
        //Make					//koja je marka trotineta
        //Model					//koji je model
        //Price 					//cijena koja je opciona
        //City
        //Description
        //ICollection<Image>
        //UserId
        //Category				//enumeracija(moze biti i odvojena tabela da bi bilo konfigurabilno)
        //Created					//datum objave
        //ICollection<Review>
        //ICollection<Comment>	//javna pitanja koja se mogu dodati na oglas
    }
}
