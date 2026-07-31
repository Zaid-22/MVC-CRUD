using System.ComponentModel.DataAnnotations;

namespace MvcCrudProject.ViewModels;

public class GalaxyViewModel
{
    public int GalaxyId { get; set; }

    [Required(ErrorMessage = "Galaxy name is required.")]
    [StringLength(100, ErrorMessage = "Galaxy name cannot exceed 100 characters.")]
    [Display(Name = "Galaxy Name")]
    public string GalaxyName { get; set; } = null!;
}
