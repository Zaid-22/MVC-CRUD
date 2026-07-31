# AGENTS.md — ASP.NET Core MVC CRUD Project (Database First + OneUI Template)

## Project Overview

This is an **ASP.NET Core MVC** web application built using the **Database-First** approach with **Entity Framework Core**. The UI uses the **OneUI Bootstrap 5 Admin Template** (pixelcave). The project implements full **CRUD operations** for all 12 entities derived from the relational database schema.

---

## Database Schema (ERD Summary)

The database has **12 tables** with the following hierarchy and foreign-key relationships:

```
Galaxy
  └── Planet (GalaxyId)
        └── Continent (PlanetId)
              └── Region (ContinentId)
                    └── Country (RegionId)
                          └── City (CountryId)
                                └── Area (CityId)

Company
  └── Branch (CompanyId, AreaId)  ← also relates to Area
        └── Department (BranchId)
              └── Section (DepartmentId)
                    └── Employee (SectionId)
```

### Tables & Columns

| Table       | Primary Key   | Columns                                     | Foreign Keys             |
|-------------|---------------|---------------------------------------------|--------------------------|
| Galaxy      | GalaxyId      | GalaxyId, GalaxyName                        | —                        |
| Planet      | PlanetId      | PlanetId, PlanetName, GalaxyId              | Galaxy.GalaxyId          |
| Continent   | ContinentId   | ContinentId, ContinentName, PlanetId        | Planet.PlanetId          |
| Region      | RegionId      | RegionId, RegionName, ContinentId           | Continent.ContinentId    |
| Country     | CountryId     | CountryId, CountryName, RegionId            | Region.RegionId          |
| City        | CityId        | CityId, CityName, CountryId                 | Country.CountryId        |
| Area        | AreaId        | AreaId, AreaName, CityId                    | City.CityId              |
| Company     | CompanyId     | CompanyId, CompanyName                      | —                        |
| Branch      | BranchId      | BranchId, BranchName, AreaId, CompanyId     | Area.AreaId, Company.CompanyId |
| Department  | DepartmentId  | DepartmentId, DepartmentName, BranchId      | Branch.BranchId          |
| Section     | SectionId     | SectionId, SectionName, DepartmentId        | Department.DepartmentId  |
| Employee    | EmployeeId    | EmployeeId, EmployeeName, SectionId         | Section.SectionId        |

---

## Technology Stack

| Layer        | Technology                                          |
|--------------|-----------------------------------------------------|
| Framework    | ASP.NET Core MVC (.NET 8)                           |
| ORM          | Entity Framework Core (Database-First)              |
| Database     | SQL Server (LocalDB or full SQL Server)             |
| UI Template  | OneUI 5.x — Bootstrap 5 Admin Template (pixelcave) |
| CSS Library  | Bootstrap 5 (bundled in OneUI)                      |
| Icons        | Font Awesome, Simple Icons (si) — included in OneUI |
| JS           | oneui.app.min.js (bundled jQuery + Bootstrap 5 JS)  |
| Scaffolding  | EF Core `dotnet-ef` CLI + MVC scaffolding            |

---

## Project Structure

```
MvcCrudProject/
├── MvcCrudProject.sln
├── MvcCrudProject/
│   ├── Controllers/
│   │   ├── HomeController.cs          ← Dashboard / Index
│   │   ├── GalaxyController.cs
│   │   ├── PlanetController.cs
│   │   ├── ContinentController.cs
│   │   ├── RegionController.cs
│   │   ├── CountryController.cs
│   │   ├── CityController.cs
│   │   ├── AreaController.cs
│   │   ├── CompanyController.cs
│   │   ├── BranchController.cs
│   │   ├── DepartmentController.cs
│   │   ├── SectionController.cs
│   │   └── EmployeeController.cs
│   ├── Models/
│   │   ├── AppDbContext.cs             ← EF Core DbContext (scaffolded DB-First)
│   │   ├── Galaxy.cs
│   │   ├── Planet.cs
│   │   ├── Continent.cs
│   │   ├── Region.cs
│   │   ├── Country.cs
│   │   ├── City.cs
│   │   ├── Area.cs
│   │   ├── Company.cs
│   │   ├── Branch.cs
│   │   ├── Department.cs
│   │   ├── Section.cs
│   │   └── Employee.cs
│   ├── Views/
│   │   ├── Shared/
│   │   │   ├── _Layout.cshtml          ← OneUI base layout (sidebar, header, footer)
│   │   │   ├── _SidebarNav.cshtml      ← Partial: sidebar navigation links
│   │   │   ├── _Header.cshtml          ← Partial: top header
│   │   │   └── _ValidationScripts.cshtml
│   │   ├── Home/
│   │   │   └── Index.cshtml            ← Dashboard page
│   │   ├── Galaxy/
│   │   │   ├── Index.cshtml            ← List with DataTable
│   │   │   ├── Create.cshtml
│   │   │   ├── Edit.cshtml
│   │   │   ├── Details.cshtml
│   │   │   └── Delete.cshtml
│   │   ├── Planet/   (same 5 views)
│   │   ├── Continent/
│   │   ├── Region/
│   │   ├── Country/
│   │   ├── City/
│   │   ├── Area/
│   │   ├── Company/
│   │   ├── Branch/
│   │   ├── Department/
│   │   ├── Section/
│   │   └── Employee/
│   ├── wwwroot/
│   │   ├── assets/                     ← OneUI compiled assets (copy from template)
│   │   │   ├── css/
│   │   │   │   ├── oneui.min.css
│   │   │   │   └── themes/
│   │   │   ├── js/
│   │   │   │   ├── oneui.app.min.js
│   │   │   │   └── setTheme.js
│   │   │   ├── fonts/
│   │   │   └── media/
│   │   │       ├── avatars/
│   │   │       └── favicons/
│   ├── appsettings.json
│   ├── Program.cs
│   └── MvcCrudProject.csproj
```

