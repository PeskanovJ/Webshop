using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Projekat.Shared.DTOs
{
    public class ItemDTO
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
        public string Category { get; set; }
        //[ValidateNever]
        //ICollection<Image>? Images { get; set; }
        //[Required]

        //public DateTime Created { get; set; }
        //ICollection<Review>? Reviews { get; set; }
        //ICollection<Comment>? Comments { get; set; }
    }
}
