using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyMvcApp.Controllers;
using MyMvcApp.Models;
using Microsoft.AspNetCore.Http;
using Xunit;

namespace HomeControllerTests
{
    public class HomeControllerTests
    {
        private HomeController GetController()
        {
            var logger = new LoggerFactory().CreateLogger<HomeController>();
            return new HomeController(logger);
        }

        // This test verifies that Index() returns a ViewResult
        [Fact]
        public void Index_ReturnsView()
        {
            var controller = GetController();
            var result = controller.Index();
            Assert.IsType<ViewResult>(result);
        }

        // This test verifies that Privacy() returns a ViewResult
        [Fact]
        public void Privacy_ReturnsView()
        {
            var controller = GetController();
            var result = controller.Privacy();
            Assert.IsType<ViewResult>(result);
        }

        // This test verifies that Error() returns a ViewResult with an ErrorViewModel
        [Fact]
        public void Error_ReturnsViewWithErrorViewModel()
        {
            var controller = GetController();

            // Mock HttpContext with TraceIdentifier
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            var result = controller.Error() as ViewResult;

            Assert.NotNull(result);
            var model = Assert.IsType<ErrorViewModel>(result.Model);
            Assert.False(string.IsNullOrEmpty(model.RequestId));
        }

    }
}