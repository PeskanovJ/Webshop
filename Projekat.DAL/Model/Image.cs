using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Projekat.DAL.Model
{
    public class Image
    {
        public int Order { get; set; }
        public bool IsMainImage { get; set; }
        [ValidateNever]
        public string Url { get; set; }
    }
}
