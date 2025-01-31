using Xunit;
using Moq;
using EMGMSA.Controllers;
using EMGMSA.Models;
using EMGMSA.Seeder;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace EMGMSA.Tests
{
    public class CarsControllerTests : IDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly CarsController _controller;

        public CarsControllerTests()
        {
            // Setup in-memory database
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);

            // Add test data
            _context.Cars.AddRange(new List<Car>
            {
                new Car 
                { 
                    Id = 1, 
                    Brand = "Toyota", 
                    Model = "Camry", 
                    Year = 2020,
                    Price = 25000,
                    IsAvailable = true
                },
                new Car 
                { 
                    Id = 2, 
                    Brand = "Honda", 
                    Model = "Civic", 
                    Year = 2021,
                    Price = 22000,
                    IsAvailable = true
                }
            });
            _context.SaveChanges();

            // Pass null for CarSeeder since it's not used in the tests
            _controller = new CarsController(_context, null);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Fact]
        public async Task Index_ReturnsViewWithCarsList()
        {
            // Act
            var result = await _controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Car>>(viewResult.Model);
            Assert.Equal(2, model.Count());
        }

        [Fact]
        public void Create_ReturnsViewResult()
        {
            // Act
            var result = _controller.Create();

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task Create_WithValidModel_RedirectsToIndex()
        {
            // Arrange
            var car = new Car
            {
                Brand = "Tesla",
                Model = "Model 3",
                Year = 2023,
                Price = 45000,
                IsAvailable = true
            };

            // Act
            var result = await _controller.Create(car, null);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
            Assert.Equal("Cars", redirectResult.ControllerName);

            // Vérifier que la voiture a été ajoutée
            var addedCar = await _context.Cars.FirstOrDefaultAsync(c => c.Brand == "Tesla");
            Assert.NotNull(addedCar);
            Assert.Equal("Model 3", addedCar.Model);
        }

        [Fact]
        public async Task Edit_WithValidId_ReturnsViewWithCar()
        {
            // Act
            var result = await _controller.Edit(1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<Car>(viewResult.Model);
            Assert.Equal(1, model.Id);
            Assert.Equal("Toyota", model.Brand);
        }

        [Fact]
        public async Task Delete_WithValidId_ReturnsPartialViewWithCar()
        {
            // Act
            var result = await _controller.Delete(1);

            // Assert
            var partialViewResult = Assert.IsType<PartialViewResult>(result);
            var model = Assert.IsType<Car>(partialViewResult.Model);
            Assert.Equal(1, model.Id);
            Assert.Equal("Toyota", ((Car)model).Brand);
        }

        [Fact]
        public async Task DeleteConfirmed_RemovesCar()
        {
            // Act
            var result = await _controller.DeleteConfirmed(1);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
            
            // Vérifier que la voiture a été supprimée
            var deletedCar = await _context.Cars.FindAsync(1);
            Assert.Null(deletedCar);
        }
    }
}