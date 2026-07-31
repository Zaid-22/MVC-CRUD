using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MvcCrudProject.Extensions;
using MvcCrudProject.Models;
using MvcCrudProject.Repositories;
using MvcCrudProject.ViewModels;

namespace MvcCrudProject.Controllers;

public class AreaController : Controller
{
    private readonly IGenericRepository<Area> _areaRepo;
    private readonly IGenericRepository<City> _cityRepo;

    public AreaController(IGenericRepository<Area> areaRepo, IGenericRepository<City> cityRepo)
    {
        _areaRepo = areaRepo;
        _cityRepo = cityRepo;
    }

    public async Task<IActionResult> Index() =>
        View((await _areaRepo.GetAllAsync(a => a.City)).ToViewModelList());

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();
        var area = await _areaRepo.GetFirstOrDefaultAsync(m => m.AreaId == id, a => a.City);
        if (area == null) return NotFound();
        return View(area.ToViewModel());
    }

    public async Task<IActionResult> Create()
    {
        ViewBag.CityId = new SelectList(await _cityRepo.GetAllAsync(), "CityId", "CityName");
        return View();
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(AreaViewModel vm)
    {
        if (ModelState.IsValid)
        {
            await _areaRepo.AddAsync(vm.ToModel());
            await _areaRepo.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewBag.CityId = new SelectList(await _cityRepo.GetAllAsync(), "CityId", "CityName", vm.CityId);
        return View(vm);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();
        var area = await _areaRepo.GetByIdAsync(id.Value);
        if (area == null) return NotFound();

        ViewBag.CityId = new SelectList(await _cityRepo.GetAllAsync(), "CityId", "CityName", area.CityId);
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
                var area = await _areaRepo.GetByIdAsync(id);
                if (area == null) return NotFound();
                area.AreaName = vm.AreaName;
                area.CityId = vm.CityId;
                _areaRepo.Update(area);
                await _areaRepo.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _areaRepo.ExistsAsync(e => e.AreaId == id)) return NotFound();
                throw;
            }
            return RedirectToAction(nameof(Index));
        }
        ViewBag.CityId = new SelectList(await _cityRepo.GetAllAsync(), "CityId", "CityName", vm.CityId);
        return View(vm);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();
        var area = await _areaRepo.GetFirstOrDefaultAsync(m => m.AreaId == id, a => a.City);
        if (area == null) return NotFound();
        return View(area.ToViewModel());
    }

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        try
        {
            var area = await _areaRepo.GetByIdAsync(id);
            if (area != null) _areaRepo.Remove(area);
            await _areaRepo.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            TempData["Error"] = "Cannot delete this Area because it has related Branches.";
        }
        return RedirectToAction(nameof(Index));
    }
}
