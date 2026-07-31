using System.ComponentModel.DataAnnotations;
using MvcCrudProject.Models;

namespace MvcCrudProject.ViewModels;

public class SectionViewModel
{
    public int SectionId { get; set; }

    [Required(ErrorMessage = "Section name is required.")]
    [StringLength(100, ErrorMessage = "Section name cannot exceed 100 characters.")]
    [Display(Name = "Section Name")]
    public string SectionName { get; set; } = null!;

    [Required(ErrorMessage = "Please select a Department.")]
    [Display(Name = "Department")]
    public int DepartmentId { get; set; }

    [Display(Name = "Department Name")]
    public string? DepartmentName { get; set; }

    public Department? Department { get; set; }
}
