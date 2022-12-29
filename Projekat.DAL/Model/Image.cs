using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projekat.DAL.Model
{
    public class Image:Entity
    {
        public int Order { get; set; }
        public bool IsMainImage { get; set; }
        [ValidateNever]
        public string Url { get; set; }

        [ForeignKey("ItemId")]
        public Item Item { get; set; }

    }
}
