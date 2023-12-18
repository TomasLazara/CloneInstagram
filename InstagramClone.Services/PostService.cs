using InstagramClone.Models;
using InstagramClone.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstagramClone.Services
{
    public class PostService : IPostService
    {
        private readonly ApplicationDbContext _dbContext;

        public PostService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Post CreatePost(int userId, Post post)
        {
            post.UserId = userId;
            _dbContext.Posts.Add(post);
            _dbContext.SaveChanges();
            return post;
        }

        public Post UpdatePost(int postId, Post post)
        {
            var existingPost = _dbContext.Posts.Find(postId);

            if (existingPost == null)
            {
                return null;
            }
            existingPost.Description = post.Description;
            _dbContext.SaveChanges();
            return existingPost;
        }
        public bool DeletePost(int postId)
        {
            var existingPost = _dbContext.Posts.Find(postId);

            if (existingPost == null)
            {
                return false;
            }
            _dbContext.Posts.Remove(existingPost);
            _dbContext.SaveChanges();

            return true;
        }
        public bool LikePost(int postId, int userId)
        {
            if (_dbContext.PostLikes.Any(pl => pl.PostId == postId && pl.UserId == userId))
            {
                return false; // no se puede dar like duplicado
            }
            var postLike = new PostLike
            {
                PostId = postId,
                UserId = userId
            };
            _dbContext.PostLikes.Add(postLike);
            _dbContext.SaveChanges();
            return true;
        }
        public bool UnlikePost(int postId, int userId)
        {
            var postLike = _dbContext.PostLikes.FirstOrDefault(pl => pl.PostId == postId && pl.UserId == userId);
            if (postLike == null)
            {
                return false; //no se quita el like porque nunca dio like
            }
            _dbContext.PostLikes.Remove(postLike);
            _dbContext.SaveChanges();

            return true;
        }
    }

}
