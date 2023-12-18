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
    public class CommentTests
    {
        private readonly Mock<ICommentService> _mockCommentService;
        private readonly Mock<IUserService> _mockUserService;
        private readonly CommentController _controller;
        private readonly ClaimsPrincipal _user;

        public CommentTests()
        {
            _mockCommentService = new Mock<ICommentService>();
            _mockUserService = new Mock<IUserService>();
            _controller = new CommentController(_mockCommentService.Object, _mockUserService.Object);
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
        public void CreateComment_WithValidComment_ReturnsOk()
        {
            // Arrange
            var commentDto = new CommentDTO { Text = "comment testeo", PostId = 1 };
            _mockUserService.Setup(us => us.GetUserIdFromClaims(It.IsAny<IEnumerable<Claim>>())).Returns(1);
            _mockCommentService.Setup(cs => cs.CreateComment(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>())).Returns(new Comment());

            // Act
            var result = _controller.CreateComment(commentDto);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void CreateComment_WithInvalidUserId_ReturnsUnauthorized()
        {
            // Arrange
            var commentDto = new CommentDTO { Text = "comment testeo", PostId = 1 };
            _mockUserService.Setup(us => us.GetUserIdFromClaims(It.IsAny<IEnumerable<Claim>>())).Returns((int?)null);

            // Act
            var result = _controller.CreateComment(commentDto);

            // Assert
            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public void GetComment_CommentExists_ReturnsOk()
        {
            // Arrange
            var comment = new Comment();
            _mockCommentService.Setup(cs => cs.GetCommentById(It.IsAny<int>())).Returns(comment);

            // Act
            var result = _controller.GetComment(1);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void GetComment_CommentDoesNotExist_ReturnsNotFound()
        {
            // Arrange
            _mockCommentService.Setup(cs => cs.GetCommentById(It.IsAny<int>())).Returns((Comment)null);

            // Act
            var result = _controller.GetComment(1);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void UpdateComment_ValidComment_ReturnsOk()
        {
            // Arrange
            var commentDto = new CommentDTO { Text = "Actualizo el comment" };
            _mockCommentService.Setup(cs => cs.UpdateComment(It.IsAny<int>(), It.IsAny<string>())).Returns(new Comment());

            // Act
            var result = _controller.UpdateComment(1, commentDto);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void UpdateComment_InvalidComment_ReturnsNotFound()
        {
            // Arrange
            var commentDto = new CommentDTO { Text = "Actualizo el comment" };
            _mockCommentService.Setup(cs => cs.UpdateComment(It.IsAny<int>(), It.IsAny<string>())).Returns((Comment)null);

            // Act
            var result = _controller.UpdateComment(1, commentDto);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void DeleteComment_CommentExists_ReturnsOk()
        {
            // Arrange
            _mockCommentService.Setup(cs => cs.DeleteComment(It.IsAny<int>())).Returns(true);

            // Act
            var result = _controller.DeleteComment(1);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void DeleteComment_CommentDoesNotExist_ReturnsNotFound()
        {
            // Arrange
            _mockCommentService.Setup(cs => cs.DeleteComment(It.IsAny<int>())).Returns(false);

            // Act
            var result = _controller.DeleteComment(1);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }
    }
}
