using System.ComponentModel.DataAnnotations;

namespace MvcCrudProject.ViewModels;

public class CompanyViewModel
{
    public int CompanyId { get; set; }

    [Required(ErrorMessage = "Company name is required.")]
    [StringLength(100, ErrorMessage = "Company name cannot exceed 100 characters.")]
    [Display(Name = "Company Name")]
    public string CompanyName { get; set; } = null!;
}
