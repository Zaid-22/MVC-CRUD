using System.ComponentModel.DataAnnotations;
using MvcCrudProject.Models;

namespace MvcCrudProject.ViewModels;

public class CountryViewModel
{
    public int CountryId { get; set; }

    [Required(ErrorMessage = "Country name is required.")]
    [StringLength(100, ErrorMessage = "Country name cannot exceed 100 characters.")]
    [Display(Name = "Country Name")]
    public string CountryName { get; set; } = null!;

    [Required(ErrorMessage = "Please select a Region.")]
    [Display(Name = "Region")]
    public int RegionId { get; set; }

    [Display(Name = "Region Name")]
    public string? RegionName { get; set; }

    public Region? Region { get; set; }
}
