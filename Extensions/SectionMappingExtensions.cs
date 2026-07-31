using MvcCrudProject.Models;
using MvcCrudProject.ViewModels;

namespace MvcCrudProject.Extensions;

public static class SectionMappingExtensions
{
    public static SectionViewModel ToViewModel(this Section section)
    {
        if (section == null) return null!;

        return new SectionViewModel
        {
            SectionId      = section.SectionId,
            SectionName    = section.SectionName,
            DepartmentId   = section.DepartmentId,
            DepartmentName = section.Department?.DepartmentName,
            Department     = section.Department
        };
    }

    public static Section ToModel(this SectionViewModel vm)
    {
        if (vm == null) return null!;

        return new Section
        {
            SectionId    = vm.SectionId,
            SectionName  = vm.SectionName,
            DepartmentId = vm.DepartmentId
        };
    }

    public static List<SectionViewModel> ToViewModelList(this IEnumerable<Section> sections)
        => sections?.Select(s => s.ToViewModel()).ToList() ?? new List<SectionViewModel>();

    public static List<Section> ToModelList(this IEnumerable<SectionViewModel> vms)
        => vms?.Select(v => v.ToModel()).ToList() ?? new List<Section>();
}
