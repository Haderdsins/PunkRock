namespace PunkRock.API.Models;

public class Cassette
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public required string Executor { get; set; }

    public required Status Status { get; set; }
}

public enum Status
{
    Old,
    New,
    Normal
}