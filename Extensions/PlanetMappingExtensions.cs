using MvcCrudProject.Models;
using MvcCrudProject.ViewModels;

namespace MvcCrudProject.Extensions;

public static class PlanetMappingExtensions
{
    public static PlanetViewModel ToViewModel(this Planet planet)
    {
        if (planet == null) return null!;

        return new PlanetViewModel
        {
            PlanetId   = planet.PlanetId,
            PlanetName = planet.PlanetName,
            GalaxyId   = planet.GalaxyId,
            GalaxyName = planet.Galaxy?.GalaxyName,
            Galaxy     = planet.Galaxy
        };
    }

    public static Planet ToModel(this PlanetViewModel vm)
    {
        if (vm == null) return null!;

        return new Planet
        {
            PlanetId   = vm.PlanetId,
            PlanetName = vm.PlanetName,
            GalaxyId   = vm.GalaxyId
        };
    }

    public static List<PlanetViewModel> ToViewModelList(this IEnumerable<Planet> planets)
        => planets?.Select(p => p.ToViewModel()).ToList() ?? new List<PlanetViewModel>();

    public static List<Planet> ToModelList(this IEnumerable<PlanetViewModel> vms)
        => vms?.Select(v => v.ToModel()).ToList() ?? new List<Planet>();
}
