using FinancePlannerAPI.Controllers;
using FinancePlannerAPI.Domain.Entities;
using FinancePlannerAPI.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FinancePlannerAPI.UnitTests
{
    [TestClass]
    public class CategoriesControllerTests
    {
        [TestMethod]
        public async Task GetCategoryById_Success()
        {
            // Arrange
            var categoryId = Guid.NewGuid();
            var category = new Category
            {
                Id = categoryId,
                UserId = Guid.NewGuid(),
                Name = "Test Category"
            };

            var serviceMock = new Mock<ICategoryService>();
            serviceMock.Setup(x => x.GetCategoryByIdAsync(categoryId)).ReturnsAsync(category);

            var controller = new CategoriesController(serviceMock.Object);

            // Act
            var result = await controller.GetCategoryById(categoryId);

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

            var serviceMock = new Mock<ICategoryService>();
            serviceMock.Setup(x => x.GetCategoryByIdAsync(categoryId)).ReturnsAsync((Category)null);

            var controller = new CategoriesController(serviceMock.Object);

            // Act
            var result = await controller.GetCategoryById(categoryId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
        }

        [TestMethod]
        public async Task GetCategoryById_InternalServerError()
        {
            // Arrange
            var categoryId = Guid.NewGuid();

            var serviceMock = new Mock<ICategoryService>();
            serviceMock.Setup(x => x.GetCategoryByIdAsync(categoryId))
                       .ThrowsAsync(new Exception("Internal server error"));

            var controller = new CategoriesController(serviceMock.Object);

            // Act
            var result = await controller.GetCategoryById(categoryId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(StatusCodeResult));
            Assert.AreEqual(StatusCodes.Status500InternalServerError, (result as StatusCodeResult)?.StatusCode);
        }
    }
}
