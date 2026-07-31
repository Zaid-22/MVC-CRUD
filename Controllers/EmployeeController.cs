using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MvcCrudProject.Extensions;
using MvcCrudProject.Models;
using MvcCrudProject.Repositories;
using MvcCrudProject.ViewModels;

namespace MvcCrudProject.Controllers;

public class EmployeeController : Controller
{
    private readonly IGenericRepository<Employee> _employeeRepo;
    private readonly IGenericRepository<Section> _sectionRepo;

    public EmployeeController(IGenericRepository<Employee> employeeRepo, IGenericRepository<Section> sectionRepo)
    {
        _employeeRepo = employeeRepo;
        _sectionRepo = sectionRepo;
    }

    public async Task<IActionResult> Index() =>
        View((await _employeeRepo.GetAllAsync(e => e.Section)).ToViewModelList());

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();
        var employee = await _employeeRepo.GetFirstOrDefaultAsync(m => m.EmployeeId == id, e => e.Section);
        if (employee == null) return NotFound();
        return View(employee.ToViewModel());
    }

    public async Task<IActionResult> Create()
    {
        ViewBag.SectionId = new SelectList(await _sectionRepo.GetAllAsync(), "SectionId", "SectionName");
        return View();
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(EmployeeViewModel vm)
    {
        if (ModelState.IsValid)
        {
            await _employeeRepo.AddAsync(vm.ToModel());
            await _employeeRepo.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewBag.SectionId = new SelectList(await _sectionRepo.GetAllAsync(), "SectionId", "SectionName", vm.SectionId);
        return View(vm);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();
        var employee = await _employeeRepo.GetByIdAsync(id.Value);
        if (employee == null) return NotFound();

        ViewBag.SectionId = new SelectList(await _sectionRepo.GetAllAsync(), "SectionId", "SectionName", employee.SectionId);
        return View(employee.ToViewModel());
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, EmployeeViewModel vm)
    {
        if (id != vm.EmployeeId) return NotFound();
        if (ModelState.IsValid)
        {
            try
            {
                var employee = await _employeeRepo.GetByIdAsync(id);
                if (employee == null) return NotFound();
                employee.EmployeeName = vm.EmployeeName;
                employee.SectionId = vm.SectionId;
                _employeeRepo.Update(employee);
                await _employeeRepo.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _employeeRepo.ExistsAsync(e => e.EmployeeId == id)) return NotFound();
                throw;
            }
            return RedirectToAction(nameof(Index));
        }
        ViewBag.SectionId = new SelectList(await _sectionRepo.GetAllAsync(), "SectionId", "SectionName", vm.SectionId);
        return View(vm);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();
        var employee = await _employeeRepo.GetFirstOrDefaultAsync(m => m.EmployeeId == id, e => e.Section);
        if (employee == null) return NotFound();
        return View(employee.ToViewModel());
    }

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var employee = await _employeeRepo.GetByIdAsync(id);
        if (employee != null) _employeeRepo.Remove(employee);
        await _employeeRepo.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}
