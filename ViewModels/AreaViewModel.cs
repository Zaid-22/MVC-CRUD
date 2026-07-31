using System.ComponentModel.DataAnnotations;
using MvcCrudProject.Models;

namespace MvcCrudProject.ViewModels;

public class AreaViewModel
{
    public int AreaId { get; set; }

    [Required(ErrorMessage = "Area name is required.")]
    [StringLength(100, ErrorMessage = "Area name cannot exceed 100 characters.")]
    [Display(Name = "Area Name")]
    public string AreaName { get; set; } = null!;

    [Required(ErrorMessage = "Please select a City.")]
    [Display(Name = "City")]
    public int CityId { get; set; }

    [Display(Name = "City Name")]
    public string? CityName { get; set; }

    public City? City { get; set; }
}
