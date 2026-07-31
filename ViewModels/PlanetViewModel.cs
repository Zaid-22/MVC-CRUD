using System.ComponentModel.DataAnnotations;
using MvcCrudProject.Models;

namespace MvcCrudProject.ViewModels;

public class PlanetViewModel
{
    public int PlanetId { get; set; }

    [Required(ErrorMessage = "Planet name is required.")]
    [StringLength(100, ErrorMessage = "Planet name cannot exceed 100 characters.")]
    [Display(Name = "Planet Name")]
    public string PlanetName { get; set; } = null!;

    [Required(ErrorMessage = "Please select a Galaxy.")]
    [Display(Name = "Galaxy")]
    public int GalaxyId { get; set; }

    [Display(Name = "Galaxy Name")]
    public string? GalaxyName { get; set; }

    public Galaxy? Galaxy { get; set; }
}
