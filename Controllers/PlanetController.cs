using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MvcCrudProject.Extensions;
using MvcCrudProject.Models;
using MvcCrudProject.Repositories;
using MvcCrudProject.ViewModels;

namespace MvcCrudProject.Controllers;

public class PlanetController : Controller
{
    private readonly IGenericRepository<Planet> _planetRepo;
    private readonly IGenericRepository<Galaxy> _galaxyRepo;

    public PlanetController(IGenericRepository<Planet> planetRepo, IGenericRepository<Galaxy> galaxyRepo)
    {
        _planetRepo = planetRepo;
        _galaxyRepo = galaxyRepo;
    }

    public async Task<IActionResult> Index() =>
        View((await _planetRepo.GetAllAsync(p => p.Galaxy)).ToViewModelList());

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();
        var planet = await _planetRepo.GetFirstOrDefaultAsync(m => m.PlanetId == id, p => p.Galaxy);
        if (planet == null) return NotFound();
        return View(planet.ToViewModel());
    }

    public async Task<IActionResult> Create()
    {
        ViewBag.GalaxyId = new SelectList(await _galaxyRepo.GetAllAsync(), "GalaxyId", "GalaxyName");
        return View();
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(PlanetViewModel vm)
    {
        if (ModelState.IsValid)
        {
            await _planetRepo.AddAsync(vm.ToModel());
            await _planetRepo.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewBag.GalaxyId = new SelectList(await _galaxyRepo.GetAllAsync(), "GalaxyId", "GalaxyName", vm.GalaxyId);
        return View(vm);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();
        var planet = await _planetRepo.GetByIdAsync(id.Value);
        if (planet == null) return NotFound();

        ViewBag.GalaxyId = new SelectList(await _galaxyRepo.GetAllAsync(), "GalaxyId", "GalaxyName", planet.GalaxyId);
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
                var planet = await _planetRepo.GetByIdAsync(id);
                if (planet == null) return NotFound();
                planet.PlanetName = vm.PlanetName;
                planet.GalaxyId = vm.GalaxyId;
                _planetRepo.Update(planet);
                await _planetRepo.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _planetRepo.ExistsAsync(e => e.PlanetId == id)) return NotFound();
                throw;
            }
            return RedirectToAction(nameof(Index));
        }
        ViewBag.GalaxyId = new SelectList(await _galaxyRepo.GetAllAsync(), "GalaxyId", "GalaxyName", vm.GalaxyId);
        return View(vm);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();
        var planet = await _planetRepo.GetFirstOrDefaultAsync(m => m.PlanetId == id, p => p.Galaxy);
        if (planet == null) return NotFound();
        return View(planet.ToViewModel());
    }

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        try
        {
            var planet = await _planetRepo.GetByIdAsync(id);
            if (planet != null) _planetRepo.Remove(planet);
            await _planetRepo.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            TempData["Error"] = "Cannot delete this Planet because it has related Continents.";
        }
        return RedirectToAction(nameof(Index));
    }
}
