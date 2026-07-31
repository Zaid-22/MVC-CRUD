using MvcCrudProject.Models;
using MvcCrudProject.ViewModels;

namespace MvcCrudProject.Extensions;

public static class CompanyMappingExtensions
{
    public static CompanyViewModel ToViewModel(this Company company)
    {
        if (company == null) return null!;

        return new CompanyViewModel
        {
            CompanyId   = company.CompanyId,
            CompanyName = company.CompanyName
        };
    }

    public static Company ToModel(this CompanyViewModel vm)
    {
        if (vm == null) return null!;

        return new Company
        {
            CompanyId   = vm.CompanyId,
            CompanyName = vm.CompanyName
        };
    }

    public static List<CompanyViewModel> ToViewModelList(this IEnumerable<Company> companies)
        => companies?.Select(c => c.ToViewModel()).ToList() ?? new List<CompanyViewModel>();

    public static List<Company> ToModelList(this IEnumerable<CompanyViewModel> vms)
        => vms?.Select(v => v.ToModel()).ToList() ?? new List<Company>();
}
