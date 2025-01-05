using Microsoft.AspNetCore.Mvc;
namespace EMGMSA.Controllers;
using Microsoft.EntityFrameworkCore;  // For EF Core extension methods like ToListAsync
using System.Linq;  // Fo
using EMGMSA.Seeder;
using EMGMSA.Models;
public class CarsController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly CarSeeder _carSeeder;

    public CarsController(ApplicationDbContext context, CarSeeder carSeeder)
    {
        _context = context;
        carSeeder.SeedCars();
    }

    public async Task<IActionResult> Index()
    {
        var cars = await _context.Cars.ToListAsync();
        return View(cars);
    }

    // Ajouter une voiture
    // [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Car car)
    {
        if (ModelState.IsValid)
        {
            _context.Add(car);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Cars");
        }
        else
        {

            foreach (var modelState in ViewData.ModelState.Values)
            {
                foreach (var error in modelState.Errors)
                {
                    System.Console.WriteLine(car);
                }

            }
            Console.WriteLine($"Brand: {car.Brand}, Model: {car.Model}, Year: {car.Year}, Price: {car.Price}");

        }
        return View(car);
    }

    // Modifier une voiture
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var car = await _context.Cars.FindAsync(id);
        if (car == null)
        {
            return NotFound();
        }
        return View(car);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Car car)
    {
        if (id != car.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(car);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CarExists(car.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        return View(car);
    }

    // Supprimer une voiture
    public async Task<IActionResult> Delete(int? id)
    {
        Console.WriteLine("Delete the in the first delete: " + id);

        if (id == null)
        {
            return NotFound();
        }

        var car = await _context.Cars
            .FirstOrDefaultAsync(m => m.Id == id);
        if (car == null)
        {
            return NotFound();
        }


        return PartialView(car);
    }

    [HttpPost, ActionName("DeleteConfirmed")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {           Console.WriteLine("DeleteConfirmed the : " + id);

        var car = await _context.Cars.FindAsync(id);
        _context.Cars.Remove(car);
        await _context.SaveChangesAsync();
        return RedirectToAction("Index", "Cars"); ;
    }

    private bool CarExists(int id)
    {
        return _context.Cars.Any(e => e.Id == id);
    }


}