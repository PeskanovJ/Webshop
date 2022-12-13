using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekat.DAL.Model
{
    public class Comment:Entity
    {
        public int ItemId { get; set; }
        public int UserId { get; set; }
        public string Content { get; set; }
        public DateTime Created { get; set; }
    }
}
