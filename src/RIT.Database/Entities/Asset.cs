namespace RIT.Database.Entities;

public abstract class Asset
{
    public int Id { get; set; }
    public required string Name { get; set; }
}
