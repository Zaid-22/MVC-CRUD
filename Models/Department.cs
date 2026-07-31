namespace MvcCrudProject.Models;

public class Department
{
    public int DepartmentId { get; set; }
    public string DepartmentName { get; set; } = null!;
    public int BranchId { get; set; }

    public virtual Branch Branch { get; set; } = null!;
    public virtual ICollection<Section> Sections { get; set; } = new List<Section>();
}
