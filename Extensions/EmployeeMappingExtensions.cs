using MvcCrudProject.Models;
using MvcCrudProject.ViewModels;

namespace MvcCrudProject.Extensions;

public static class EmployeeMappingExtensions
{
    public static EmployeeViewModel ToViewModel(this Employee employee)
    {
        if (employee == null) return null!;

        return new EmployeeViewModel
        {
            EmployeeId   = employee.EmployeeId,
            EmployeeName = employee.EmployeeName,
            SectionId    = employee.SectionId,
            SectionName  = employee.Section?.SectionName,
            Section      = employee.Section
        };
    }

    public static Employee ToModel(this EmployeeViewModel vm)
    {
        if (vm == null) return null!;

        return new Employee
        {
            EmployeeId   = vm.EmployeeId,
            EmployeeName = vm.EmployeeName,
            SectionId    = vm.SectionId
        };
    }

    public static List<EmployeeViewModel> ToViewModelList(this IEnumerable<Employee> employees)
        => employees?.Select(e => e.ToViewModel()).ToList() ?? new List<EmployeeViewModel>();

    public static List<Employee> ToModelList(this IEnumerable<EmployeeViewModel> vms)
        => vms?.Select(v => v.ToModel()).ToList() ?? new List<Employee>();
}
