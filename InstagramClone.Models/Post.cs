using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstagramClone.Models
{
    public class Post
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Description { get; set; }
        public virtual User User { get; set; }

    }
}
