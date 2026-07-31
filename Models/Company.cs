namespace MvcCrudProject.Models;

public class Company
{
    public int CompanyId { get; set; }
    public string CompanyName { get; set; } = null!;

    public virtual ICollection<Branch> Branches { get; set; } = new List<Branch>();
}
