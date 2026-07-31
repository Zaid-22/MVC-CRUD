using MvcCrudProject.Models;
using MvcCrudProject.ViewModels;

namespace MvcCrudProject.Extensions;

public static class GalaxyMappingExtensions
{
    public static GalaxyViewModel ToViewModel(this Galaxy galaxy)
    {
        if (galaxy == null) return null!;

        return new GalaxyViewModel
        {
            GalaxyId   = galaxy.GalaxyId,
            GalaxyName = galaxy.GalaxyName
        };
    }

    public static Galaxy ToModel(this GalaxyViewModel vm)
    {
        if (vm == null) return null!;

        return new Galaxy
        {
            GalaxyId   = vm.GalaxyId,
            GalaxyName = vm.GalaxyName
        };
    }

    public static List<GalaxyViewModel> ToViewModelList(this IEnumerable<Galaxy> galaxies)
        => galaxies?.Select(g => g.ToViewModel()).ToList() ?? new List<GalaxyViewModel>();

    public static List<Galaxy> ToModelList(this IEnumerable<GalaxyViewModel> vms)
        => vms?.Select(v => v.ToModel()).ToList() ?? new List<Galaxy>();
}
