using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MvcCrudProject.Models;
using MvcCrudProject.ViewModels;

namespace MvcCrudProject.Controllers;

public class BranchController : Controller
{
    private readonly AppDbContext _context;
    public BranchController(AppDbContext context) => _context = context;

    public async Task<IActionResult> Index() =>
        View(await _context.Branches
            .Select(b => new BranchViewModel
            {
                BranchId = b.BranchId,
                BranchName = b.BranchName,
                AreaId = b.AreaId,
                AreaName = b.Area.AreaName,
                CompanyId = b.CompanyId,
                CompanyName = b.Company.CompanyName,
                Area = b.Area,
                Company = b.Company
            }).ToListAsync());

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();
        var branch = await _context.Branches.Include(b => b.Area).Include(b => b.Company).FirstOrDefaultAsync(m => m.BranchId == id);
        if (branch == null) return NotFound();
        return View(new BranchViewModel
        {
            BranchId = branch.BranchId,
            BranchName = branch.BranchName,
            AreaId = branch.AreaId,
            AreaName = branch.Area?.AreaName,
            CompanyId = branch.CompanyId,
            CompanyName = branch.Company?.CompanyName,
            Area = branch.Area,
            Company = branch.Company
        });
    }

    public IActionResult Create(int? companyId)
    {
        ViewBag.AreaId = new SelectList(_context.Areas, "AreaId", "AreaName");
        ViewBag.CompanyId = new SelectList(_context.Companies, "CompanyId", "CompanyName", companyId);
        return View(new BranchViewModel { CompanyId = companyId ?? 0 });
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(BranchViewModel vm)
    {
        if (ModelState.IsValid)
        {
            var branch = new Branch { BranchName = vm.BranchName, AreaId = vm.AreaId, CompanyId = vm.CompanyId };
            _context.Add(branch);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewBag.AreaId = new SelectList(_context.Areas, "AreaId", "AreaName", vm.AreaId);
        ViewBag.CompanyId = new SelectList(_context.Companies, "CompanyId", "CompanyName", vm.CompanyId);
        return View(vm);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();
        var branch = await _context.Branches.FindAsync(id);
        if (branch == null) return NotFound();

        ViewBag.AreaId = new SelectList(_context.Areas, "AreaId", "AreaName", branch.AreaId);
        ViewBag.CompanyId = new SelectList(_context.Companies, "CompanyId", "CompanyName", branch.CompanyId);
        return View(new BranchViewModel { BranchId = branch.BranchId, BranchName = branch.BranchName, AreaId = branch.AreaId, CompanyId = branch.CompanyId });
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, BranchViewModel vm)
    {
        if (id != vm.BranchId) return NotFound();
        if (ModelState.IsValid)
        {
            try
            {
                var branch = await _context.Branches.FindAsync(id);
                if (branch == null) return NotFound();
                branch.BranchName = vm.BranchName;
                branch.AreaId = vm.AreaId;
                branch.CompanyId = vm.CompanyId;
                _context.Update(branch);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Branches.Any(e => e.BranchId == id)) return NotFound();
                throw;
            }
            return RedirectToAction(nameof(Index));
        }
        ViewBag.AreaId = new SelectList(_context.Areas, "AreaId", "AreaName", vm.AreaId);
        ViewBag.CompanyId = new SelectList(_context.Companies, "CompanyId", "CompanyName", vm.CompanyId);
        return View(vm);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();
        var branch = await _context.Branches.Include(b => b.Area).Include(b => b.Company).FirstOrDefaultAsync(m => m.BranchId == id);
        if (branch == null) return NotFound();
        return View(new BranchViewModel
        {
            BranchId = branch.BranchId,
            BranchName = branch.BranchName,
            AreaId = branch.AreaId,
            AreaName = branch.Area?.AreaName,
            CompanyId = branch.CompanyId,
            CompanyName = branch.Company?.CompanyName,
            Area = branch.Area,
            Company = branch.Company
        });
    }

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        try
        {
            var branch = await _context.Branches.FindAsync(id);
            if (branch != null) _context.Branches.Remove(branch);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            TempData["Error"] = "Cannot delete this Branch because it has related Departments.";
        }
        return RedirectToAction(nameof(Index));
    }
}
