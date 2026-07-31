using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MvcCrudProject.Extensions;
using MvcCrudProject.Models;
using MvcCrudProject.Repositories;
using MvcCrudProject.ViewModels;

namespace MvcCrudProject.Controllers;

public class BranchController : Controller
{
    private readonly IGenericRepository<Branch> _branchRepo;
    private readonly IGenericRepository<Area> _areaRepo;
    private readonly IGenericRepository<Company> _companyRepo;

    public BranchController(
        IGenericRepository<Branch> branchRepo,
        IGenericRepository<Area> areaRepo,
        IGenericRepository<Company> companyRepo)
    {
        _branchRepo = branchRepo;
        _areaRepo = areaRepo;
        _companyRepo = companyRepo;
    }

    public async Task<IActionResult> Index() =>
        View((await _branchRepo.GetAllAsync(b => b.Area, b => b.Company)).ToViewModelList());

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();
        var branch = await _branchRepo.GetFirstOrDefaultAsync(m => m.BranchId == id, b => b.Area, b => b.Company);
        if (branch == null) return NotFound();
        return View(branch.ToViewModel());
    }

    public async Task<IActionResult> Create(int? companyId)
    {
        ViewBag.AreaId = new SelectList(await _areaRepo.GetAllAsync(), "AreaId", "AreaName");
        ViewBag.CompanyId = new SelectList(await _companyRepo.GetAllAsync(), "CompanyId", "CompanyName", companyId);
        return View(new BranchViewModel { CompanyId = companyId ?? 0 });
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(BranchViewModel vm)
    {
        if (ModelState.IsValid)
        {
            await _branchRepo.AddAsync(vm.ToModel());
            await _branchRepo.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewBag.AreaId = new SelectList(await _areaRepo.GetAllAsync(), "AreaId", "AreaName", vm.AreaId);
        ViewBag.CompanyId = new SelectList(await _companyRepo.GetAllAsync(), "CompanyId", "CompanyName", vm.CompanyId);
        return View(vm);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();
        var branch = await _branchRepo.GetByIdAsync(id.Value);
        if (branch == null) return NotFound();

        ViewBag.AreaId = new SelectList(await _areaRepo.GetAllAsync(), "AreaId", "AreaName", branch.AreaId);
        ViewBag.CompanyId = new SelectList(await _companyRepo.GetAllAsync(), "CompanyId", "CompanyName", branch.CompanyId);
        return View(branch.ToViewModel());
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, BranchViewModel vm)
    {
        if (id != vm.BranchId) return NotFound();
        if (ModelState.IsValid)
        {
            try
            {
                var branch = await _branchRepo.GetByIdAsync(id);
                if (branch == null) return NotFound();
                branch.BranchName = vm.BranchName;
                branch.AreaId = vm.AreaId;
                branch.CompanyId = vm.CompanyId;
                _branchRepo.Update(branch);
                await _branchRepo.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _branchRepo.ExistsAsync(e => e.BranchId == id)) return NotFound();
                throw;
            }
            return RedirectToAction(nameof(Index));
        }
        ViewBag.AreaId = new SelectList(await _areaRepo.GetAllAsync(), "AreaId", "AreaName", vm.AreaId);
        ViewBag.CompanyId = new SelectList(await _companyRepo.GetAllAsync(), "CompanyId", "CompanyName", vm.CompanyId);
        return View(vm);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();
        var branch = await _branchRepo.GetFirstOrDefaultAsync(m => m.BranchId == id, b => b.Area, b => b.Company);
        if (branch == null) return NotFound();
        return View(branch.ToViewModel());
    }

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        try
        {
            var branch = await _branchRepo.GetByIdAsync(id);
            if (branch != null) _branchRepo.Remove(branch);
            await _branchRepo.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            TempData["Error"] = "Cannot delete this Branch because it has related Departments.";
        }
        return RedirectToAction(nameof(Index));
    }
}
