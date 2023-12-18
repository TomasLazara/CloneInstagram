using InstagramClone.API.Controllers;
using InstagramClone.API.DTOs;
using InstagramClone.Models;
using InstagramClone.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
namespace InstagramClone.Tests
{
    public class AuthTests
    {
        private readonly Mock<IUserService> _mockUserService;
        private readonly AuthController _controller;

        public AuthTests()
        {
            _mockUserService = new Mock<IUserService>();
            _controller = new AuthController(_mockUserService.Object);
        }

        [Fact]
        public void SignUp_ConUsuarioNuevo_RetornaOk()
        {
            var userDto = new UserDTO { Username = "TomasNuevo", Password = "contraseña", NickName = "tom" };
            _mockUserService.Setup(us => us.Register(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(new User());

            var result = _controller.SignUp(userDto);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void SignUp_ConUsuarioExistente_RetornaBadRequest()
        {
            var userDto = new UserDTO { Username = "TomasExistente", Password = "contraseña", NickName = "tom" };
            _mockUserService.Setup(us => us.Register(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns((User)null);

            var result = _controller.SignUp(userDto);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void SignIn_ConCredencialesValidas_RetornaOk()
        {
            var userDto = new UserDTO { Username = "TomasValido", Password = "contraseñaCorrecta" };
            _mockUserService.Setup(us => us.Authenticate(It.IsAny<string>(), It.IsAny<string>())).Returns("tokenValido");

            var result = _controller.SignIn(userDto);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void SignIn_ConCredencialesInvalidas_RetornaUnauthorized()
        {
            var userDto = new UserDTO { Username = "TomasInvalido", Password = "contraseñaIncorrecta" };
            _mockUserService.Setup(us => us.Authenticate(It.IsAny<string>(), It.IsAny<string>())).Returns(string.Empty);

            var result = _controller.SignIn(userDto);

            Assert.IsType<UnauthorizedObjectResult>(result);
        }
    }
}