---

## Step-by-Step Implementation Guide

### Step 1 — Create the SQL Server Database

Run the following SQL script to create the database and all 12 tables:

```sql
CREATE DATABASE OrganizationDB;
GO
USE OrganizationDB;
GO

CREATE TABLE Galaxy (
    GalaxyId   INT PRIMARY KEY IDENTITY(1,1),
    GalaxyName NVARCHAR(100) NOT NULL
);

CREATE TABLE Planet (
    PlanetId   INT PRIMARY KEY IDENTITY(1,1),
    PlanetName NVARCHAR(100) NOT NULL,
    GalaxyId   INT NOT NULL FOREIGN KEY REFERENCES Galaxy(GalaxyId)
);

CREATE TABLE Continent (
    ContinentId   INT PRIMARY KEY IDENTITY(1,1),
    ContinentName NVARCHAR(100) NOT NULL,
    PlanetId      INT NOT NULL FOREIGN KEY REFERENCES Planet(PlanetId)
);

CREATE TABLE Region (
    RegionId      INT PRIMARY KEY IDENTITY(1,1),
    RegionName    NVARCHAR(100) NOT NULL,
    ContinentId   INT NOT NULL FOREIGN KEY REFERENCES Continent(ContinentId)
);

CREATE TABLE Country (
    CountryId   INT PRIMARY KEY IDENTITY(1,1),
    CountryName NVARCHAR(100) NOT NULL,
    RegionId    INT NOT NULL FOREIGN KEY REFERENCES Region(RegionId)
);

CREATE TABLE City (
    CityId    INT PRIMARY KEY IDENTITY(1,1),
    CityName  NVARCHAR(100) NOT NULL,
    CountryId INT NOT NULL FOREIGN KEY REFERENCES Country(CountryId)
);

CREATE TABLE Area (
    AreaId   INT PRIMARY KEY IDENTITY(1,1),
    AreaName NVARCHAR(100) NOT NULL,
    CityId   INT NOT NULL FOREIGN KEY REFERENCES City(CityId)
);

CREATE TABLE Company (
    CompanyId   INT PRIMARY KEY IDENTITY(1,1),
    CompanyName NVARCHAR(100) NOT NULL
);

CREATE TABLE Branch (
    BranchId   INT PRIMARY KEY IDENTITY(1,1),
    BranchName NVARCHAR(100) NOT NULL,
    AreaId     INT NOT NULL FOREIGN KEY REFERENCES Area(AreaId),
    CompanyId  INT NOT NULL FOREIGN KEY REFERENCES Company(CompanyId)
);

CREATE TABLE Department (
    DepartmentId   INT PRIMARY KEY IDENTITY(1,1),
    DepartmentName NVARCHAR(100) NOT NULL,
    BranchId       INT NOT NULL FOREIGN KEY REFERENCES Branch(BranchId)
);

CREATE TABLE Section (
    SectionId   INT PRIMARY KEY IDENTITY(1,1),
    SectionName NVARCHAR(100) NOT NULL,
    DepartmentId INT NOT NULL FOREIGN KEY REFERENCES Department(DepartmentId)
);

CREATE TABLE Employee (
    EmployeeId   INT PRIMARY KEY IDENTITY(1,1),
    EmployeeName NVARCHAR(100) NOT NULL,
    SectionId    INT NOT NULL FOREIGN KEY REFERENCES Section(SectionId)
);
```

