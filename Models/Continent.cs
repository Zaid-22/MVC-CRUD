namespace MvcCrudProject.Models;

public class Continent
{
    public int ContinentId { get; set; }
    public string ContinentName { get; set; } = null!;
    public int PlanetId { get; set; }

    public virtual Planet Planet { get; set; } = null!;
    public virtual ICollection<Region> Regions { get; set; } = new List<Region>();
}
