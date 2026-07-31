namespace MvcCrudProject.Models;

public class Galaxy
{
    public int GalaxyId { get; set; }
    public string GalaxyName { get; set; } = null!;

    public virtual ICollection<Planet> Planets { get; set; } = new List<Planet>();
}
