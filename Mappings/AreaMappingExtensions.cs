using MvcCrudProject.Models;
using MvcCrudProject.ViewModels;

namespace MvcCrudProject.Mappings;

public static class AreaMappingExtensions
{
    public static AreaViewModel ToViewModel(this Area area)
    {
        if (area == null) return null!;

        return new AreaViewModel
        {
            AreaId = area.AreaId,
            AreaName = area.AreaName,
            CityId = area.CityId,
            CityName = area.City?.CityName,
            City = area.City
        };
    }

    public static Area ToEntity(this AreaViewModel vm)
    {
        if (vm == null) return null!;

        return new Area
        {
            AreaId = vm.AreaId,
            AreaName = vm.AreaName,
            CityId = vm.CityId
        };
    }

    public static void UpdateEntity(this AreaViewModel vm, Area area)
    {
        if (vm == null || area == null) return;

        area.AreaName = vm.AreaName;
        area.CityId = vm.CityId;
    }
}
