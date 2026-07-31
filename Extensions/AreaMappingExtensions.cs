using MvcCrudProject.Models;
using MvcCrudProject.ViewModels;

namespace MvcCrudProject.Extensions;

public static class AreaMappingExtensions
{
    public static AreaViewModel ToViewModel(this Area area)
    {
        if (area == null) return null!;

        return new AreaViewModel
        {
            AreaId   = area.AreaId,
            AreaName = area.AreaName,
            CityId   = area.CityId,
            CityName = area.City?.CityName,
            City     = area.City
        };
    }

    public static Area ToModel(this AreaViewModel vm)
    {
        if (vm == null) return null!;

        return new Area
        {
            AreaId   = vm.AreaId,
            AreaName = vm.AreaName,
            CityId   = vm.CityId
        };
    }

    public static List<AreaViewModel> ToViewModelList(this IEnumerable<Area> areas)
        => areas?.Select(a => a.ToViewModel()).ToList() ?? new List<AreaViewModel>();

    public static List<Area> ToModelList(this IEnumerable<AreaViewModel> vms)
        => vms?.Select(v => v.ToModel()).ToList() ?? new List<Area>();
}
