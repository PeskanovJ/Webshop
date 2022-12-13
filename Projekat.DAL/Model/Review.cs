using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekat.DAL.Model
{
    public class Review : Entity
    {
        public int UserId { get; set; }
        public int ItemId { get; set; }
        public string? Content { get; set; }
        [Range(1, 5)]
        public int Rating { get; set; }

        public DateTime Created { get; set; }


        
    }
}
