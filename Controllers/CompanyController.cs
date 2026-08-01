using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MvcCrudProject.Extensions;
using MvcCrudProject.Models;
using MvcCrudProject.Repositories;
using MvcCrudProject.ViewModels;

namespace MvcCrudProject.Controllers;

public class CompanyController : Controller
{
    private readonly IGenericRepository<Company> _companyRepo;

    public CompanyController(IGenericRepository<Company> companyRepo)
    {
        _companyRepo = companyRepo;
    }

    public async Task<IActionResult> Index() =>
        View((await _companyRepo.GetAllAsync()).ToViewModelList());

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();
        var company = await _companyRepo.GetFirstOrDefaultAsync(m => m.CompanyId == id);
        if (company == null) return NotFound();
        return View(company.ToViewModel());
    }

    public IActionResult Create() => View();

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CompanyViewModel vm)
    {
        if (ModelState.IsValid)
        {
            await _companyRepo.AddAsync(vm.ToModel());
            await _companyRepo.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(vm);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();
        var company = await _companyRepo.GetByIdAsync(id.Value);
        if (company == null) return NotFound();
        return View(company.ToViewModel());
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, CompanyViewModel vm)
    {
        if (id != vm.CompanyId) return NotFound();
        if (ModelState.IsValid)
        {
            try
            {
                var company = await _companyRepo.GetByIdAsync(id);
                if (company == null) return NotFound();
                company.CompanyName = vm.CompanyName;
                _companyRepo.Update(company);
                await _companyRepo.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _companyRepo.ExistsAsync(e => e.CompanyId == id)) return NotFound();
                throw;
            }
            return RedirectToAction(nameof(Index));
        }
        return View(vm);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();
        var company = await _companyRepo.GetFirstOrDefaultAsync(m => m.CompanyId == id);
        if (company == null) return NotFound();
        return View(company.ToViewModel());
    }

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        try
        {
            var company = await _companyRepo.GetByIdAsync(id);
            if (company != null) _companyRepo.Remove(company);
            await _companyRepo.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            TempData["Error"] = "Cannot delete this Company because it has related Branches.";
        }
        return RedirectToAction(nameof(Index));
    }
}
