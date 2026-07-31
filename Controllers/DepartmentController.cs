using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MvcCrudProject.Data;
using MvcCrudProject.Extensions;
using MvcCrudProject.Models;
using MvcCrudProject.ViewModels;

namespace MvcCrudProject.Controllers;

public class DepartmentController : Controller
{
    private readonly AppDbContext _context;
    public DepartmentController(AppDbContext context) => _context = context;

    public async Task<IActionResult> Index() =>
        View((await _context.Departments.Include(d => d.Branch).ToListAsync()).ToViewModelList());

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();
        var department = await _context.Departments.Include(d => d.Branch).FirstOrDefaultAsync(m => m.DepartmentId == id);
        if (department == null) return NotFound();
        return View(department.ToViewModel());
    }

    public IActionResult Create()
    {
        ViewBag.BranchId = new SelectList(_context.Branches, "BranchId", "BranchName");
        return View();
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(DepartmentViewModel vm)
    {
        if (ModelState.IsValid)
        {
            _context.Add(vm.ToModel());
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewBag.BranchId = new SelectList(_context.Branches, "BranchId", "BranchName", vm.BranchId);
        return View(vm);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();
        var department = await _context.Departments.FindAsync(id);
        if (department == null) return NotFound();

        ViewBag.BranchId = new SelectList(_context.Branches, "BranchId", "BranchName", department.BranchId);
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
                var department = await _context.Departments.FindAsync(id);
                if (department == null) return NotFound();
                department.DepartmentName = vm.DepartmentName;
                department.BranchId = vm.BranchId;
                _context.Update(department);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Departments.Any(e => e.DepartmentId == id)) return NotFound();
                throw;
            }
            return RedirectToAction(nameof(Index));
        }
        ViewBag.BranchId = new SelectList(_context.Branches, "BranchId", "BranchName", vm.BranchId);
        return View(vm);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();
        var department = await _context.Departments.Include(d => d.Branch).FirstOrDefaultAsync(m => m.DepartmentId == id);
        if (department == null) return NotFound();
        return View(department.ToViewModel());
    }

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        try
        {
            var department = await _context.Departments.FindAsync(id);
            if (department != null) _context.Departments.Remove(department);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            TempData["Error"] = "Cannot delete this Department because it has related Sections.";
        }
        return RedirectToAction(nameof(Index));
    }
}
