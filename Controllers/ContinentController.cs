using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MvcCrudProject.Data;
using MvcCrudProject.Extensions;
using MvcCrudProject.Models;
using MvcCrudProject.ViewModels;

namespace MvcCrudProject.Controllers;

public class ContinentController : Controller
{
    private readonly AppDbContext _context;
    public ContinentController(AppDbContext context) => _context = context;

    public async Task<IActionResult> Index() =>
        View((await _context.Continents.Include(c => c.Planet).ToListAsync()).ToViewModelList());

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();
        var continent = await _context.Continents.Include(c => c.Planet).FirstOrDefaultAsync(m => m.ContinentId == id);
        if (continent == null) return NotFound();
        return View(continent.ToViewModel());
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
            _context.Add(vm.ToModel());
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
        return View(continent.ToViewModel());
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
        return View(continent.ToViewModel());
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
