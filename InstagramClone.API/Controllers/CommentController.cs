using InstagramClone.API.DTOs;
using InstagramClone.Models;
using InstagramClone.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InstagramClone.API.Controllers
{
    // Controlador CommentController
    [Authorize]
    [ApiController]
    [Route("api/comments")]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;
        private readonly IUserService _userService;

        public CommentController(ICommentService commentService, IUserService userService)
        {
            _commentService = commentService;
            _userService = userService;
        }

        [HttpPost]
        public IActionResult CreateComment([FromBody] CommentDTO commentDto)
        {
            var userId = _userService.GetUserIdFromClaims(User.Claims);
            if (userId == null)
            {
                return Unauthorized();
            }
            // Establezco esto estilo twitter
            if (commentDto.Text.Length > 140)
            {
                commentDto.Text = commentDto.Text.Substring(0, 140);
            }
            var createdComment = _commentService.CreateComment(commentDto.PostId, userId.Value, commentDto.Text);

            if (createdComment == null)
            {
                return BadRequest("No se pudo crear el comentario.");
            }

            return Ok(createdComment);
        }

        [HttpGet("{commentId}")]
        public IActionResult GetComment(int commentId)
        {
            var comment = _commentService.GetCommentById(commentId);
            if (comment == null)
            {
                return NotFound("No se hallo el comentario.");
            }
            return Ok(comment);
        }

        [HttpPut("{commentId}")]
        public IActionResult UpdateComment(int commentId, [FromBody] CommentDTO commentDto)
        {
            var updatedComment = _commentService.UpdateComment(commentId, commentDto.Text);
            if (updatedComment == null)
            {
                return NotFound("No se hallo el comentario o no se ha podido actualizar.");
            }

            return Ok(updatedComment);
        }

        [HttpDelete("{commentId}")]
        public IActionResult DeleteComment(int commentId)
        {
            var result = _commentService.DeleteComment(commentId);
            if (!result)
            {
                return NotFound("No se hallo el comentario o no se ha podido ekiminar.");
            }

            return Ok("Comentario eliminado con éxito.");
        }
    }

}
