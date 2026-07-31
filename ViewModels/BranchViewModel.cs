using System.ComponentModel.DataAnnotations;
using MvcCrudProject.Models;

namespace MvcCrudProject.ViewModels;

public class BranchViewModel
{
    public int BranchId { get; set; }

    [Required(ErrorMessage = "Branch name is required.")]
    [StringLength(100, ErrorMessage = "Branch name cannot exceed 100 characters.")]
    [Display(Name = "Branch Name")]
    public string BranchName { get; set; } = null!;

    [Required(ErrorMessage = "Please select an Area.")]
    [Display(Name = "Area")]
    public int AreaId { get; set; }

    [Display(Name = "Area Name")]
    public string? AreaName { get; set; }

    [Required(ErrorMessage = "Please select a Company.")]
    [Display(Name = "Company")]
    public int CompanyId { get; set; }

    [Display(Name = "Company Name")]
    public string? CompanyName { get; set; }

    public Area? Area { get; set; }
    public Company? Company { get; set; }
}
