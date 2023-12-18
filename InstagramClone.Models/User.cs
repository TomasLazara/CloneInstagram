using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstagramClone.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string NickName { get; set; } 
        public DateTime CreationDate { get; set; }
        public ICollection<User> Followers { get; set; }
        public ICollection<User> Following { get; set; }

    }

}
