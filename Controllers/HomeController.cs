using Microsoft.AspNetCore.Mvc;
using MvcCrudProject.Models;
using MvcCrudProject.Repositories;

namespace MvcCrudProject.Controllers;

public class HomeController : Controller
{
    private readonly IGenericRepository<Galaxy> _galaxyRepo;
    private readonly IGenericRepository<Planet> _planetRepo;
    private readonly IGenericRepository<Continent> _continentRepo;
    private readonly IGenericRepository<Region> _regionRepo;
    private readonly IGenericRepository<Country> _countryRepo;
    private readonly IGenericRepository<City> _cityRepo;
    private readonly IGenericRepository<Area> _areaRepo;
    private readonly IGenericRepository<Company> _companyRepo;
    private readonly IGenericRepository<Branch> _branchRepo;
    private readonly IGenericRepository<Department> _departmentRepo;
    private readonly IGenericRepository<Section> _sectionRepo;
    private readonly IGenericRepository<Employee> _employeeRepo;

    public HomeController(
        IGenericRepository<Galaxy> galaxyRepo,
        IGenericRepository<Planet> planetRepo,
        IGenericRepository<Continent> continentRepo,
        IGenericRepository<Region> regionRepo,
        IGenericRepository<Country> countryRepo,
        IGenericRepository<City> cityRepo,
        IGenericRepository<Area> areaRepo,
        IGenericRepository<Company> companyRepo,
        IGenericRepository<Branch> branchRepo,
        IGenericRepository<Department> departmentRepo,
        IGenericRepository<Section> sectionRepo,
        IGenericRepository<Employee> employeeRepo)
    {
        _galaxyRepo = galaxyRepo;
        _planetRepo = planetRepo;
        _continentRepo = continentRepo;
        _regionRepo = regionRepo;
        _countryRepo = countryRepo;
        _cityRepo = cityRepo;
        _areaRepo = areaRepo;
        _companyRepo = companyRepo;
        _branchRepo = branchRepo;
        _departmentRepo = departmentRepo;
        _sectionRepo = sectionRepo;
        _employeeRepo = employeeRepo;
    }

    public async Task<IActionResult> Index()
    {
        ViewBag.GalaxyCount = await _galaxyRepo.CountAsync();
        ViewBag.PlanetCount = await _planetRepo.CountAsync();
        ViewBag.ContinentCount = await _continentRepo.CountAsync();
        ViewBag.RegionCount = await _regionRepo.CountAsync();
        ViewBag.CountryCount = await _countryRepo.CountAsync();
        ViewBag.CityCount = await _cityRepo.CountAsync();
        ViewBag.AreaCount = await _areaRepo.CountAsync();
        ViewBag.CompanyCount = await _companyRepo.CountAsync();
        ViewBag.BranchCount = await _branchRepo.CountAsync();
        ViewBag.DepartmentCount = await _departmentRepo.CountAsync();
        ViewBag.SectionCount = await _sectionRepo.CountAsync();
        ViewBag.EmployeeCount = await _employeeRepo.CountAsync();
        return View();
    }
}
