using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MvcCrudProject.Extensions;
using MvcCrudProject.Models;
using MvcCrudProject.Repositories;
using MvcCrudProject.ViewModels;

namespace MvcCrudProject.Controllers;

public class RegionController : Controller
{
    private readonly IGenericRepository<Region> _regionRepo;
    private readonly IGenericRepository<Continent> _continentRepo;

    public RegionController(IGenericRepository<Region> regionRepo, IGenericRepository<Continent> continentRepo)
    {
        _regionRepo = regionRepo;
        _continentRepo = continentRepo;
    }

    public async Task<IActionResult> Index() =>
        View((await _regionRepo.GetAllAsync(r => r.Continent)).ToViewModelList());

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();
        var region = await _regionRepo.GetFirstOrDefaultAsync(m => m.RegionId == id, r => r.Continent);
        if (region == null) return NotFound();
        return View(region.ToViewModel());
    }

    public async Task<IActionResult> Create()
    {
        ViewBag.ContinentId = new SelectList(await _continentRepo.GetAllAsync(), "ContinentId", "ContinentName");
        return View();
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(RegionViewModel vm)
    {
        if (ModelState.IsValid)
        {
            await _regionRepo.AddAsync(vm.ToModel());
            await _regionRepo.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewBag.ContinentId = new SelectList(await _continentRepo.GetAllAsync(), "ContinentId", "ContinentName", vm.ContinentId);
        return View(vm);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();
        var region = await _regionRepo.GetByIdAsync(id.Value);
        if (region == null) return NotFound();

        ViewBag.ContinentId = new SelectList(await _continentRepo.GetAllAsync(), "ContinentId", "ContinentName", region.ContinentId);
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
                var region = await _regionRepo.GetByIdAsync(id);
                if (region == null) return NotFound();
                region.RegionName = vm.RegionName;
                region.ContinentId = vm.ContinentId;
                _regionRepo.Update(region);
                await _regionRepo.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _regionRepo.ExistsAsync(e => e.RegionId == id)) return NotFound();
                throw;
            }
            return RedirectToAction(nameof(Index));
        }
        ViewBag.ContinentId = new SelectList(await _continentRepo.GetAllAsync(), "ContinentId", "ContinentName", vm.ContinentId);
        return View(vm);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();
        var region = await _regionRepo.GetFirstOrDefaultAsync(m => m.RegionId == id, r => r.Continent);
        if (region == null) return NotFound();
        return View(region.ToViewModel());
    }

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        try
        {
            var region = await _regionRepo.GetByIdAsync(id);
            if (region != null) _regionRepo.Remove(region);
            await _regionRepo.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            TempData["Error"] = "Cannot delete this Region because it has related Countries.";
        }
        return RedirectToAction(nameof(Index));
    }
}
