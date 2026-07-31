using MvcCrudProject.Models;
using MvcCrudProject.ViewModels;

namespace MvcCrudProject.Extensions;

public static class ContinentMappingExtensions
{
    public static ContinentViewModel ToViewModel(this Continent continent)
    {
        if (continent == null) return null!;

        return new ContinentViewModel
        {
            ContinentId   = continent.ContinentId,
            ContinentName = continent.ContinentName,
            PlanetId      = continent.PlanetId,
            PlanetName    = continent.Planet?.PlanetName,
            Planet        = continent.Planet
        };
    }

    public static Continent ToModel(this ContinentViewModel vm)
    {
        if (vm == null) return null!;

        return new Continent
        {
            ContinentId   = vm.ContinentId,
            ContinentName = vm.ContinentName,
            PlanetId      = vm.PlanetId
        };
    }

    public static List<ContinentViewModel> ToViewModelList(this IEnumerable<Continent> continents)
        => continents?.Select(c => c.ToViewModel()).ToList() ?? new List<ContinentViewModel>();

    public static List<Continent> ToModelList(this IEnumerable<ContinentViewModel> vms)
        => vms?.Select(v => v.ToModel()).ToList() ?? new List<Continent>();
}