---

### Step 2 — Create the ASP.NET Core MVC Project

```bash
dotnet new mvc -n MvcCrudProject
cd MvcCrudProject
```

---

### Step 3 — Install EF Core Packages

```bash
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Microsoft.EntityFrameworkCore.Tools
dotnet tool install --global dotnet-ef
```

---

### Step 4 — Scaffold Models from Database (Database-First)

```bash
dotnet ef dbcontext scaffold \
  "Server=(localdb)\mssqllocaldb;Database=OrganizationDB;Trusted_Connection=True;" \
  Microsoft.EntityFrameworkCore.SqlServer \
  --output-dir Models \
  --context AppDbContext \
  --context-dir Models \
  --force \
  --no-onconfiguring
```

> **Result:** All 12 model classes + `AppDbContext.cs` are auto-generated in `Models/`.

---

### Step 5 — Configure Connection String

In `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=OrganizationDB;Trusted_Connection=True;MultipleActiveResultSets=true"
  }
}
```

In `Program.cs`:

```csharp
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
```

---

### Step 6 — Copy OneUI Template Assets

Copy the compiled assets from the OneUI HTML template into `wwwroot/assets/`:

```
Source:      01 OneUI Source (HTML)/src/assets/
Destination: MvcCrudProject/wwwroot/assets/
```

Files to copy:
- `assets/css/oneui.min.css` and `assets/css/themes/`
- `assets/js/oneui.app.min.js` and `assets/js/setTheme.js`
- `assets/fonts/`
- `assets/media/favicons/` and `assets/media/avatars/`

---

### Step 7 — Create the Shared Layout (`_Layout.cshtml`)

The layout is derived from the template file `gs_backend.html`. Key structure:

```html
<!DOCTYPE html>
<html lang="en" class="remember-theme">
<head>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width,initial-scale=1.0">
    <title>@ViewData["Title"] - Organization Manager</title>
    <link rel="stylesheet" href="~/assets/css/oneui.min.css" asp-append-version="true">
    <script src="~/assets/js/setTheme.js"></script>
</head>
<body>
  <div id="page-container" class="sidebar-o sidebar-dark enable-page-overlay side-scroll page-header-fixed main-content-narrow">

    <!-- Sidebar Navigation -->
    <nav id="sidebar" aria-label="Main Navigation">
      <div class="content-header">
        <a class="fw-semibold text-dual" asp-controller="Home" asp-action="Index">
          <span class="smini-hide fs-5 tracking-wider">OrgManager</span>
        </a>
      </div>
      <div class="js-sidebar-scroll">
        <div class="content-side">
          <partial name="_SidebarNav" />
        </div>
      </div>
    </nav>

    <!-- Header -->
    <header id="page-header">
      <div class="content-header">
        <div class="d-flex align-items-center">
          <button type="button" class="btn btn-sm btn-alt-secondary me-2 d-lg-none" data-toggle="layout" data-action="sidebar_toggle">
            <i class="fa fa-fw fa-bars"></i>
          </button>
        </div>
        <div class="d-flex align-items-center">
          <!-- User Dropdown -->
          <div class="dropdown d-inline-block ms-2">
            <button type="button" class="btn btn-sm btn-alt-secondary" id="page-header-user-dropdown" data-bs-toggle="dropdown">
              <i class="fa fa-fw fa-user"></i>
              <span class="d-none d-sm-inline-block ms-2">Admin</span>
            </button>
          </div>
        </div>
      </div>
    </header>

    <!-- Main Content -->
    <main id="main-container">
      <div class="bg-body-light">
        <div class="content content-full">
          <div class="d-flex flex-column flex-sm-row justify-content-sm-between align-items-sm-center py-2">
            <div class="flex-grow-1">
              <h1 class="h3 fw-bold mb-1">@ViewData["PageTitle"]</h1>
              <h2 class="fs-base lh-base fw-medium text-muted mb-0">@ViewData["PageSubtitle"]</h2>
            </div>
          </div>
        </div>
      </div>
      <div class="content">
        @RenderBody()
      </div>
    </main>

    <!-- Footer -->
    <footer id="page-footer" class="bg-body-light">
      <div class="content py-3">
        <div class="row fs-sm">
          <div class="col-sm-6 order-sm-2 py-1 text-center text-sm-end">
            Organization Manager &copy; <span data-toggle="year-copy"></span>
          </div>
        </div>
      </div>
    </footer>
  </div>

  <script src="~/assets/js/oneui.app.min.js"></script>
  @await RenderSectionAsync("Scripts", required: false)
</body>
</html>
```

