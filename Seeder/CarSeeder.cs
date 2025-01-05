using Microsoft.EntityFrameworkCore;

namespace EMGMSA.Seeder;
using EMGMSA.Models;
public class CarSeeder
{
    private readonly ApplicationDbContext _context;

    public CarSeeder(ApplicationDbContext context)
    {
        _context = context;
    }

    public void SeedCars()
    {   
        if (_context.Cars.Any())
        {
            return; // Ne pas *seeder* si des données existent
        }
        var cars = new List<Car>
        {
            new Car
            {
                Brand = "Toyota",
                Model = "Corolla",
                Year = 2020,
                Price = 15000.00m,
                IsAvailable = true,
                Description = "Une voiture fiable et économique.",
                PhotoUrl = "http://example.com/images/toyota-corolla.jpg"
            },
            new Car
            {
                Brand = "Honda",
                Model = "Civic",
                Year = 2021,
                Price = 18000.00m,
                IsAvailable = false,
                Description = "Compacte et économe en carburant.",
                PhotoUrl = "http://example.com/images/honda-civic.jpg"
            },
            new Car
            {
                Brand = "BMW",
                Model = "X5",
                Year = 2022,
                Price = 45000.00m,
                IsAvailable = true,
                Description = "SUV de luxe, idéal pour les familles.",
                PhotoUrl = "http://example.com/images/bmw-x5.jpg"
            },
            new Car
            {
                Brand = "Ford",
                Model = "Focus",
                Year = 2020,
                Price = 12000.00m,
                IsAvailable = true,
                Description = "Voiture compacte et pratique.",
                PhotoUrl = "http://example.com/images/ford-focus.jpg"
            },
            new Car
            {
                Brand = "Audi",
                Model = "A3",
                Year = 2021,
                Price = 25000.00m,
                IsAvailable = true,
                Description = "Voiture sportive et élégante.",
                PhotoUrl = "http://example.com/images/audi-a3.jpg"
            }
        };

        _context.Cars.AddRange(cars);
        _context.SaveChanges();
    }
}
