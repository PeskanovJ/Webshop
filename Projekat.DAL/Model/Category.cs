using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekat.DAL.Model
{
    public class Category:Entity
    {
        [Required]
        public string Name { get; set; }

    }
}