---

### Step 8 — Create Sidebar Navigation Partial (`_SidebarNav.cshtml`)

```html
<ul class="nav-main">
  <li class="nav-main-item">
    <a class="nav-main-link" asp-controller="Home" asp-action="Index">
      <i class="nav-main-link-icon si si-speedometer"></i>
      <span class="nav-main-link-name">Dashboard</span>
    </a>
  </li>

  <!-- Geographic Hierarchy -->
  <li class="nav-main-heading">Geographic</li>
  <li class="nav-main-item">
    <a class="nav-main-link nav-main-link-submenu" data-toggle="submenu" aria-haspopup="true" aria-expanded="false" href="#">
      <i class="nav-main-link-icon fa fa-globe"></i>
      <span class="nav-main-link-name">Locations</span>
    </a>
    <ul class="nav-main-submenu">
      <li class="nav-main-item"><a class="nav-main-link" asp-controller="Galaxy" asp-action="Index"><span class="nav-main-link-name">Galaxies</span></a></li>
      <li class="nav-main-item"><a class="nav-main-link" asp-controller="Planet" asp-action="Index"><span class="nav-main-link-name">Planets</span></a></li>
      <li class="nav-main-item"><a class="nav-main-link" asp-controller="Continent" asp-action="Index"><span class="nav-main-link-name">Continents</span></a></li>
      <li class="nav-main-item"><a class="nav-main-link" asp-controller="Region" asp-action="Index"><span class="nav-main-link-name">Regions</span></a></li>
      <li class="nav-main-item"><a class="nav-main-link" asp-controller="Country" asp-action="Index"><span class="nav-main-link-name">Countries</span></a></li>
      <li class="nav-main-item"><a class="nav-main-link" asp-controller="City" asp-action="Index"><span class="nav-main-link-name">Cities</span></a></li>
      <li class="nav-main-item"><a class="nav-main-link" asp-controller="Area" asp-action="Index"><span class="nav-main-link-name">Areas</span></a></li>
    </ul>
  </li>

  <!-- Organization Hierarchy -->
  <li class="nav-main-heading">Organization</li>
  <li class="nav-main-item">
    <a class="nav-main-link nav-main-link-submenu" data-toggle="submenu" aria-haspopup="true" aria-expanded="false" href="#">
      <i class="nav-main-link-icon fa fa-building"></i>
      <span class="nav-main-link-name">Structure</span>
    </a>
    <ul class="nav-main-submenu">
      <li class="nav-main-item"><a class="nav-main-link" asp-controller="Company" asp-action="Index"><span class="nav-main-link-name">Companies</span></a></li>
      <li class="nav-main-item"><a class="nav-main-link" asp-controller="Branch" asp-action="Index"><span class="nav-main-link-name">Branches</span></a></li>
      <li class="nav-main-item"><a class="nav-main-link" asp-controller="Department" asp-action="Index"><span class="nav-main-link-name">Departments</span></a></li>
      <li class="nav-main-item"><a class="nav-main-link" asp-controller="Section" asp-action="Index"><span class="nav-main-link-name">Sections</span></a></li>
      <li class="nav-main-item"><a class="nav-main-link" asp-controller="Employee" asp-action="Index"><span class="nav-main-link-name">Employees</span></a></li>
    </ul>
  </li>
</ul>
```

---

### Step 9 — Controller Pattern (Repeat for each entity)

Each controller follows this exact pattern. Example for **GalaxyController** (root entity — no FK):

