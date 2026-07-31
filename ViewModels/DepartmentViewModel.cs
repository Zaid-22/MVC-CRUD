using System.ComponentModel.DataAnnotations;
using MvcCrudProject.Models;

namespace MvcCrudProject.ViewModels;

public class DepartmentViewModel
{
    public int DepartmentId { get; set; }

    [Required(ErrorMessage = "Department name is required.")]
    [StringLength(100, ErrorMessage = "Department name cannot exceed 100 characters.")]
    [Display(Name = "Department Name")]
    public string DepartmentName { get; set; } = null!;

    [Required(ErrorMessage = "Please select a Branch.")]
    [Display(Name = "Branch")]
    public int BranchId { get; set; }

    [Display(Name = "Branch Name")]
    public string? BranchName { get; set; }

    public Branch? Branch { get; set; }
}
