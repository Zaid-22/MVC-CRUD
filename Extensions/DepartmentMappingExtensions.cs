using MvcCrudProject.Models;
using MvcCrudProject.ViewModels;

namespace MvcCrudProject.Extensions;

public static class DepartmentMappingExtensions
{
    public static DepartmentViewModel ToViewModel(this Department department)
    {
        if (department == null) return null!;

        return new DepartmentViewModel
        {
            DepartmentId   = department.DepartmentId,
            DepartmentName = department.DepartmentName,
            BranchId       = department.BranchId,
            BranchName     = department.Branch?.BranchName,
            Branch         = department.Branch
        };
    }

    public static Department ToModel(this DepartmentViewModel vm)
    {
        if (vm == null) return null!;

        return new Department
        {
            DepartmentId   = vm.DepartmentId,
            DepartmentName = vm.DepartmentName,
            BranchId       = vm.BranchId
        };
    }

    public static List<DepartmentViewModel> ToViewModelList(this IEnumerable<Department> departments)
        => departments?.Select(d => d.ToViewModel()).ToList() ?? new List<DepartmentViewModel>();

    public static List<Department> ToModelList(this IEnumerable<DepartmentViewModel> vms)
        => vms?.Select(v => v.ToModel()).ToList() ?? new List<Department>();
}
