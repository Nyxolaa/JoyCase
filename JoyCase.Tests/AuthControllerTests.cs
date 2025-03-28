using JoyCase.Application;
using JoyCase.Application.User.Dto;
using JoyCase.Application.User.Query.LoginUserQuery;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

namespace JoyCase.Tests
{
    public class AuthControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly AuthController _controller;

        public AuthControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _configurationMock = new Mock<IConfiguration>();
            _controller = new AuthController(_configurationMock.Object, _mediatorMock.Object);
        }

        [Fact]
        public async Task Login_ShouldReturnToken_WhenValidUser()
        {
            var request = new LoginUserQuery { /* kullanıcı bilgileri */ };
            var response = new Response<LoginUserDto>
            {
                Data = new LoginUserDto
                {
                    UserId = 1,
                    RoleId = 2,
                    UserPermissions = new List<UserPermissionDto> { new UserPermissionDto { Name = "Read" } },
                    RolePermissions = new List<RolePermissionDto> { new RolePermissionDto { Name = "Write" } }
                }
            };

            // Mocking the Mediator call
            _mediatorMock.Setup(m => m.Send(It.IsAny<LoginUserQuery>(), default)).ReturnsAsync(response);

            // Mock JWT Settings
            var jwtSettings = new Mock<IConfigurationSection>();
            jwtSettings.Setup(s => s["SecretKey"]).Returns("ÇokGizliVeGüçlüBirŞifre123!");
            jwtSettings.Setup(s => s["Issuer"]).Returns("MyIssuer");
            jwtSettings.Setup(s => s["Audience"]).Returns("MyAudience");
            jwtSettings.Setup(s => s["ExpiryInMinutes"]).Returns("60");

            _configurationMock.Setup(c => c.GetSection("JwtSettings")).Returns(jwtSettings.Object);

            var result = await _controller.Login(request);
            var actionResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(actionResult);

            // İçeriği kontrol etmek için key-value çiftini ele alalım
            var value = actionResult.Value as IDictionary<string, object>;

            // actionResult.Value'ı kontrol et
            Console.WriteLine("value type !!!", value.GetType().FullName);
            Assert.NotNull(value);

            Assert.True(value.ContainsKey("token"));
            Assert.True(value.ContainsKey("expiresIn"));

            Assert.NotNull(value["token"]);
            Assert.NotNull(value["expiresIn"]);

        }

    }

}
