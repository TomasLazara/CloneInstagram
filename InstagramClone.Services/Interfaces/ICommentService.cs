using InstagramClone.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstagramClone.Services.Interfaces
{
    public interface ICommentService
    {
       public Comment CreateComment(int postId, int userId, string text);
       public Comment GetCommentById(int commentId);
       public Comment UpdateComment(int commentId, string newText);
      public bool DeleteComment(int commentId);
      public List<Comment> GetCommentsByPost(int postId);
    }
}
