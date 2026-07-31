using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MvcCrudProject.Data;
using MvcCrudProject.Extensions;
using MvcCrudProject.Models;
using MvcCrudProject.ViewModels;

namespace MvcCrudProject.Controllers;

public class RegionController : Controller
{
    private readonly AppDbContext _context;
    public RegionController(AppDbContext context) => _context = context;

    public async Task<IActionResult> Index() =>
        View((await _context.Regions.Include(r => r.Continent).ToListAsync()).ToViewModelList());

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();
        var region = await _context.Regions.Include(r => r.Continent).FirstOrDefaultAsync(m => m.RegionId == id);
        if (region == null) return NotFound();
        return View(region.ToViewModel());
    }

    public IActionResult Create()
    {
        ViewBag.ContinentId = new SelectList(_context.Continents, "ContinentId", "ContinentName");
        return View();
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(RegionViewModel vm)
    {
        if (ModelState.IsValid)
        {
            _context.Add(vm.ToModel());
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewBag.ContinentId = new SelectList(_context.Continents, "ContinentId", "ContinentName", vm.ContinentId);
        return View(vm);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();
        var region = await _context.Regions.FindAsync(id);
        if (region == null) return NotFound();

        ViewBag.ContinentId = new SelectList(_context.Continents, "ContinentId", "ContinentName", region.ContinentId);
        return View(region.ToViewModel());
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, RegionViewModel vm)
    {
        if (id != vm.RegionId) return NotFound();
        if (ModelState.IsValid)
        {
            try
            {
                var region = await _context.Regions.FindAsync(id);
                if (region == null) return NotFound();
                region.RegionName = vm.RegionName;
                region.ContinentId = vm.ContinentId;
                _context.Update(region);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Regions.Any(e => e.RegionId == id)) return NotFound();
                throw;
            }
            return RedirectToAction(nameof(Index));
        }
        ViewBag.ContinentId = new SelectList(_context.Continents, "ContinentId", "ContinentName", vm.ContinentId);
        return View(vm);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();
        var region = await _context.Regions.Include(r => r.Continent).FirstOrDefaultAsync(m => m.RegionId == id);
        if (region == null) return NotFound();
        return View(region.ToViewModel());
    }

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        try
        {
            var region = await _context.Regions.FindAsync(id);
            if (region != null) _context.Regions.Remove(region);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            TempData["Error"] = "Cannot delete this Region because it has related Countries.";
        }
        return RedirectToAction(nameof(Index));
    }
}
