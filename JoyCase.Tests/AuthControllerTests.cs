using JoyCase.Application.User.Query.LoginUserQuery;
using JoyCase.Application.User.Dto;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;
using JoyCase.Api.Log;
using JoyCase.Application;

namespace JoyCase.Tests
{
    public class AuthControllerTests
    {
        private readonly AuthController _controller;
        private readonly Mock<IMediator> _mediatorMock;
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly Mock<LogService> _logServiceMock;

        public AuthControllerTests()
        {
            // Mock'ları başlat
            _mediatorMock = new Mock<IMediator>();
            _configurationMock = new Mock<IConfiguration>();
            _logServiceMock = new Mock<LogService>();

            // AuthController'ı başlat
            _controller = new AuthController(
                _configurationMock.Object,
                _mediatorMock.Object
            );
        }

        [Fact]
        public async Task Login_InvalidCredentials_ReturnsNotFoundWithErrorMessage()
        {
            // Arrange
            var request = new LoginUserQuery
            {
                Username = "wrongUser",
                Password = "wrongPassword"
            };

            // Geçersiz kullanıcı adı veya şifre ile dönecek response
            var response = new Response<LoginUserDto>
            {
                Data = null,
                Errors = new[] { "Geçersiz kullanıcı adı veya şifre" }
            };

            // _mediator mock'laması
            _mediatorMock
                .Setup(m => m.Send(It.IsAny<LoginUserQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            // Act
            var result = await _controller.Login(request);

            // Assert
            var actionResult = Assert.IsType<NotFoundObjectResult>(result); // NotFound bekliyoruz, Unauthorized değil.
            Assert.NotNull(actionResult);

            var errorMessage = Assert.IsType<string[]>(actionResult.Value);  // Hata mesajı bir dizi olacak
            Assert.Contains("Geçersiz kullanıcı adı veya şifre", errorMessage); // Hata mesajı doğrulaması
        }

    }
}
