using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MvcCrudProject.Extensions;
using MvcCrudProject.Models;
using MvcCrudProject.Repositories;
using MvcCrudProject.ViewModels;

namespace MvcCrudProject.Controllers;

public class CityController : Controller
{
    private readonly IGenericRepository<City> _cityRepo;
    private readonly IGenericRepository<Country> _countryRepo;

    public CityController(IGenericRepository<City> cityRepo, IGenericRepository<Country> countryRepo)
    {
        _cityRepo = cityRepo;
        _countryRepo = countryRepo;
    }

    public async Task<IActionResult> Index() =>
        View((await _cityRepo.GetAllAsync(c => c.Country)).ToViewModelList());

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();
        var city = await _cityRepo.GetFirstOrDefaultAsync(m => m.CityId == id, c => c.Country);
        if (city == null) return NotFound();
        return View(city.ToViewModel());
    }

    public async Task<IActionResult> Create()
    {
        ViewBag.CountryId = new SelectList(await _countryRepo.GetAllAsync(), "CountryId", "CountryName");
        return View();
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CityViewModel vm)
    {
        if (ModelState.IsValid)
        {
            await _cityRepo.AddAsync(vm.ToModel());
            await _cityRepo.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewBag.CountryId = new SelectList(await _countryRepo.GetAllAsync(), "CountryId", "CountryName", vm.CountryId);
        return View(vm);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();
        var city = await _cityRepo.GetByIdAsync(id.Value);
        if (city == null) return NotFound();

        ViewBag.CountryId = new SelectList(await _countryRepo.GetAllAsync(), "CountryId", "CountryName", city.CountryId);
        return View(city.ToViewModel());
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, CityViewModel vm)
    {
        if (id != vm.CityId) return NotFound();
        if (ModelState.IsValid)
        {
            try
            {
                var city = await _cityRepo.GetByIdAsync(id);
                if (city == null) return NotFound();
                city.CityName = vm.CityName;
                city.CountryId = vm.CountryId;
                _cityRepo.Update(city);
                await _cityRepo.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _cityRepo.ExistsAsync(e => e.CityId == id)) return NotFound();
                throw;
            }
            return RedirectToAction(nameof(Index));
        }
        ViewBag.CountryId = new SelectList(await _countryRepo.GetAllAsync(), "CountryId", "CountryName", vm.CountryId);
        return View(vm);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();
        var city = await _cityRepo.GetFirstOrDefaultAsync(m => m.CityId == id, c => c.Country);
        if (city == null) return NotFound();
        return View(city.ToViewModel());
    }

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        try
        {
            var city = await _cityRepo.GetByIdAsync(id);
            if (city != null) _cityRepo.Remove(city);
            await _cityRepo.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            TempData["Error"] = "Cannot delete this City because it has related Areas.";
        }
        return RedirectToAction(nameof(Index));
    }
}
