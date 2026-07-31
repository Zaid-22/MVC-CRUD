namespace MvcCrudProject.Models;

public class Country
{
    public int CountryId { get; set; }
    public string CountryName { get; set; } = null!;
    public int RegionId { get; set; }

    public virtual Region Region { get; set; } = null!;
    public virtual ICollection<City> Cities { get; set; } = new List<City>();
}
