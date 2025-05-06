using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System;
using System.Linq;
using MyMvcApp.Controllers;
using MyMvcApp.Data;
using MyMvcApp.Models;

namespace ScheduleControllerTests
{
    public class ScheduleControllerTests
    {
        private AppDbContext _context;
        private Mock<UserManager<ApplicationUser>> _userManagerMock;
        private ScheduleController _controller;

        public ScheduleControllerTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(options);
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            var store = new Mock<IUserStore<ApplicationUser>>();
            _userManagerMock = new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);

            _controller = new ScheduleController(_context, _userManagerMock.Object);
        }

        [Fact]
        public async Task RateSchedule_RatingTooHigh_ReturnsRedirectWithError()
        {
            // Act
            var result = await _controller.RateSchedule(10, 1) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            Assert.False(_controller.ModelState.IsValid);
        }

        [Fact]
        public async Task RateSchedule_NewRating_AddsRating()
        {
            // Arrange
            var userId = "user1";
            _userManagerMock.Setup(u => u.GetUserId(It.IsAny<ClaimsPrincipal>())).Returns(userId);

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal() }
            };

            // Act
            await _controller.RateSchedule(4, 101);

            // Assert
            var rating = _context.ScheduleRatings.FirstOrDefault(r => r.UserId == userId && r.ScheduleId == 101);
            Assert.NotNull(rating);
            Assert.Equal(4, rating.Rating);
        }

        [Fact]
        public async Task RateSchedule_ExistingRating_UpdatesIt()
        {
            // Arrange
            var userId = "user2";

            _context.ScheduleRatings.Add(new ScheduleRating
            {
                UserId = userId,
                ScheduleId = 102,
                Rating = 2,
                CreatedAt = DateTime.UtcNow.AddDays(-1)
            });
            await _context.SaveChangesAsync();

            _userManagerMock.Setup(u => u.GetUserId(It.IsAny<ClaimsPrincipal>())).Returns(userId);

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal() }
            };

            // Act
            await _controller.RateSchedule(5, 102);

            // Assert
            var rating = _context.ScheduleRatings.FirstOrDefault(r => r.UserId == userId && r.ScheduleId == 102);
            Assert.NotNull(rating);
            Assert.Equal(5, rating.Rating);
        }
    }
}
