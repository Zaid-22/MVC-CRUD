namespace MvcCrudProject.Models;

public class Branch
{
    public int BranchId { get; set; }
    public string BranchName { get; set; } = null!;
    public int AreaId { get; set; }
    public int CompanyId { get; set; }

    public virtual Area Area { get; set; } = null!;
    public virtual Company Company { get; set; } = null!;
    public virtual ICollection<Department> Departments { get; set; } = new List<Department>();
}
