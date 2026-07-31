using MvcCrudProject.Models;
using MvcCrudProject.ViewModels;

namespace MvcCrudProject.Extensions;

public static class RegionMappingExtensions
{
    public static RegionViewModel ToViewModel(this Region region)
    {
        if (region == null) return null!;

        return new RegionViewModel
        {
            RegionId      = region.RegionId,
            RegionName    = region.RegionName,
            ContinentId   = region.ContinentId,
            ContinentName = region.Continent?.ContinentName,
            Continent     = region.Continent
        };
    }

    public static Region ToModel(this RegionViewModel vm)
    {
        if (vm == null) return null!;

        return new Region
        {
            RegionId    = vm.RegionId,
            RegionName  = vm.RegionName,
            ContinentId = vm.ContinentId
        };
    }

    public static List<RegionViewModel> ToViewModelList(this IEnumerable<Region> regions)
        => regions?.Select(r => r.ToViewModel()).ToList() ?? new List<RegionViewModel>();

    public static List<Region> ToModelList(this IEnumerable<RegionViewModel> vms)
        => vms?.Select(v => v.ToModel()).ToList() ?? new List<Region>();
}
