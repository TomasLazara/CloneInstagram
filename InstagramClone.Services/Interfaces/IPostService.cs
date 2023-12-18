using InstagramClone.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstagramClone.Services.Interfaces
{
    public interface IPostService
    {
        public Post CreatePost(int userId, Post post);
        public Post UpdatePost(int postId, Post post);
        public bool DeletePost(int postId);
        public bool LikePost(int postId, int userId);
        public bool UnlikePost(int postId, int userId);
    }
}
