using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MvcCrudProject.Models;
using MvcCrudProject.ViewModels;

namespace MvcCrudProject.Controllers;

public class DepartmentController : Controller
{
    private readonly AppDbContext _context;
    public DepartmentController(AppDbContext context) => _context = context;

    public async Task<IActionResult> Index() =>
        View(await _context.Departments
            .Select(d => new DepartmentViewModel
            {
                DepartmentId = d.DepartmentId,
                DepartmentName = d.DepartmentName,
                BranchId = d.BranchId,
                BranchName = d.Branch.BranchName,
                Branch = d.Branch
            }).ToListAsync());

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();
        var department = await _context.Departments.Include(d => d.Branch).FirstOrDefaultAsync(m => m.DepartmentId == id);
        if (department == null) return NotFound();
        return View(new DepartmentViewModel
        {
            DepartmentId = department.DepartmentId,
            DepartmentName = department.DepartmentName,
            BranchId = department.BranchId,
            BranchName = department.Branch?.BranchName,
            Branch = department.Branch
        });
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
            var department = new Department { DepartmentName = vm.DepartmentName, BranchId = vm.BranchId };
            _context.Add(department);
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
        return View(new DepartmentViewModel { DepartmentId = department.DepartmentId, DepartmentName = department.DepartmentName, BranchId = department.BranchId });
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
        return View(new DepartmentViewModel
        {
            DepartmentId = department.DepartmentId,
            DepartmentName = department.DepartmentName,
            BranchId = department.BranchId,
            BranchName = department.Branch?.BranchName,
            Branch = department.Branch
        });
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
