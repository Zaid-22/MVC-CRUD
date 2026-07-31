using System.ComponentModel.DataAnnotations;
using MvcCrudProject.Models;

namespace MvcCrudProject.ViewModels;

public class CityViewModel
{
    public int CityId { get; set; }

    [Required(ErrorMessage = "City name is required.")]
    [StringLength(100, ErrorMessage = "City name cannot exceed 100 characters.")]
    [Display(Name = "City Name")]
    public string CityName { get; set; } = null!;

    [Required(ErrorMessage = "Please select a Country.")]
    [Display(Name = "Country")]
    public int CountryId { get; set; }

    [Display(Name = "Country Name")]
    public string? CountryName { get; set; }

    public Country? Country { get; set; }
}
