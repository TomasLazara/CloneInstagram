using InstagramClone.API.Controllers;
using InstagramClone.API.DTOs;
using InstagramClone.Models;
using InstagramClone.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Security.Claims;
using Xunit;

namespace InstagramClone.Tests
{
    public class PostTests
    {
        private readonly Mock<IPostService> _mockPostService;
        private readonly Mock<IUserService> _mockUserService;
        private readonly PostController _controller;
        private readonly ClaimsPrincipal _user;

        public PostTests()
        {
            _mockPostService = new Mock<IPostService>();
            _mockUserService = new Mock<IUserService>();
            _controller = new PostController(_mockPostService.Object, _mockUserService.Object);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, "1"),           
            };
            _user = new ClaimsPrincipal(new ClaimsIdentity(claims, "TestAuthentication"));

            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = _user }
            };
        }
        [Fact]
        public void CreatePost_ConUsuarioValido_RetornaOk()
        {
            var postDto = new PostDTO { Description = "desc de la publicación" };
            _mockUserService.Setup(us => us.GetUserIdFromClaims(It.IsAny<IEnumerable<Claim>>())).Returns(1);
            _mockPostService.Setup(ps => ps.CreatePost(It.IsAny<int>(), It.IsAny<Post>())).Returns(new Post());

            var result = _controller.CreatePost(postDto);

            Assert.IsType<OkObjectResult>(result);
        }
        [Fact]
        public void CreatePost_ConUsuarioNoValido_RetornaNoAutorizado()
        {
            var postDto = new PostDTO { Description = "desc de la publicación" };
            _mockUserService.Setup(us => us.GetUserIdFromClaims(It.IsAny<IEnumerable<Claim>>())).Returns((int?)null);

            var result = _controller.CreatePost(postDto);

            Assert.IsType<UnauthorizedResult>(result);
        }
        [Fact]
        public void LikePost_UsuarioDaLike_RetornaOk()
        {
            _mockUserService.Setup(us => us.GetUserIdFromClaims(It.IsAny<IEnumerable<Claim>>())).Returns(1);
            _mockPostService.Setup(ps => ps.LikePost(It.IsAny<int>(), It.IsAny<int>())).Returns(true);

            var result = _controller.LikePost(1);

            Assert.IsType<OkObjectResult>(result);
        }
        [Fact]
        public void LikePost_UsuarioNoDaLike_RetornaLlamadaAUnlikePost()
        {
            _mockUserService.Setup(us => us.GetUserIdFromClaims(It.IsAny<IEnumerable<Claim>>())).Returns(1);
            _mockPostService.Setup(ps => ps.LikePost(It.IsAny<int>(), It.IsAny<int>())).Returns(false);
            _mockPostService.Setup(ps => ps.UnlikePost(It.IsAny<int>(), It.IsAny<int>())).Returns(true);

            var result = _controller.LikePost(1);

            Assert.IsType<OkObjectResult>(result);
        }
        [Fact]
        public void UnlikePost_UsuarioEliminaLike_RetornaOk()
        {
            _mockUserService.Setup(us => us.GetUserIdFromClaims(It.IsAny<IEnumerable<Claim>>())).Returns(1);
            _mockPostService.Setup(ps => ps.UnlikePost(It.IsAny<int>(), It.IsAny<int>())).Returns(true);

            var result = _controller.UnlikePost(1);

            Assert.IsType<OkObjectResult>(result);
        }
        [Fact]
        public void UnlikePost_UsuarioNoEliminaLike_RetornaBadRequest()
        {
            _mockUserService.Setup(us => us.GetUserIdFromClaims(It.IsAny<IEnumerable<Claim>>())).Returns(1);
            _mockPostService.Setup(ps => ps.UnlikePost(It.IsAny<int>(), It.IsAny<int>())).Returns(false);

            var result = _controller.UnlikePost(1);

            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}
