namespace MvcCrudProject.Models;

public class Region
{
    public int RegionId { get; set; }
    public string RegionName { get; set; } = null!;
    public int ContinentId { get; set; }

    public virtual Continent Continent { get; set; } = null!;
    public virtual ICollection<Country> Countries { get; set; } = new List<Country>();
}
