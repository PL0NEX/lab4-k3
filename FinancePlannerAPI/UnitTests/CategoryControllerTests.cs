using FinancePlannerAPI.Controllers;
using FinancePlannerAPI.Domain.Entities;
using FinancePlannerAPI.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FinancePlannerAPI.UnitTests
{
    [TestClass]
    public class CategoryControllerTests
    {
        private Mock<ICategoryService> _serviceMock;
        private Mock<ILogger<CategoryController>> _loggerMock;
        private CategoryController _controller;

        [TestInitialize]
        public void Setup()
        {
            _serviceMock = new Mock<ICategoryService>();
            _loggerMock = new Mock<ILogger<CategoryController>>();
            _controller = new CategoryController(_serviceMock.Object, _loggerMock.Object);
        }

        [TestMethod]
        public async Task GetCategoryById_Success()
        {
            // Arrange
            var categoryId = Guid.NewGuid();
            var category = new Category
            {
                Id = categoryId,
                UserId = Guid.NewGuid(),
                Name = "Tестовая категория"
            };

            _serviceMock.Setup(x => x.GetCategoryByIdAsync(categoryId)).ReturnsAsync(category);

            // Act
            var result = await _controller.GetCategoryById(categoryId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(category, okResult.Value);
        }

        [TestMethod]
        public async Task GetCategoryById_NotFound()
        {
            // Arrange
            var categoryId = Guid.NewGuid();
            _serviceMock.Setup(x => x.GetCategoryByIdAsync(categoryId)).ReturnsAsync((Category)null);

            // Act
            var result = await _controller.GetCategoryById(categoryId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
            var notFoundResult = result as NotFoundObjectResult;
            Assert.AreEqual("Категория не найдена", notFoundResult?.Value);
        }

        [TestMethod]
        public async Task GetCategoryById_InternalServerError()
        {
            // Arrange
            var categoryId = Guid.NewGuid();
            _serviceMock.Setup(x => x.GetCategoryByIdAsync(categoryId))
                        .ThrowsAsync(new Exception("Internal server error"));

            // Act
            var result = await _controller.GetCategoryById(categoryId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ObjectResult));
            var objectResult = result as ObjectResult;
            Assert.AreEqual(StatusCodes.Status500InternalServerError, objectResult?.StatusCode);
            Assert.AreEqual("Произошла неизвестная ошибка", objectResult?.Value);
        }
    }
}
