using MvcCrudProject.Models;
using MvcCrudProject.ViewModels;

namespace MvcCrudProject.Extensions;

public static class CountryMappingExtensions
{
    public static CountryViewModel ToViewModel(this Country country)
    {
        if (country == null) return null!;

        return new CountryViewModel
        {
            CountryId   = country.CountryId,
            CountryName = country.CountryName,
            RegionId    = country.RegionId,
            RegionName  = country.Region?.RegionName,
            Region      = country.Region
        };
    }

    public static Country ToModel(this CountryViewModel vm)
    {
        if (vm == null) return null!;

        return new Country
        {
            CountryId   = vm.CountryId,
            CountryName = vm.CountryName,
            RegionId    = vm.RegionId
        };
    }

    public static List<CountryViewModel> ToViewModelList(this IEnumerable<Country> countries)
        => countries?.Select(c => c.ToViewModel()).ToList() ?? new List<CountryViewModel>();

    public static List<Country> ToModelList(this IEnumerable<CountryViewModel> vms)
        => vms?.Select(v => v.ToModel()).ToList() ?? new List<Country>();
}
