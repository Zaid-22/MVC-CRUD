using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MvcCrudProject.Models;
using MvcCrudProject.ViewModels;

namespace MvcCrudProject.Controllers;

public class CompanyController : Controller
{
    private readonly AppDbContext _context;
    public CompanyController(AppDbContext context) => _context = context;

    public async Task<IActionResult> Index() =>
        View(await _context.Companies
            .Select(c => new CompanyViewModel { CompanyId = c.CompanyId, CompanyName = c.CompanyName })
            .ToListAsync());

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();
        var company = await _context.Companies.FirstOrDefaultAsync(m => m.CompanyId == id);
        if (company == null) return NotFound();
        return View(new CompanyViewModel { CompanyId = company.CompanyId, CompanyName = company.CompanyName });
    }

    public IActionResult Create() => View();

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CompanyViewModel vm)
    {
        if (ModelState.IsValid)
        {
            var company = new Company { CompanyName = vm.CompanyName };
            _context.Add(company);
            await _context.SaveChangesAsync();
            return RedirectToAction("Create", "Branch", new { companyId = company.CompanyId });
        }
        return View(vm);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();
        var company = await _context.Companies.FindAsync(id);
        if (company == null) return NotFound();
        return View(new CompanyViewModel { CompanyId = company.CompanyId, CompanyName = company.CompanyName });
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, CompanyViewModel vm)
    {
        if (id != vm.CompanyId) return NotFound();
        if (ModelState.IsValid)
        {
            try
            {
                var company = await _context.Companies.FindAsync(id);
                if (company == null) return NotFound();
                company.CompanyName = vm.CompanyName;
                _context.Update(company);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Companies.Any(e => e.CompanyId == id)) return NotFound();
                throw;
            }
            return RedirectToAction(nameof(Index));
        }
        return View(vm);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();
        var company = await _context.Companies.FirstOrDefaultAsync(m => m.CompanyId == id);
        if (company == null) return NotFound();
        return View(new CompanyViewModel { CompanyId = company.CompanyId, CompanyName = company.CompanyName });
    }

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        try
        {
            var company = await _context.Companies.FindAsync(id);
            if (company != null) _context.Companies.Remove(company);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            TempData["Error"] = "Cannot delete this Company because it has related Branches.";
        }
        return RedirectToAction(nameof(Index));
    }
}
