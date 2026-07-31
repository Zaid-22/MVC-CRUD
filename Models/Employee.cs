namespace MvcCrudProject.Models;

public class Employee
{
    public int EmployeeId { get; set; }
    public string EmployeeName { get; set; } = null!;
    public int SectionId { get; set; }

    public virtual Section Section { get; set; } = null!;
}
