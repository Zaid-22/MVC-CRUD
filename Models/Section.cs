namespace MvcCrudProject.Models;

public class Section
{
    public int SectionId { get; set; }
    public string SectionName { get; set; } = null!;
    public int DepartmentId { get; set; }

    public virtual Department Department { get; set; } = null!;
    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
