using InstagramClone.API.DTOs;
using InstagramClone.Models;
using InstagramClone.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace InstagramClone.API.Controllers
{
    [ApiController]
    [Route("api/posts")]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;
        private readonly IUserService _userService;

        public PostController(IPostService postService, IUserService userService)
        {
            _postService = postService;
            _userService = userService;
        }

        [HttpPost]
        public IActionResult CreatePost([FromBody] PostDTO postDto)
        {
            var userId = _userService.GetUserIdFromClaims(User.Claims);
            if (userId == null)
            {
                return Unauthorized();
            }

            var post = new Post();
            post.UserId = userId.Value;
            post.Description = postDto.Description;
            var createdPost = _postService.CreatePost(userId.Value, post);

            if (createdPost == null)
            {
                return BadRequest("No se pudo crear la publicación");
            }

            return Ok(createdPost);
        }

        [HttpPost("{postId}/like")]
        public IActionResult LikePost(int postId)
        {
            var userId = _userService.GetUserIdFromClaims(User.Claims);
            if (userId == null)
            {
                return Unauthorized();
            }
            var liked = _postService.LikePost(postId, userId.Value);

            if (liked)
            {
                return Ok("Like agregado.");
            }
            else
            {
                return UnlikePost(postId);
            }
        }

        [HttpPost("{postId}/unlike")]
        public IActionResult UnlikePost(int postId)
        {
            var userId = _userService.GetUserIdFromClaims(User.Claims);
            if (userId == null)
            {
                return Unauthorized();
            }
            var unliked = _postService.UnlikePost(postId, userId.Value);

            if (unliked)
            {
                return Ok("Like eliminado con éxito.");
            }
            else
            {
                return BadRequest("El usuario no ha dado like a esta publicación.");
            }
        }
    }

}