```csharp
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MvcCrudProject.Models;

public class GalaxyController : Controller
{
    private readonly AppDbContext _context;

    public GalaxyController(AppDbContext context)
    {
        _context = context;
    }

    // GET: Galaxy
    public async Task<IActionResult> Index()
    {
        return View(await _context.Galaxies.ToListAsync());
    }

    // GET: Galaxy/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();
        var galaxy = await _context.Galaxies.FirstOrDefaultAsync(m => m.GalaxyId == id);
        if (galaxy == null) return NotFound();
        return View(galaxy);
    }

    // GET: Galaxy/Create
    public IActionResult Create() => View();

    // POST: Galaxy/Create
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("GalaxyName")] Galaxy galaxy)
    {
        if (ModelState.IsValid)
        {
            _context.Add(galaxy);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(galaxy);
    }

    // GET: Galaxy/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();
        var galaxy = await _context.Galaxies.FindAsync(id);
        if (galaxy == null) return NotFound();
        return View(galaxy);
    }

    // POST: Galaxy/Edit/5
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("GalaxyId,GalaxyName")] Galaxy galaxy)
    {
        if (id != galaxy.GalaxyId) return NotFound();
        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(galaxy);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Galaxies.Any(e => e.GalaxyId == id)) return NotFound();
                throw;
            }
            return RedirectToAction(nameof(Index));
        }
        return View(galaxy);
    }

    // GET: Galaxy/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();
        var galaxy = await _context.Galaxies.FirstOrDefaultAsync(m => m.GalaxyId == id);
        if (galaxy == null) return NotFound();
        return View(galaxy);
    }

    // POST: Galaxy/Delete/5
    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var galaxy = await _context.Galaxies.FindAsync(id);
        if (galaxy != null) _context.Galaxies.Remove(galaxy);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}
```

Example for **PlanetController** (entity with FK to Galaxy):

```csharp
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MvcCrudProject.Models;

public class PlanetController : Controller
{
    private readonly AppDbContext _context;

    public PlanetController(AppDbContext context)
    {
        _context = context;
    }

    // GET: Planet — Include parent Galaxy name
    public async Task<IActionResult> Index()
    {
        var planets = _context.Planets.Include(p => p.Galaxy);
        return View(await planets.ToListAsync());
    }

    // GET: Planet/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();
        var planet = await _context.Planets
            .Include(p => p.Galaxy)
            .FirstOrDefaultAsync(m => m.PlanetId == id);
        if (planet == null) return NotFound();
        return View(planet);
    }

    // GET: Planet/Create — populate FK dropdown
    public IActionResult Create()
    {
        ViewBag.GalaxyId = new SelectList(_context.Galaxies, "GalaxyId", "GalaxyName");
        return View();
    }

    // POST: Planet/Create
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("PlanetName,GalaxyId")] Planet planet)
    {
        if (ModelState.IsValid)
        {
            _context.Add(planet);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewBag.GalaxyId = new SelectList(_context.Galaxies, "GalaxyId", "GalaxyName", planet.GalaxyId);
        return View(planet);
    }

    // GET: Planet/Edit/5 — populate FK dropdown with current selection
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();
        var planet = await _context.Planets.FindAsync(id);
        if (planet == null) return NotFound();
        ViewBag.GalaxyId = new SelectList(_context.Galaxies, "GalaxyId", "GalaxyName", planet.GalaxyId);
        return View(planet);
    }

    // POST: Planet/Edit/5
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("PlanetId,PlanetName,GalaxyId")] Planet planet)
    {
        if (id != planet.PlanetId) return NotFound();
        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(planet);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Planets.Any(e => e.PlanetId == id)) return NotFound();
                throw;
            }
            return RedirectToAction(nameof(Index));
        }
        ViewBag.GalaxyId = new SelectList(_context.Galaxies, "GalaxyId", "GalaxyName", planet.GalaxyId);
        return View(planet);
    }

    // GET: Planet/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();
        var planet = await _context.Planets
            .Include(p => p.Galaxy)
            .FirstOrDefaultAsync(m => m.PlanetId == id);
        if (planet == null) return NotFound();
        return View(planet);
    }

    // POST: Planet/Delete/5
    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var planet = await _context.Planets.FindAsync(id);
        if (planet != null) _context.Planets.Remove(planet);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}
```

---

### Step 10 — View Pattern: Index (List + DataTable using OneUI Block)

```razor
@model IEnumerable<MvcCrudProject.Models.Galaxy>
@{
    ViewData["Title"] = "Galaxies";
    ViewData["PageTitle"] = "Galaxies";
    ViewData["PageSubtitle"] = "Manage all galaxies";
}

<div class="block block-rounded">
    <div class="block-header block-header-default">
        <h3 class="block-title">All Galaxies</h3>
        <div class="block-options">
            <a asp-action="Create" class="btn btn-sm btn-primary">
                <i class="fa fa-plus me-1"></i> Add New
            </a>
        </div>
    </div>
    <div class="block-content">
        <table class="table table-bordered table-striped table-vcenter">
            <thead>
                <tr>
                    <th>#</th>
                    <th>Galaxy Name</th>
                    <th class="text-center" style="width:150px">Actions</th>
                </tr>
            </thead>
            <tbody>
                @foreach (var item in Model)
                {
                    <tr>
                        <td>@item.GalaxyId</td>
                        <td>@item.GalaxyName</td>
                        <td class="text-center">
                            <a asp-action="Edit" asp-route-id="@item.GalaxyId" class="btn btn-sm btn-alt-warning me-1">
                                <i class="fa fa-pencil-alt"></i>
                            </a>
                            <a asp-action="Details" asp-route-id="@item.GalaxyId" class="btn btn-sm btn-alt-info me-1">
                                <i class="fa fa-eye"></i>
                            </a>
                            <a asp-action="Delete" asp-route-id="@item.GalaxyId" class="btn btn-sm btn-alt-danger">
                                <i class="fa fa-trash"></i>
                            </a>
                        </td>
                    </tr>
                }
            </tbody>
        </table>
    </div>
</div>
```

