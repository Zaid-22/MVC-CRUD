using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MvcCrudProject.Extensions;
using MvcCrudProject.Models;
using MvcCrudProject.Repositories;
using MvcCrudProject.ViewModels;

namespace MvcCrudProject.Controllers;

public class GalaxyController : Controller
{
    private readonly IGenericRepository<Galaxy> _galaxyRepo;

    public GalaxyController(IGenericRepository<Galaxy> galaxyRepo)
    {
        _galaxyRepo = galaxyRepo;
    }

    public async Task<IActionResult> Index() =>
        View((await _galaxyRepo.GetAllAsync()).ToViewModelList());

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();
        var galaxy = await _galaxyRepo.GetFirstOrDefaultAsync(m => m.GalaxyId == id);
        if (galaxy == null) return NotFound();
        return View(galaxy.ToViewModel());
    }

    public IActionResult Create() => View();

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(GalaxyViewModel vm)
    {
        if (ModelState.IsValid)
        {
            await _galaxyRepo.AddAsync(vm.ToModel());
            await _galaxyRepo.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(vm);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();
        var galaxy = await _galaxyRepo.GetByIdAsync(id.Value);
        if (galaxy == null) return NotFound();
        return View(galaxy.ToViewModel());
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, GalaxyViewModel vm)
    {
        if (id != vm.GalaxyId) return NotFound();
        if (ModelState.IsValid)
        {
            try
            {
                var galaxy = await _galaxyRepo.GetByIdAsync(id);
                if (galaxy == null) return NotFound();
                galaxy.GalaxyName = vm.GalaxyName;
                _galaxyRepo.Update(galaxy);
                await _galaxyRepo.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _galaxyRepo.ExistsAsync(e => e.GalaxyId == id)) return NotFound();
                throw;
            }
            return RedirectToAction(nameof(Index));
        }
        return View(vm);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();
        var galaxy = await _galaxyRepo.GetFirstOrDefaultAsync(m => m.GalaxyId == id);
        if (galaxy == null) return NotFound();
        return View(galaxy.ToViewModel());
    }

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        try
        {
            var galaxy = await _galaxyRepo.GetByIdAsync(id);
            if (galaxy != null) _galaxyRepo.Remove(galaxy);
            await _galaxyRepo.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            TempData["Error"] = "Cannot delete this Galaxy because it has related Planets.";
        }
        return RedirectToAction(nameof(Index));
    }
}
