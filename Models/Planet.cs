namespace MvcCrudProject.Models;

public class Planet
{
    public int PlanetId { get; set; }
    public string PlanetName { get; set; } = null!;
    public int GalaxyId { get; set; }

    public virtual Galaxy Galaxy { get; set; } = null!;
    public virtual ICollection<Continent> Continents { get; set; } = new List<Continent>();
}
