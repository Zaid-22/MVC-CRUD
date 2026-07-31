using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MvcCrudProject.Models;
using MvcCrudProject.ViewModels;

namespace MvcCrudProject.Controllers;

public class SectionController : Controller
{
    private readonly AppDbContext _context;
    public SectionController(AppDbContext context) => _context = context;

    public async Task<IActionResult> Index() =>
        View(await _context.Sections
            .Select(s => new SectionViewModel
            {
                SectionId = s.SectionId,
                SectionName = s.SectionName,
                DepartmentId = s.DepartmentId,
                DepartmentName = s.Department.DepartmentName,
                Department = s.Department
            }).ToListAsync());

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();
        var section = await _context.Sections.Include(s => s.Department).FirstOrDefaultAsync(m => m.SectionId == id);
        if (section == null) return NotFound();
        return View(new SectionViewModel
        {
            SectionId = section.SectionId,
            SectionName = section.SectionName,
            DepartmentId = section.DepartmentId,
            DepartmentName = section.Department?.DepartmentName,
            Department = section.Department
        });
    }

    public IActionResult Create()
    {
        ViewBag.DepartmentId = new SelectList(_context.Departments, "DepartmentId", "DepartmentName");
        return View();
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(SectionViewModel vm)
    {
        if (ModelState.IsValid)
        {
            var section = new Section { SectionName = vm.SectionName, DepartmentId = vm.DepartmentId };
            _context.Add(section);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewBag.DepartmentId = new SelectList(_context.Departments, "DepartmentId", "DepartmentName", vm.DepartmentId);
        return View(vm);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();
        var section = await _context.Sections.FindAsync(id);
        if (section == null) return NotFound();

        ViewBag.DepartmentId = new SelectList(_context.Departments, "DepartmentId", "DepartmentName", section.DepartmentId);
        return View(new SectionViewModel { SectionId = section.SectionId, SectionName = section.SectionName, DepartmentId = section.DepartmentId });
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, SectionViewModel vm)
    {
        if (id != vm.SectionId) return NotFound();
        if (ModelState.IsValid)
        {
            try
            {
                var section = await _context.Sections.FindAsync(id);
                if (section == null) return NotFound();
                section.SectionName = vm.SectionName;
                section.DepartmentId = vm.DepartmentId;
                _context.Update(section);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Sections.Any(e => e.SectionId == id)) return NotFound();
                throw;
            }
            return RedirectToAction(nameof(Index));
        }
        ViewBag.DepartmentId = new SelectList(_context.Departments, "DepartmentId", "DepartmentName", vm.DepartmentId);
        return View(vm);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();
        var section = await _context.Sections.Include(s => s.Department).FirstOrDefaultAsync(m => m.SectionId == id);
        if (section == null) return NotFound();
        return View(new SectionViewModel
        {
            SectionId = section.SectionId,
            SectionName = section.SectionName,
            DepartmentId = section.DepartmentId,
            DepartmentName = section.Department?.DepartmentName,
            Department = section.Department
        });
    }

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        try
        {
            var section = await _context.Sections.FindAsync(id);
            if (section != null) _context.Sections.Remove(section);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            TempData["Error"] = "Cannot delete this Section because it has related Employees.";
        }
        return RedirectToAction(nameof(Index));
    }
}
