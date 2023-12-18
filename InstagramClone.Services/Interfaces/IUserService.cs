using InstagramClone.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace InstagramClone.Services.Interfaces
{
    public interface IUserService
    {
       public User Register(string username, string password, string nickname);
       public string Authenticate(string username, string password);
       public int? GetUserIdFromClaims(IEnumerable<Claim> claims);
       public User GetUserById(int userId);      
    }

}
