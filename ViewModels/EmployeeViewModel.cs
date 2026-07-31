using System.ComponentModel.DataAnnotations;
using MvcCrudProject.Models;

namespace MvcCrudProject.ViewModels;

public class EmployeeViewModel
{
    public int EmployeeId { get; set; }

    [Required(ErrorMessage = "Employee name is required.")]
    [StringLength(100, ErrorMessage = "Employee name cannot exceed 100 characters.")]
    [Display(Name = "Employee Name")]
    public string EmployeeName { get; set; } = null!;

    [Required(ErrorMessage = "Please select a Section.")]
    [Display(Name = "Section")]
    public int SectionId { get; set; }

    [Display(Name = "Section Name")]
    public string? SectionName { get; set; }

    public Section? Section { get; set; }
}