---

### Step 11 — View Pattern: Create/Edit Form (with FK Dropdown)

```razor
@model MvcCrudProject.Models.Planet
@{
    ViewData["Title"] = "Add Planet";
    ViewData["PageTitle"] = "Add Planet";
    ViewData["PageSubtitle"] = "Create a new planet entry";
}

<div class="block block-rounded">
    <div class="block-header block-header-default">
        <h3 class="block-title">Add Planet</h3>
    </div>
    <div class="block-content">
        <form asp-action="Create">
            <div asp-validation-summary="ModelOnly" class="alert alert-danger"></div>

            <div class="mb-4">
                <label asp-for="PlanetName" class="form-label fw-semibold"></label>
                <input asp-for="PlanetName" class="form-control" placeholder="Enter planet name" />
                <span asp-validation-for="PlanetName" class="text-danger"></span>
            </div>

            <div class="mb-4">
                <label asp-for="GalaxyId" class="form-label fw-semibold">Galaxy</label>
                <select asp-for="GalaxyId" asp-items="ViewBag.GalaxyId" class="form-select">
                    <option value="">-- Select Galaxy --</option>
                </select>
                <span asp-validation-for="GalaxyId" class="text-danger"></span>
            </div>

            <div class="mb-4">
                <button type="submit" class="btn btn-primary">
                    <i class="fa fa-save me-1"></i> Save
                </button>
                <a asp-action="Index" class="btn btn-alt-secondary ms-1">
                    <i class="fa fa-arrow-left me-1"></i> Back to List
                </a>
            </div>
        </form>
    </div>
</div>

@section Scripts {
    @{ await Html.RenderPartialAsync("_ValidationScriptsPartial"); }
}
```

---

### Step 12 — View Pattern: Details

```razor
@model MvcCrudProject.Models.Planet
@{
    ViewData["Title"] = "Planet Details";
    ViewData["PageTitle"] = "Planet Details";
    ViewData["PageSubtitle"] = "View planet information";
}

<div class="block block-rounded">
    <div class="block-header block-header-default">
        <h3 class="block-title">Planet Details</h3>
    </div>
    <div class="block-content">
        <dl class="row">
            <dt class="col-sm-3">Planet Name</dt>
            <dd class="col-sm-9">@Model.PlanetName</dd>

            <dt class="col-sm-3">Galaxy</dt>
            <dd class="col-sm-9">@Model.Galaxy?.GalaxyName</dd>
        </dl>
        <div class="mb-4">
            <a asp-action="Edit" asp-route-id="@Model.PlanetId" class="btn btn-alt-warning">
                <i class="fa fa-pencil-alt me-1"></i> Edit
            </a>
            <a asp-action="Index" class="btn btn-alt-secondary ms-1">
                <i class="fa fa-arrow-left me-1"></i> Back to List
            </a>
        </div>
    </div>
</div>
```

---

### Step 13 — View Pattern: Delete Confirmation

```razor
@model MvcCrudProject.Models.Planet
@{
    ViewData["Title"] = "Delete Planet";
    ViewData["PageTitle"] = "Delete Planet";
    ViewData["PageSubtitle"] = "Confirm deletion";
}

<div class="block block-rounded">
    <div class="block-header block-header-default bg-danger-light">
        <h3 class="block-title text-danger">
            <i class="fa fa-exclamation-triangle me-1"></i> Confirm Delete
        </h3>
    </div>
    <div class="block-content">
        <p class="fw-semibold">Are you sure you want to delete this planet?</p>
        <dl class="row">
            <dt class="col-sm-3">Planet Name</dt>
            <dd class="col-sm-9">@Model.PlanetName</dd>
            <dt class="col-sm-3">Galaxy</dt>
            <dd class="col-sm-9">@Model.Galaxy?.GalaxyName</dd>
        </dl>
        <form asp-action="Delete">
            <input type="hidden" asp-for="PlanetId" />
            <button type="submit" class="btn btn-danger">
                <i class="fa fa-trash me-1"></i> Delete
            </button>
            <a asp-action="Index" class="btn btn-alt-secondary ms-1">
                <i class="fa fa-arrow-left me-1"></i> Back to List
            </a>
        </form>
    </div>
</div>
```

