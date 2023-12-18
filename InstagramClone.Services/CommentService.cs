using InstagramClone.Models;
using InstagramClone.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstagramClone.Services
{
    public class CommentService : ICommentService
    {
        private readonly ApplicationDbContext _dbContext;
        public CommentService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Comment CreateComment(int postId, int userId, string text)
        {
            var comment = new Comment
            {
                PostId = postId,
                UserId = userId,
                Text = text,
                CreatedAt = DateTime.Now
            };
            _dbContext.Comments.Add(comment);
            _dbContext.SaveChanges();
            return comment;
        }

        public Comment GetCommentById(int commentId)
        {
            return _dbContext.Comments.FirstOrDefault(c => c.Id == commentId);
        }

        public Comment UpdateComment(int commentId, string newText)
        {
            var comment = _dbContext.Comments.FirstOrDefault(c => c.Id == commentId);
            if (comment == null)
            {
                return null;
            }

            comment.Text = newText;
            comment.UpdatedAt = DateTime.Now;

            _dbContext.SaveChanges();

            return comment;
        }

        public bool DeleteComment(int commentId)
        {
            var comment = _dbContext.Comments.FirstOrDefault(c => c.Id == commentId);
            if (comment == null)
            {
                return false;
            }

            _dbContext.Comments.Remove(comment);
            _dbContext.SaveChanges();

            return true;
        }

        public List<Comment> GetCommentsByPost(int postId)
        {
            return _dbContext.Comments.Where(c => c.PostId == postId).ToList();
        }
    }
}
