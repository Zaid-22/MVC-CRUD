using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MvcCrudProject.Extensions;
using MvcCrudProject.Models;
using MvcCrudProject.Repositories;
using MvcCrudProject.ViewModels;

namespace MvcCrudProject.Controllers;

public class SectionController : Controller
{
    private readonly IGenericRepository<Section> _sectionRepo;
    private readonly IGenericRepository<Department> _departmentRepo;

    public SectionController(IGenericRepository<Section> sectionRepo, IGenericRepository<Department> departmentRepo)
    {
        _sectionRepo = sectionRepo;
        _departmentRepo = departmentRepo;
    }

    public async Task<IActionResult> Index() =>
        View((await _sectionRepo.GetAllAsync(s => s.Department)).ToViewModelList());

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();
        var section = await _sectionRepo.GetFirstOrDefaultAsync(m => m.SectionId == id, s => s.Department);
        if (section == null) return NotFound();
        return View(section.ToViewModel());
    }

    public async Task<IActionResult> Create()
    {
        ViewBag.DepartmentId = new SelectList(await _departmentRepo.GetAllAsync(), "DepartmentId", "DepartmentName");
        return View();
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(SectionViewModel vm)
    {
        if (ModelState.IsValid)
        {
            await _sectionRepo.AddAsync(vm.ToModel());
            await _sectionRepo.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewBag.DepartmentId = new SelectList(await _departmentRepo.GetAllAsync(), "DepartmentId", "DepartmentName", vm.DepartmentId);
        return View(vm);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();
        var section = await _sectionRepo.GetByIdAsync(id.Value);
        if (section == null) return NotFound();

        ViewBag.DepartmentId = new SelectList(await _departmentRepo.GetAllAsync(), "DepartmentId", "DepartmentName", section.DepartmentId);
        return View(section.ToViewModel());
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, SectionViewModel vm)
    {
        if (id != vm.SectionId) return NotFound();
        if (ModelState.IsValid)
        {
            try
            {
                var section = await _sectionRepo.GetByIdAsync(id);
                if (section == null) return NotFound();
                section.SectionName = vm.SectionName;
                section.DepartmentId = vm.DepartmentId;
                _sectionRepo.Update(section);
                await _sectionRepo.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _sectionRepo.ExistsAsync(e => e.SectionId == id)) return NotFound();
                throw;
            }
            return RedirectToAction(nameof(Index));
        }
        ViewBag.DepartmentId = new SelectList(await _departmentRepo.GetAllAsync(), "DepartmentId", "DepartmentName", vm.DepartmentId);
        return View(vm);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();
        var section = await _sectionRepo.GetFirstOrDefaultAsync(m => m.SectionId == id, s => s.Department);
        if (section == null) return NotFound();
        return View(section.ToViewModel());
    }

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        try
        {
            var section = await _sectionRepo.GetByIdAsync(id);
            if (section != null) _sectionRepo.Remove(section);
            await _sectionRepo.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            TempData["Error"] = "Cannot delete this Section because it has related Employees.";
        }
        return RedirectToAction(nameof(Index));
    }
}
