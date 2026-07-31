using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MvcCrudProject.Models;
using MvcCrudProject.ViewModels;

namespace MvcCrudProject.Controllers;

public class ContinentController : Controller
{
    private readonly AppDbContext _context;
    public ContinentController(AppDbContext context) => _context = context;

    public async Task<IActionResult> Index() =>
        View(await _context.Continents.Include(c => c.Planet).Select(c => new ContinentViewModel
        {
            ContinentId = c.ContinentId,
            ContinentName = c.ContinentName,
            PlanetId = c.PlanetId,
            PlanetName = c.Planet.PlanetName,
            Planet = c.Planet
        }).ToListAsync());

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();
        var continent = await _context.Continents.Include(c => c.Planet).FirstOrDefaultAsync(m => m.ContinentId == id);
        if (continent == null) return NotFound();
        return View(new ContinentViewModel
        {
            ContinentId = continent.ContinentId,
            ContinentName = continent.ContinentName,
            PlanetId = continent.PlanetId,
            PlanetName = continent.Planet?.PlanetName,
            Planet = continent.Planet
        });
    }

    public IActionResult Create()
    {
        ViewBag.PlanetId = new SelectList(_context.Planets, "PlanetId", "PlanetName");
        return View();
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ContinentViewModel vm)
    {
        if (ModelState.IsValid)
        {
            var continent = new Continent { ContinentName = vm.ContinentName, PlanetId = vm.PlanetId };
            _context.Add(continent);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewBag.PlanetId = new SelectList(_context.Planets, "PlanetId", "PlanetName", vm.PlanetId);
        return View(vm);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();
        var continent = await _context.Continents.FindAsync(id);
        if (continent == null) return NotFound();

        ViewBag.PlanetId = new SelectList(_context.Planets, "PlanetId", "PlanetName", continent.PlanetId);
        return View(new ContinentViewModel { ContinentId = continent.ContinentId, ContinentName = continent.ContinentName, PlanetId = continent.PlanetId });
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, ContinentViewModel vm)
    {
        if (id != vm.ContinentId) return NotFound();
        if (ModelState.IsValid)
        {
            try
            {
                var continent = await _context.Continents.FindAsync(id);
                if (continent == null) return NotFound();
                continent.ContinentName = vm.ContinentName;
                continent.PlanetId = vm.PlanetId;
                _context.Update(continent);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Continents.Any(e => e.ContinentId == id)) return NotFound();
                throw;
            }
            return RedirectToAction(nameof(Index));
        }
        ViewBag.PlanetId = new SelectList(_context.Planets, "PlanetId", "PlanetName", vm.PlanetId);
        return View(vm);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();
        var continent = await _context.Continents.Include(c => c.Planet).FirstOrDefaultAsync(m => m.ContinentId == id);
        if (continent == null) return NotFound();
        return View(new ContinentViewModel
        {
            ContinentId = continent.ContinentId,
            ContinentName = continent.ContinentName,
            PlanetId = continent.PlanetId,
            PlanetName = continent.Planet?.PlanetName,
            Planet = continent.Planet
        });
    }

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        try
        {
            var continent = await _context.Continents.FindAsync(id);
            if (continent != null) _context.Continents.Remove(continent);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            TempData["Error"] = "Cannot delete this Continent because it has related Regions.";
        }
        return RedirectToAction(nameof(Index));
    }
}
