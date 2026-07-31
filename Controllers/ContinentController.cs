using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MvcCrudProject.Extensions;
using MvcCrudProject.Models;
using MvcCrudProject.Repositories;
using MvcCrudProject.ViewModels;

namespace MvcCrudProject.Controllers;

public class ContinentController : Controller
{
    private readonly IGenericRepository<Continent> _continentRepo;
    private readonly IGenericRepository<Planet> _planetRepo;

    public ContinentController(IGenericRepository<Continent> continentRepo, IGenericRepository<Planet> planetRepo)
    {
        _continentRepo = continentRepo;
        _planetRepo = planetRepo;
    }

    public async Task<IActionResult> Index() =>
        View((await _continentRepo.GetAllAsync(c => c.Planet)).ToViewModelList());

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();
        var continent = await _continentRepo.GetFirstOrDefaultAsync(m => m.ContinentId == id, c => c.Planet);
        if (continent == null) return NotFound();
        return View(continent.ToViewModel());
    }

    public async Task<IActionResult> Create()
    {
        ViewBag.PlanetId = new SelectList(await _planetRepo.GetAllAsync(), "PlanetId", "PlanetName");
        return View();
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ContinentViewModel vm)
    {
        if (ModelState.IsValid)
        {
            await _continentRepo.AddAsync(vm.ToModel());
            await _continentRepo.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewBag.PlanetId = new SelectList(await _planetRepo.GetAllAsync(), "PlanetId", "PlanetName", vm.PlanetId);
        return View(vm);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();
        var continent = await _continentRepo.GetByIdAsync(id.Value);
        if (continent == null) return NotFound();

        ViewBag.PlanetId = new SelectList(await _planetRepo.GetAllAsync(), "PlanetId", "PlanetName", continent.PlanetId);
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
                var continent = await _continentRepo.GetByIdAsync(id);
                if (continent == null) return NotFound();
                continent.ContinentName = vm.ContinentName;
                continent.PlanetId = vm.PlanetId;
                _continentRepo.Update(continent);
                await _continentRepo.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _continentRepo.ExistsAsync(e => e.ContinentId == id)) return NotFound();
                throw;
            }
            return RedirectToAction(nameof(Index));
        }
        ViewBag.PlanetId = new SelectList(await _planetRepo.GetAllAsync(), "PlanetId", "PlanetName", vm.PlanetId);
        return View(vm);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();
        var continent = await _continentRepo.GetFirstOrDefaultAsync(m => m.ContinentId == id, c => c.Planet);
        if (continent == null) return NotFound();
        return View(continent.ToViewModel());
    }

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        try
        {
            var continent = await _continentRepo.GetByIdAsync(id);
            if (continent != null) _continentRepo.Remove(continent);
            await _continentRepo.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            TempData["Error"] = "Cannot delete this Continent because it has related Regions.";
        }
        return RedirectToAction(nameof(Index));
    }
}