---

## Entity Controllers — FK SelectList Reference

| Entity     | ViewBag Setup Required (in Create/Edit GET actions)                                 |
|------------|-------------------------------------------------------------------------------------|
| Galaxy     | _(none — root entity)_                                                              |
| Planet     | `ViewBag.GalaxyId = new SelectList(_context.Galaxies, "GalaxyId", "GalaxyName")`    |
| Continent  | `ViewBag.PlanetId = new SelectList(_context.Planets, "PlanetId", "PlanetName")`     |
| Region     | `ViewBag.ContinentId = new SelectList(_context.Continents, "ContinentId", "ContinentName")` |
| Country    | `ViewBag.RegionId = new SelectList(_context.Regions, "RegionId", "RegionName")`     |
| City       | `ViewBag.CountryId = new SelectList(_context.Countries, "CountryId", "CountryName")`|
| Area       | `ViewBag.CityId = new SelectList(_context.Cities, "CityId", "CityName")`            |
| Company    | _(none — root entity)_                                                              |
| Branch     | `ViewBag.AreaId = new SelectList(...)` **+** `ViewBag.CompanyId = new SelectList(...)` |
| Department | `ViewBag.BranchId = new SelectList(_context.Branches, "BranchId", "BranchName")`   |
| Section    | `ViewBag.DepartmentId = new SelectList(_context.Departments, "DepartmentId", "DepartmentName")` |
| Employee   | `ViewBag.SectionId = new SelectList(_context.Sections, "SectionId", "SectionName")`|

---

## Entity Controllers — Include() Reference for Index/Details

| Entity     | Include Needed in Index & Details                     |
|------------|-------------------------------------------------------|
| Galaxy     | _(none)_                                              |
| Planet     | `.Include(p => p.Galaxy)`                             |
| Continent  | `.Include(c => c.Planet)`                             |
| Region     | `.Include(r => r.Continent)`                          |
| Country    | `.Include(c => c.Region)`                             |
| City       | `.Include(c => c.Country)`                            |
| Area       | `.Include(a => a.City)`                               |
| Company    | _(none)_                                              |
| Branch     | `.Include(b => b.Area).Include(b => b.Company)`       |
| Department | `.Include(d => d.Branch)`                             |
| Section    | `.Include(s => s.Department)`                         |
| Employee   | `.Include(e => e.Section)`                            |

---

## Naming Conventions

| Item                | Convention           | Example                        |
|---------------------|----------------------|--------------------------------|
| Model class         | PascalCase singular  | `Galaxy`, `Employee`           |
| DbSet property      | PascalCase plural    | `Galaxies`, `Employees`        |
| Controller          | PascalCase + suffix  | `GalaxyController`             |
| View folder         | Match entity name    | `Views/Galaxy/Index.cshtml`    |
| PK column           | EntityName + "Id"    | `GalaxyId`, `EmployeeId`       |
| FK column           | Referenced entity PK | `GalaxyId` in `Planet`         |
| Connection string   | `DefaultConnection`  | In `appsettings.json`          |

---

## OneUI Template CSS Classes Quick Reference

| Purpose               | Class(es)                                              |
|-----------------------|--------------------------------------------------------|
| Page wrapper          | `#page-container .sidebar-o .sidebar-dark .enable-page-overlay .side-scroll .page-header-fixed .main-content-narrow` |
| Content block         | `block block-rounded`                                  |
| Block header          | `block-header block-header-default`                    |
| Block content         | `block-content`                                        |
| Block title           | `block-title`                                          |
| Table style           | `table table-bordered table-striped table-vcenter`     |
| Primary button        | `btn btn-primary`                                      |
| Warning button (alt)  | `btn btn-alt-warning`                                  |
| Danger button (alt)   | `btn btn-alt-danger`                                   |
| Info button (alt)     | `btn btn-alt-info`                                     |
| Secondary button (alt)| `btn btn-alt-secondary`                                |
| Form input            | `form-control`                                         |
| Form select           | `form-select`                                          |
| Form label            | `form-label fw-semibold`                               |
| Alert/Validation      | `alert alert-danger`                                   |
| Nav sidebar heading   | `nav-main-heading`                                     |
| Nav sidebar item      | `nav-main-item`                                        |
| Nav sidebar link      | `nav-main-link`                                        |
| Nav sidebar submenu toggle | `nav-main-link-submenu` + `data-toggle="submenu"` |
| Submenu container     | `nav-main-submenu`                                     |
| Icon in nav link      | `nav-main-link-icon`                                   |
| Text in nav link      | `nav-main-link-name`                                   |

