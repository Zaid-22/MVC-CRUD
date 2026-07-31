using System.ComponentModel.DataAnnotations;
using MvcCrudProject.Models;

namespace MvcCrudProject.ViewModels;

public class RegionViewModel
{
    public int RegionId { get; set; }

    [Required(ErrorMessage = "Region name is required.")]
    [StringLength(100, ErrorMessage = "Region name cannot exceed 100 characters.")]
    [Display(Name = "Region Name")]
    public string RegionName { get; set; } = null!;

    [Required(ErrorMessage = "Please select a Continent.")]
    [Display(Name = "Continent")]
    public int ContinentId { get; set; }

    [Display(Name = "Continent Name")]
    public string? ContinentName { get; set; }

    public Continent? Continent { get; set; }
}
