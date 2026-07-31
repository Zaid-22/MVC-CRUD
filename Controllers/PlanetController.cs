using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MvcCrudProject.Data;
using MvcCrudProject.Extensions;
using MvcCrudProject.Models;
using MvcCrudProject.ViewModels;

namespace MvcCrudProject.Controllers;

public class PlanetController : Controller
{
    private readonly AppDbContext _context;
    public PlanetController(AppDbContext context) => _context = context;

    public async Task<IActionResult> Index() =>
        View((await _context.Planets.Include(p => p.Galaxy).ToListAsync()).ToViewModelList());

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();
        var planet = await _context.Planets.Include(p => p.Galaxy).FirstOrDefaultAsync(m => m.PlanetId == id);
        if (planet == null) return NotFound();
        return View(planet.ToViewModel());
    }

    public IActionResult Create()
    {
        ViewBag.GalaxyId = new SelectList(_context.Galaxies, "GalaxyId", "GalaxyName");
        return View();
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(PlanetViewModel vm)
    {
        if (ModelState.IsValid)
        {
            _context.Add(vm.ToModel());
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewBag.GalaxyId = new SelectList(_context.Galaxies, "GalaxyId", "GalaxyName", vm.GalaxyId);
        return View(vm);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();
        var planet = await _context.Planets.FindAsync(id);
        if (planet == null) return NotFound();

        ViewBag.GalaxyId = new SelectList(_context.Galaxies, "GalaxyId", "GalaxyName", planet.GalaxyId);
        return View(planet.ToViewModel());
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, PlanetViewModel vm)
    {
        if (id != vm.PlanetId) return NotFound();
        if (ModelState.IsValid)
        {
            try
            {
                var planet = await _context.Planets.FindAsync(id);
                if (planet == null) return NotFound();
                planet.PlanetName = vm.PlanetName;
                planet.GalaxyId = vm.GalaxyId;
                _context.Update(planet);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Planets.Any(e => e.PlanetId == id)) return NotFound();
                throw;
            }
            return RedirectToAction(nameof(Index));
        }
        ViewBag.GalaxyId = new SelectList(_context.Galaxies, "GalaxyId", "GalaxyName", vm.GalaxyId);
        return View(vm);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();
        var planet = await _context.Planets.Include(p => p.Galaxy).FirstOrDefaultAsync(m => m.PlanetId == id);
        if (planet == null) return NotFound();
        return View(planet.ToViewModel());
    }

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        try
        {
            var planet = await _context.Planets.FindAsync(id);
            if (planet != null) _context.Planets.Remove(planet);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            TempData["Error"] = "Cannot delete this Planet because it has related Continents.";
        }
        return RedirectToAction(nameof(Index));
    }
}
