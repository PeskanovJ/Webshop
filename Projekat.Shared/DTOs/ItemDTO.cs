using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekat.Shared.DTOs
{
    public class ItemDTO
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Make { get; set; }
        [Required]
        public string Model { get; set; }
        public int? Price { get; set; }
        public string City { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; }
        [Required]
        public int UserId { get; set; }

        [Required]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        [ValidateNever]
        public ICollection<ImageDTO>? Images { get; set; }


        public string getMainImage()
        {
            foreach(var image in Images)
            {
                if (image.IsMainImage)
                    return image.Url;
            }

            return "";
        }

    }
}
