namespace PunkRock.API.Models.Commands;

public class UpdateCassetteCommand
{
    public required string Name { get; set; }

    public required string Executor { get; set; }

    public required string Status { get; set; }
}