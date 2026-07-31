using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MvcCrudProject.Mappings;
using MvcCrudProject.Models;
using MvcCrudProject.ViewModels;

namespace MvcCrudProject.Controllers;

public class AreaController : Controller
{
    private readonly AppDbContext _context;
    public AreaController(AppDbContext context) => _context = context;

    public async Task<IActionResult> Index() =>
        View(await _context.Areas
            .Select(a => new AreaViewModel
            {
                AreaId = a.AreaId,
                AreaName = a.AreaName,
                CityId = a.CityId,
                CityName = a.City.CityName,
                City = a.City
            }).ToListAsync());

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();
        var area = await _context.Areas.Include(a => a.City).FirstOrDefaultAsync(m => m.AreaId == id);
        if (area == null) return NotFound();
        return View(area.ToViewModel());
    }

    public IActionResult Create()
    {
        ViewBag.CityId = new SelectList(_context.Cities, "CityId", "CityName");
        return View();
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(AreaViewModel vm)
    {
        if (ModelState.IsValid)
        {
            var area = vm.ToEntity();
            _context.Add(area);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewBag.CityId = new SelectList(_context.Cities, "CityId", "CityName", vm.CityId);
        return View(vm);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();
        var area = await _context.Areas.FindAsync(id);
        if (area == null) return NotFound();

        ViewBag.CityId = new SelectList(_context.Cities, "CityId", "CityName", area.CityId);
        return View(area.ToViewModel());
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, AreaViewModel vm)
    {
        if (id != vm.AreaId) return NotFound();
        if (ModelState.IsValid)
        {
            try
            {
                var area = await _context.Areas.FindAsync(id);
                if (area == null) return NotFound();
                vm.UpdateEntity(area);
                _context.Update(area);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Areas.Any(e => e.AreaId == id)) return NotFound();
                throw;
            }
            return RedirectToAction(nameof(Index));
        }
        ViewBag.CityId = new SelectList(_context.Cities, "CityId", "CityName", vm.CityId);
        return View(vm);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();
        var area = await _context.Areas.Include(a => a.City).FirstOrDefaultAsync(m => m.AreaId == id);
        if (area == null) return NotFound();
        return View(area.ToViewModel());
    }

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        try
        {
            var area = await _context.Areas.FindAsync(id);
            if (area != null) _context.Areas.Remove(area);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            TempData["Error"] = "Cannot delete this Area because it has related Branches.";
        }
        return RedirectToAction(nameof(Index));
    }
}