---

## Agent Coding Rules

1. **Always use `async/await`** for all database operations (EF Core).
2. **Always include `[ValidateAntiForgeryToken]`** on all POST actions.
3. **Never hardcode connection strings** — always use `appsettings.json` + `IConfiguration`.
4. **Use Tag Helpers** (`asp-controller`, `asp-action`, `asp-for`, `asp-items`, `asp-validation-for`) — never build raw HTML form attributes manually.
5. **Use partial views** for sidebar navigation and validation scripts to keep views DRY.
6. **Apply `.Include()`** when navigating foreign keys in Index/Details views (e.g., `_context.Planets.Include(p => p.Galaxy).ToListAsync()`).
7. **Use `SelectList` with `ViewBag`** for dropdown FK population in Create and Edit GET actions. Always re-populate the `ViewBag` on POST failure (validation error).
8. **Use `[Bind()]`** in POST actions to prevent over-posting attacks — only include updatable fields.
9. **Always set `ViewData["Title"]`**, `ViewData["PageTitle"]`, and `ViewData["PageSubtitle"]` in each view.
10. **OneUI JS** (`oneui.app.min.js`) must be loaded at the **bottom of `<body>`**, not in `<head>`.
11. **`setTheme.js`** must be in `<head>` (blocking) to prevent dark mode flash of unstyled content.
12. **Validation scripts partial** (`_ValidationScriptsPartial.cshtml`) should only render in the `Scripts` section via `@section Scripts {}`, not globally.
13. When deleting an entity that has **child records**, EF Core will throw a `DbUpdateException` — always handle this gracefully with a try/catch and user-friendly error message.
14. **OneUI sidebar submenu** requires `data-toggle="submenu"` on the parent `<a>` — do not remove this attribute or the submenu will not expand/collapse.

---

## Verification Checklist

- [ ] SQL Server database `OrganizationDB` created with all 12 tables
- [ ] EF Core scaffold generates all 12 model files + `AppDbContext.cs`
- [ ] `appsettings.json` connection string configured correctly
- [ ] `Program.cs` registers `AppDbContext` with DI container
- [ ] OneUI assets copied to `wwwroot/assets/`
- [ ] `_Layout.cshtml` loads `oneui.min.css` and `oneui.app.min.js` correctly
- [ ] Sidebar renders all 12 entity navigation links
- [ ] All 12 controllers implement: Index, Details, Create (GET+POST), Edit (GET+POST), Delete (GET+POST)
- [ ] All FK dropdowns populate correctly (SelectList with ViewBag)
- [ ] Index views display FK related names (not raw IDs) using `.Include()`
- [ ] All forms use Tag Helpers which auto-add CSRF tokens
- [ ] Client-side validation scripts are included in Create/Edit forms
- [ ] Dark mode toggle and theme switcher functional (requires `setTheme.js` in `<head>`)
- [ ] Application runs without errors via `dotnet run`

---

## Common Mistakes to Avoid

| Mistake | Fix |
|---------|-----|
| Scaffold overwrites model customizations | Use `--no-onconfiguring` and keep customizations in separate partial classes |
| FK shown as number in Index view | Use `.Include()` to load navigation properties, display `@item.Galaxy.GalaxyName` |
| Dropdown shows empty options | Set `ViewBag` in **both** GET Create and GET Edit, and also re-set on POST failure |
| CSRF token missing | Use `<form asp-action="...">` Tag Helper (auto-adds token) + `[ValidateAntiForgeryToken]` |
| OneUI sidebar submenu not expanding | Ensure `oneui.app.min.js` is loaded and `data-toggle="submenu"` is present on parent link |
| Dark mode flash on page load | Move `<script src="setTheme.js">` to `<head>` before CSS |
| Delete fails on parent with children | Catch `DbUpdateException` and show: "Cannot delete — child records exist" |
| Views not found (404) | Ensure view folders match controller name exactly: `Views/Galaxy/`, not `Views/Galaxies/` |

---

*This AGENTS.md is the single source of truth for building the MVC CRUD project. All AI agents and developers should follow this document when generating, modifying, or reviewing code for this project.*
