using MvcCrudProject.Models;
using MvcCrudProject.ViewModels;

namespace MvcCrudProject.Extensions;

public static class CityMappingExtensions
{
    public static CityViewModel ToViewModel(this City city)
    {
        if (city == null) return null!;

        return new CityViewModel
        {
            CityId      = city.CityId,
            CityName    = city.CityName,
            CountryId   = city.CountryId,
            CountryName = city.Country?.CountryName,
            Country     = city.Country
        };
    }

    public static City ToModel(this CityViewModel vm)
    {
        if (vm == null) return null!;

        return new City
        {
            CityId    = vm.CityId,
            CityName  = vm.CityName,
            CountryId = vm.CountryId
        };
    }

    public static List<CityViewModel> ToViewModelList(this IEnumerable<City> cities)
        => cities?.Select(c => c.ToViewModel()).ToList() ?? new List<CityViewModel>();

    public static List<City> ToModelList(this IEnumerable<CityViewModel> vms)
        => vms?.Select(v => v.ToModel()).ToList() ?? new List<City>();
}
