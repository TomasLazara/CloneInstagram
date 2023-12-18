using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstagramClone.Services.Interfaces
{
    public interface IUserFollowService
    {
       public bool FollowUser(int followerId, int followeeId);
       public bool UnfollowUser(int followerId, int followeeId);
       public List<int> GetFollowers(int userId);
       public List<int> GetFollowing(int userId);
       public bool IsFollowingUser(int followerId, int followeeId);     

    }
}
