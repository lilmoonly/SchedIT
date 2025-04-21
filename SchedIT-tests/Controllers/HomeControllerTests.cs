using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using MyMvcApp.Controllers;
using MyMvcApp.Models;
using Xunit;

namespace HomeControllerTests
{
    public class HomeControllerTests
    {
        private Mock<UserManager<ApplicationUser>> _mockUserManager;
        private Mock<SignInManager<ApplicationUser>> _mockSignInManager;

        public HomeControllerTests()
        {
            var userStoreMock = new Mock<IUserStore<ApplicationUser>>();

            _mockUserManager = new Mock<UserManager<ApplicationUser>>(
                userStoreMock.Object,
                null, null, null, null, null, null, null, null);

            var contextAccessorMock = new Mock<IHttpContextAccessor>();
            var claimsFactoryMock = new Mock<IUserClaimsPrincipalFactory<ApplicationUser>>();

            _mockSignInManager = new Mock<SignInManager<ApplicationUser>>(
                _mockUserManager.Object,
                contextAccessorMock.Object,
                claimsFactoryMock.Object,
                null, null, null, null);
        }

        private HomeController GetController()
        {
            var logger = new LoggerFactory().CreateLogger<HomeController>();
            return new HomeController(_mockSignInManager.Object, _mockUserManager.Object);
        }

        // Test for Index (GET)
        [Fact]
        public void Index_Get_ReturnsViewWithLoginViewModel()
        {
            var controller = GetController();
            var result = controller.Index() as ViewResult;

            Assert.NotNull(result);
            Assert.IsType<LoginViewModel>(result.Model);
        }

        // Test for Index (POST) with valid login
        [Fact]
        public async void Index_Post_ValidLogin_RedirectsToHome()
        {
            var controller = GetController();
            var loginViewModel = new LoginViewModel { Email = "test@example.com", Password = "Password123" };

            _mockSignInManager
                .Setup(m => m.PasswordSignInAsync(loginViewModel.Email, loginViewModel.Password, false, false))
                .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);

            _mockUserManager
                .Setup(m => m.FindByEmailAsync(loginViewModel.Email))
                .ReturnsAsync(new ApplicationUser());

            _mockUserManager
                .Setup(m => m.GetRolesAsync(It.IsAny<ApplicationUser>()))
                .ReturnsAsync(new[] { "User" });

            var result = await controller.Index(loginViewModel) as RedirectToActionResult;

            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            Assert.Equal("Home", result.ControllerName);
        }

        // Test for Index (POST) with invalid login
        [Fact]
        public async void Index_Post_InvalidLogin_ReturnsViewWithModelError()
        {
            var controller = GetController();
            var loginViewModel = new LoginViewModel { Email = "test@example.com", Password = "WrongPassword" };

            _mockSignInManager
                .Setup(m => m.PasswordSignInAsync(loginViewModel.Email, loginViewModel.Password, false, false))
                .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Failed);

            var result = await controller.Index(loginViewModel) as ViewResult;

            Assert.NotNull(result);
            Assert.IsType<LoginViewModel>(result.Model);
            Assert.False(controller.ModelState.IsValid);
        }
    }
}