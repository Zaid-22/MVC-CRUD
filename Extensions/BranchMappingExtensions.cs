using MvcCrudProject.Models;
using MvcCrudProject.ViewModels;

namespace MvcCrudProject.Extensions;

public static class BranchMappingExtensions
{
    public static BranchViewModel ToViewModel(this Branch branch)
    {
        if (branch == null) return null!;

        return new BranchViewModel
        {
            BranchId    = branch.BranchId,
            BranchName  = branch.BranchName,
            AreaId      = branch.AreaId,
            AreaName    = branch.Area?.AreaName,
            CompanyId   = branch.CompanyId,
            CompanyName = branch.Company?.CompanyName,
            Area        = branch.Area,
            Company     = branch.Company
        };
    }

    public static Branch ToModel(this BranchViewModel vm)
    {
        if (vm == null) return null!;

        return new Branch
        {
            BranchId   = vm.BranchId,
            BranchName = vm.BranchName,
            AreaId     = vm.AreaId,
            CompanyId  = vm.CompanyId
        };
    }

    public static List<BranchViewModel> ToViewModelList(this IEnumerable<Branch> branches)
        => branches?.Select(b => b.ToViewModel()).ToList() ?? new List<BranchViewModel>();

    public static List<Branch> ToModelList(this IEnumerable<BranchViewModel> vms)
        => vms?.Select(v => v.ToModel()).ToList() ?? new List<Branch>();
}
