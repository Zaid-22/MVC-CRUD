using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MvcCrudProject.Extensions;
using MvcCrudProject.Models;
using MvcCrudProject.Repositories;
using MvcCrudProject.ViewModels;

namespace MvcCrudProject.Controllers;

public class DepartmentController : Controller
{
    private readonly IGenericRepository<Department> _departmentRepo;
    private readonly IGenericRepository<Branch> _branchRepo;

    public DepartmentController(IGenericRepository<Department> departmentRepo, IGenericRepository<Branch> branchRepo)
    {
        _departmentRepo = departmentRepo;
        _branchRepo = branchRepo;
    }

    public async Task<IActionResult> Index() =>
        View((await _departmentRepo.GetAllAsync(d => d.Branch)).ToViewModelList());

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();
        var department = await _departmentRepo.GetFirstOrDefaultAsync(m => m.DepartmentId == id, d => d.Branch);
        if (department == null) return NotFound();
        return View(department.ToViewModel());
    }

    public async Task<IActionResult> Create()
    {
        ViewBag.BranchId = new SelectList(await _branchRepo.GetAllAsync(), "BranchId", "BranchName");
        return View();
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(DepartmentViewModel vm)
    {
        if (ModelState.IsValid)
        {
            await _departmentRepo.AddAsync(vm.ToModel());
            await _departmentRepo.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewBag.BranchId = new SelectList(await _branchRepo.GetAllAsync(), "BranchId", "BranchName", vm.BranchId);
        return View(vm);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();
        var department = await _departmentRepo.GetByIdAsync(id.Value);
        if (department == null) return NotFound();

        ViewBag.BranchId = new SelectList(await _branchRepo.GetAllAsync(), "BranchId", "BranchName", department.BranchId);
        return View(department.ToViewModel());
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, DepartmentViewModel vm)
    {
        if (id != vm.DepartmentId) return NotFound();
        if (ModelState.IsValid)
        {
            try
            {
                var department = await _departmentRepo.GetByIdAsync(id);
                if (department == null) return NotFound();
                department.DepartmentName = vm.DepartmentName;
                department.BranchId = vm.BranchId;
                _departmentRepo.Update(department);
                await _departmentRepo.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _departmentRepo.ExistsAsync(e => e.DepartmentId == id)) return NotFound();
                throw;
            }
            return RedirectToAction(nameof(Index));
        }
        ViewBag.BranchId = new SelectList(await _branchRepo.GetAllAsync(), "BranchId", "BranchName", vm.BranchId);
        return View(vm);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();
        var department = await _departmentRepo.GetFirstOrDefaultAsync(m => m.DepartmentId == id, d => d.Branch);
        if (department == null) return NotFound();
        return View(department.ToViewModel());
    }

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        try
        {
            var department = await _departmentRepo.GetByIdAsync(id);
            if (department != null) _departmentRepo.Remove(department);
            await _departmentRepo.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            TempData["Error"] = "Cannot delete this Department because it has related Sections.";
        }
        return RedirectToAction(nameof(Index));
    }
}
