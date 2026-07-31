using System.ComponentModel.DataAnnotations;
using MvcCrudProject.Models;

namespace MvcCrudProject.ViewModels;

public class ContinentViewModel
{
    public int ContinentId { get; set; }

    [Required(ErrorMessage = "Continent name is required.")]
    [StringLength(100, ErrorMessage = "Continent name cannot exceed 100 characters.")]
    [Display(Name = "Continent Name")]
    public string ContinentName { get; set; } = null!;

    [Required(ErrorMessage = "Please select a Planet.")]
    [Display(Name = "Planet")]
    public int PlanetId { get; set; }

    [Display(Name = "Planet Name")]
    public string? PlanetName { get; set; }

    public Planet? Planet { get; set; }
}
