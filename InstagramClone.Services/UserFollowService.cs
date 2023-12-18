using InstagramClone.Models;
using InstagramClone.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstagramClone.Services
{
    public class UserFollowService : IUserFollowService
    {
        private readonly ApplicationDbContext _dbContext;

        public UserFollowService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public bool FollowUser(int followerId, int followeeId)
        {
            if (_dbContext.UserFollows.Any(uf => uf.FollowerId == followerId && uf.FollowedId == followeeId))
            {
                return false; // Ya sigue al usuario
            }
            var userFollow = new UserFollow
            {
                FollowerId = followerId,
                FollowedId = followeeId
            };
            _dbContext.UserFollows.Add(userFollow);
            _dbContext.SaveChanges();
            return true;
        }
        public bool UnfollowUser(int followerId, int followeeId)
        {
            var userFollow = _dbContext.UserFollows.FirstOrDefault(uf => uf.FollowerId == followerId && uf.FollowedId == followeeId);
            if (userFollow == null)
            {
                return false; 
            }

            _dbContext.UserFollows.Remove(userFollow);
            _dbContext.SaveChanges();

            return true;
        }

        public List<int> GetFollowers(int userId)
        {
            return _dbContext.UserFollows.Where(uf => uf.FollowedId == userId).Select(uf => uf.FollowerId).ToList();
        }

        public List<int> GetFollowing(int userId)
        {
            return _dbContext.UserFollows.Where(uf => uf.FollowerId == userId).Select(uf => uf.FollowedId).ToList();
        }
        public bool IsFollowingUser(int followerId, int followeeId)
        {
            var isFollowing = _dbContext.UserFollows
                .Any(uf => uf.FollowerId == followerId && uf.FollowedId == followeeId);

            return isFollowing;
        }

    }
}
