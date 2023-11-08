namespace PunkRock.API.Models.Responces;

public class CassetteResponse
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public required string Executor { get; set; }

    public required string Status { get; set; }
}