using App.Metrics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PunkRock.API.Models;
using PunkRock.API.Models.Commands;
using PunkRock.API.Models.Responces;
namespace PunkRock.API.Controllers;




[ApiController]
[Route("api/[controller]")]
public class PunkRocksController : ControllerBase
{
    private readonly IMetrics _metrics;
    private readonly ILogger<PunkRocksController> _logger;
    public PunkRocksController(IMetrics metrics, ILogger<PunkRocksController> logger)
    {
        _metrics = metrics;
        _logger = logger;
    }
    private static List<Cassette> _cassettes = new()
    {
        new Cassette
        {
            Id = 1,
            Name = "BAAMs",
            Executor = "Максим Легенда",
            Status = Status.Normal
        }
    };
    

    /// <summary>
    /// Вывести кассету по ID
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id:int}")]
    public IActionResult Get(int id)
    {
        var cassette = _cassettes.Find(c => c.Id == id);
        _metrics.Measure.Counter.Increment(MetricsRegistry.GettedById);

        if (cassette == null)
        {
            _logger.LogWarning("Cassette not found for ID: {Id}", id);
            return NotFound("Cassette not found");
        }

        var result = new CassetteResponse
        {
            Id = cassette.Id,
            Name = cassette.Name,
            Executor = cassette.Executor,
            Status = Enum.GetName(cassette.Status)
        };

        _logger.LogInformation("Cassette retrieved by ID: {Id}", id);
        return Ok(result);
    }


    /// <summary>
    /// Вывести все кассеты
    /// </summary>
    /// <returns></returns>
    [HttpGet]
public IActionResult GetAll()
{
    try
    {
        _metrics.Measure.Counter.Increment(MetricsRegistry.GettedByAll);

        var result = _cassettes.Select(cassette => new CassetteResponse
        {
            Id = cassette.Id,
            Name = cassette.Name,
            Executor = cassette.Executor,
            Status = Enum.GetName(cassette.Status)
        });

        _logger.LogInformation("All cassettes retrieved.");
        return Ok(result);
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Error retrieving all cassettes");
        return StatusCode(500, "Internal Server Error");
    }
}
/// <summary>
/// Вывести кассеты по статусу
/// </summary>
/// <param name="status"></param>
/// <returns></returns>
[HttpGet("{status}")]
public IActionResult GetByStatus(string status)
{
    try
    {
        _metrics.Measure.Counter.Increment(MetricsRegistry.GettedByStatus);

        if (!Enum.TryParse<Status>(status, true, out var statusEnum) || !_cassettes.Exists(c => c.Status == statusEnum))
        {
            _logger.LogWarning("Invalid status or cassette not found for status: {Status}", status);
            return BadRequest("Invalid status or cassette not found");
        }

        var cassette = _cassettes.First(c => c.Status == statusEnum);

        var result = new CassetteResponse
        {
            Id = cassette.Id,
            Name = cassette.Name,
            Executor = cassette.Executor,
            Status = status // Use the passed status, which is already in string format
        };

        _logger.LogInformation("Cassette retrieved by status: {Status}", status);
        return Ok(result);
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Error retrieving cassette by status: {Status}", status);
        return StatusCode(500, "Internal Server Error");
    }
}
/// <summary>
/// Создать позицию кассеты
/// </summary>
/// <param name="model"></param>
/// <returns></returns>
[HttpPost]
public ActionResult Create(CreateCassetteCommand model)
{
    try
    {
        _metrics.Measure.Counter.Increment(MetricsRegistry.CreatedCassette);

        var cassette = new Cassette
        {
            Id = model.Id,
            Name = model.Name,
            Executor = model.Executor,
            Status = Enum.Parse<Status>(model.Status)
        };

        _cassettes.Add(cassette);

        _logger.LogInformation("Cassette created with ID: {Id}", model.Id);
        return Ok();
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Error creating cassette");
        return StatusCode(500, "Internal Server Error");
    }
}
/// <summary>
/// Обновить кассету
/// </summary>
/// <param name="id"></param>
/// <param name="updatedCassette"></param>
/// <returns></returns>
[HttpPut("{id}")]
public ActionResult UpdateCassette(int id, [FromBody] UpdateCassetteCommand updatedCassette)
{
    try
    {
        _metrics.Measure.Counter.Increment(MetricsRegistry.UpdateCassette);

        var existingCassette = _cassettes.Find(c => c.Id == id);
        if (existingCassette == null)
        {
            _logger.LogInformation("Cassette not found for ID: {Id}", id);
            return NoContent(); // If cassette with the specified id is not found
        }

        existingCassette.Name = updatedCassette.Name;
        existingCassette.Executor = updatedCassette.Executor;
        existingCassette.Status = Enum.Parse<Status>(updatedCassette.Status);

        _logger.LogInformation("Cassette updated for ID: {Id}", id);
        return Ok(existingCassette); // Return the updated cassette
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Error updating cassette for ID: {Id}", id);
        return StatusCode(500, "Internal Server Error");
    }
}
/// <summary>
/// Удалить кассету
/// </summary>
/// <param name="id"></param>
/// <returns></returns>
[HttpDelete("{id:int}")]
public ActionResult Delete(int id)
{
    try
    {
        _metrics.Measure.Counter.Increment(MetricsRegistry.RemovedCassette);

        var cassetteToRemove = _cassettes.Find(c => c.Id == id);
        if (cassetteToRemove == null)
        {
            _logger.LogWarning("Cassette not found for ID: {Id}", id);
            return NotFound();
        }

        _cassettes.Remove(cassetteToRemove);

        _logger.LogInformation("Cassette removed for ID: {Id}", id);
        return Ok();
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Error deleting cassette for ID: {Id}", id);
        return StatusCode(500, "Internal Server Error");
    }
}
}