using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekat.Shared.DTOs
{
    public class ImageDTO
    {
        public int Order { get; set; }
        public bool IsMainImage { get; set; }
        [ValidateNever]
        public string Url { get; set; }
    }
}
