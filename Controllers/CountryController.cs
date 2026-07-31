using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MvcCrudProject.Models;
using MvcCrudProject.ViewModels;

namespace MvcCrudProject.Controllers;

public class CountryController : Controller
{
    private readonly AppDbContext _context;
    public CountryController(AppDbContext context) => _context = context;

    public async Task<IActionResult> Index() =>
        View(await _context.Countries
            .Select(c => new CountryViewModel
            {
                CountryId = c.CountryId,
                CountryName = c.CountryName,
                RegionId = c.RegionId,
                RegionName = c.Region.RegionName,
                Region = c.Region
            }).ToListAsync());

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();
        var country = await _context.Countries.Include(c => c.Region).FirstOrDefaultAsync(m => m.CountryId == id);
        if (country == null) return NotFound();
        return View(new CountryViewModel
        {
            CountryId = country.CountryId,
            CountryName = country.CountryName,
            RegionId = country.RegionId,
            RegionName = country.Region?.RegionName,
            Region = country.Region
        });
    }

    public IActionResult Create()
    {
        ViewBag.RegionId = new SelectList(_context.Regions, "RegionId", "RegionName");
        return View();
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CountryViewModel vm)
    {
        if (ModelState.IsValid)
        {
            var country = new Country { CountryName = vm.CountryName, RegionId = vm.RegionId };
            _context.Add(country);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewBag.RegionId = new SelectList(_context.Regions, "RegionId", "RegionName", vm.RegionId);
        return View(vm);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();
        var country = await _context.Countries.FindAsync(id);
        if (country == null) return NotFound();

        ViewBag.RegionId = new SelectList(_context.Regions, "RegionId", "RegionName", country.RegionId);
        return View(new CountryViewModel { CountryId = country.CountryId, CountryName = country.CountryName, RegionId = country.RegionId });
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, CountryViewModel vm)
    {
        if (id != vm.CountryId) return NotFound();
        if (ModelState.IsValid)
        {
            try
            {
                var country = await _context.Countries.FindAsync(id);
                if (country == null) return NotFound();
                country.CountryName = vm.CountryName;
                country.RegionId = vm.RegionId;
                _context.Update(country);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Countries.Any(e => e.CountryId == id)) return NotFound();
                throw;
            }
            return RedirectToAction(nameof(Index));
        }
        ViewBag.RegionId = new SelectList(_context.Regions, "RegionId", "RegionName", vm.RegionId);
        return View(vm);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();
        var country = await _context.Countries.Include(c => c.Region).FirstOrDefaultAsync(m => m.CountryId == id);
        if (country == null) return NotFound();
        return View(new CountryViewModel
        {
            CountryId = country.CountryId,
            CountryName = country.CountryName,
            RegionId = country.RegionId,
            RegionName = country.Region?.RegionName,
            Region = country.Region
        });
    }

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        try
        {
            var country = await _context.Countries.FindAsync(id);
            if (country != null) _context.Countries.Remove(country);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            TempData["Error"] = "Cannot delete this Country because it has related Cities.";
        }
        return RedirectToAction(nameof(Index));
    }
}
