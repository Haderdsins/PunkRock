namespace PunkRock.API.Models.Commands;

public class CreateCassetteCommand
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public required string Executor { get; set; }

    public required string Status { get; set; }
}