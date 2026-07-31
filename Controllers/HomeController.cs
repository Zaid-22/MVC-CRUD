using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MvcCrudProject.Models;

namespace MvcCrudProject.Controllers;

public class HomeController : Controller
{
    private readonly AppDbContext _context;

    public HomeController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        ViewBag.GalaxyCount = await _context.Galaxies.CountAsync();
        ViewBag.PlanetCount = await _context.Planets.CountAsync();
        ViewBag.ContinentCount = await _context.Continents.CountAsync();
        ViewBag.RegionCount = await _context.Regions.CountAsync();
        ViewBag.CountryCount = await _context.Countries.CountAsync();
        ViewBag.CityCount = await _context.Cities.CountAsync();
        ViewBag.AreaCount = await _context.Areas.CountAsync();
        ViewBag.CompanyCount = await _context.Companies.CountAsync();
        ViewBag.BranchCount = await _context.Branches.CountAsync();
        ViewBag.DepartmentCount = await _context.Departments.CountAsync();
        ViewBag.SectionCount = await _context.Sections.CountAsync();
        ViewBag.EmployeeCount = await _context.Employees.CountAsync();
        return View();
    }
}
