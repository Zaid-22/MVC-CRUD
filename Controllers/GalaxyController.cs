using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MvcCrudProject.Models;
using MvcCrudProject.ViewModels;

namespace MvcCrudProject.Controllers;

public class GalaxyController : Controller
{
    private readonly AppDbContext _context;
    public GalaxyController(AppDbContext context) => _context = context;

    public async Task<IActionResult> Index() =>
        View(await _context.Galaxies
            .Select(g => new GalaxyViewModel { GalaxyId = g.GalaxyId, GalaxyName = g.GalaxyName })
            .ToListAsync());

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();
        var galaxy = await _context.Galaxies.FirstOrDefaultAsync(m => m.GalaxyId == id);
        if (galaxy == null) return NotFound();
        return View(new GalaxyViewModel { GalaxyId = galaxy.GalaxyId, GalaxyName = galaxy.GalaxyName });
    }

    public IActionResult Create() => View();

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(GalaxyViewModel vm)
    {
        if (ModelState.IsValid)
        {
            var galaxy = new Galaxy { GalaxyName = vm.GalaxyName };
            _context.Add(galaxy);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(vm);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();
        var galaxy = await _context.Galaxies.FindAsync(id);
        if (galaxy == null) return NotFound();
        return View(new GalaxyViewModel { GalaxyId = galaxy.GalaxyId, GalaxyName = galaxy.GalaxyName });
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, GalaxyViewModel vm)
    {
        if (id != vm.GalaxyId) return NotFound();
        if (ModelState.IsValid)
        {
            try
            {
                var galaxy = await _context.Galaxies.FindAsync(id);
                if (galaxy == null) return NotFound();
                galaxy.GalaxyName = vm.GalaxyName;
                _context.Update(galaxy);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Galaxies.Any(e => e.GalaxyId == id)) return NotFound();
                throw;
            }
            return RedirectToAction(nameof(Index));
        }
        return View(vm);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();
        var galaxy = await _context.Galaxies.FirstOrDefaultAsync(m => m.GalaxyId == id);
        if (galaxy == null) return NotFound();
        return View(new GalaxyViewModel { GalaxyId = galaxy.GalaxyId, GalaxyName = galaxy.GalaxyName });
    }

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        try
        {
            var galaxy = await _context.Galaxies.FindAsync(id);
            if (galaxy != null) _context.Galaxies.Remove(galaxy);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            TempData["Error"] = "Cannot delete this Galaxy because it has related Planets.";
        }
        return RedirectToAction(nameof(Index));
    }
}
