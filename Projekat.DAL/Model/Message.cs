using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekat.DAL.Model
{
    public class Message:Entity
    {
        public int SenderId { get; set; }
        public int ReciverId { get; set; }
        public string Content { get; set; }
        public bool IsRead { get; set; }
        public DateTime Created { get; set; }
    }
}
