namespace MvcCrudProject.Models;

public class Area
{
    public int AreaId { get; set; }
    public string AreaName { get; set; } = null!;
    public int CityId { get; set; }

    public virtual City City { get; set; } = null!;
    public virtual ICollection<Branch> Branches { get; set; } = new List<Branch>();
}
