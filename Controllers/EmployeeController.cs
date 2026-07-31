using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MvcCrudProject.Data;
using MvcCrudProject.Extensions;
using MvcCrudProject.Models;
using MvcCrudProject.ViewModels;

namespace MvcCrudProject.Controllers;

public class EmployeeController : Controller
{
    private readonly AppDbContext _context;
    public EmployeeController(AppDbContext context) => _context = context;

    public async Task<IActionResult> Index() =>
        View((await _context.Employees.Include(e => e.Section).ToListAsync()).ToViewModelList());

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();
        var employee = await _context.Employees.Include(e => e.Section).FirstOrDefaultAsync(m => m.EmployeeId == id);
        if (employee == null) return NotFound();
        return View(employee.ToViewModel());
    }

    public IActionResult Create()
    {
        ViewBag.SectionId = new SelectList(_context.Sections, "SectionId", "SectionName");
        return View();
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(EmployeeViewModel vm)
    {
        if (ModelState.IsValid)
        {
            _context.Add(vm.ToModel());
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewBag.SectionId = new SelectList(_context.Sections, "SectionId", "SectionName", vm.SectionId);
        return View(vm);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();
        var employee = await _context.Employees.FindAsync(id);
        if (employee == null) return NotFound();

        ViewBag.SectionId = new SelectList(_context.Sections, "SectionId", "SectionName", employee.SectionId);
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
                var employee = await _context.Employees.FindAsync(id);
                if (employee == null) return NotFound();
                employee.EmployeeName = vm.EmployeeName;
                employee.SectionId = vm.SectionId;
                _context.Update(employee);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Employees.Any(e => e.EmployeeId == id)) return NotFound();
                throw;
            }
            return RedirectToAction(nameof(Index));
        }
        ViewBag.SectionId = new SelectList(_context.Sections, "SectionId", "SectionName", vm.SectionId);
        return View(vm);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();
        var employee = await _context.Employees.Include(e => e.Section).FirstOrDefaultAsync(m => m.EmployeeId == id);
        if (employee == null) return NotFound();
        return View(employee.ToViewModel());
    }

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var employee = await _context.Employees.FindAsync(id);
        if (employee != null) _context.Employees.Remove(employee);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}
