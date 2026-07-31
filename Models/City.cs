namespace MvcCrudProject.Models;

public class City
{
    public int CityId { get; set; }
    public string CityName { get; set; } = null!;
    public int CountryId { get; set; }

    public virtual Country Country { get; set; } = null!;
    public virtual ICollection<Area> Areas { get; set; } = new List<Area>();
}
