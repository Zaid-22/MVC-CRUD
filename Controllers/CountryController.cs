using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MvcCrudProject.Extensions;
using MvcCrudProject.Models;
using MvcCrudProject.Repositories;
using MvcCrudProject.ViewModels;

namespace MvcCrudProject.Controllers;

public class CountryController : Controller
{
    private readonly IGenericRepository<Country> _countryRepo;
    private readonly IGenericRepository<Region> _regionRepo;

    public CountryController(IGenericRepository<Country> countryRepo, IGenericRepository<Region> regionRepo)
    {
        _countryRepo = countryRepo;
        _regionRepo = regionRepo;
    }

    public async Task<IActionResult> Index() =>
        View((await _countryRepo.GetAllAsync(c => c.Region)).ToViewModelList());

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();
        var country = await _countryRepo.GetFirstOrDefaultAsync(m => m.CountryId == id, c => c.Region);
        if (country == null) return NotFound();
        return View(country.ToViewModel());
    }

    public async Task<IActionResult> Create()
    {
        ViewBag.RegionId = new SelectList(await _regionRepo.GetAllAsync(), "RegionId", "RegionName");
        return View();
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CountryViewModel vm)
    {
        if (ModelState.IsValid)
        {
            await _countryRepo.AddAsync(vm.ToModel());
            await _countryRepo.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewBag.RegionId = new SelectList(await _regionRepo.GetAllAsync(), "RegionId", "RegionName", vm.RegionId);
        return View(vm);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();
        var country = await _countryRepo.GetByIdAsync(id.Value);
        if (country == null) return NotFound();

        ViewBag.RegionId = new SelectList(await _regionRepo.GetAllAsync(), "RegionId", "RegionName", country.RegionId);
        return View(country.ToViewModel());
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, CountryViewModel vm)
    {
        if (id != vm.CountryId) return NotFound();
        if (ModelState.IsValid)
        {
            try
            {
                var country = await _countryRepo.GetByIdAsync(id);
                if (country == null) return NotFound();
                country.CountryName = vm.CountryName;
                country.RegionId = vm.RegionId;
                _countryRepo.Update(country);
                await _countryRepo.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _countryRepo.ExistsAsync(e => e.CountryId == id)) return NotFound();
                throw;
            }
            return RedirectToAction(nameof(Index));
        }
        ViewBag.RegionId = new SelectList(await _regionRepo.GetAllAsync(), "RegionId", "RegionName", vm.RegionId);
        return View(vm);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();
        var country = await _countryRepo.GetFirstOrDefaultAsync(m => m.CountryId == id, c => c.Region);
        if (country == null) return NotFound();
        return View(country.ToViewModel());
    }

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        try
        {
            var country = await _countryRepo.GetByIdAsync(id);
            if (country != null) _countryRepo.Remove(country);
            await _countryRepo.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            TempData["Error"] = "Cannot delete this Country because it has related Cities.";
        }
        return RedirectToAction(nameof(Index));
    }
}
