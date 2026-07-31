using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MvcCrudProject.Data;
using MvcCrudProject.Extensions;
using MvcCrudProject.Models;
using MvcCrudProject.ViewModels;

namespace MvcCrudProject.Controllers;

public class CityController : Controller
{
    private readonly AppDbContext _context;
    public CityController(AppDbContext context) => _context = context;

    public async Task<IActionResult> Index() =>
        View((await _context.Cities.Include(c => c.Country).ToListAsync()).ToViewModelList());

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();
        var city = await _context.Cities.Include(c => c.Country).FirstOrDefaultAsync(m => m.CityId == id);
        if (city == null) return NotFound();
        return View(city.ToViewModel());
    }

    public IActionResult Create()
    {
        ViewBag.CountryId = new SelectList(_context.Countries, "CountryId", "CountryName");
        return View();
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CityViewModel vm)
    {
        if (ModelState.IsValid)
        {
            _context.Add(vm.ToModel());
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewBag.CountryId = new SelectList(_context.Countries, "CountryId", "CountryName", vm.CountryId);
        return View(vm);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();
        var city = await _context.Cities.FindAsync(id);
        if (city == null) return NotFound();

        ViewBag.CountryId = new SelectList(_context.Countries, "CountryId", "CountryName", city.CountryId);
        return View(city.ToViewModel());
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, CityViewModel vm)
    {
        if (id != vm.CityId) return NotFound();
        if (ModelState.IsValid)
        {
            try
            {
                var city = await _context.Cities.FindAsync(id);
                if (city == null) return NotFound();
                city.CityName = vm.CityName;
                city.CountryId = vm.CountryId;
                _context.Update(city);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Cities.Any(e => e.CityId == id)) return NotFound();
                throw;
            }
            return RedirectToAction(nameof(Index));
        }
        ViewBag.CountryId = new SelectList(_context.Countries, "CountryId", "CountryName", vm.CountryId);
        return View(vm);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();
        var city = await _context.Cities.Include(c => c.Country).FirstOrDefaultAsync(m => m.CityId == id);
        if (city == null) return NotFound();
        return View(city.ToViewModel());
    }

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        try
        {
            var city = await _context.Cities.FindAsync(id);
            if (city != null) _context.Cities.Remove(city);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            TempData["Error"] = "Cannot delete this City because it has related Areas.";
        }
        return RedirectToAction(nameof(Index));
    }
}
