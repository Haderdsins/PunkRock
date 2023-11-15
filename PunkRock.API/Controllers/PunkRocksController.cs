using App.Metrics;
using Microsoft.AspNetCore.Mvc;
using PunkRock.API.Models;
using PunkRock.API.Models.Commands;
using PunkRock.API.Models.Responces;
using App.Metrics.AspNetCore;
using App.Metrics.Formatters.Prometheus;

namespace PunkRock.API.Controllers;



[ApiController]
[Route("api/[controller]")]
public class PunkRocksController : ControllerBase
{
    
    private static List<Cassette> _cassettes = new()
    {
        new Cassette
        {
            Id = 1,
            Name = "BAAMs",
            Executor = "Илья Легенда",
            Status = Status.Normal
        }
    };
    private readonly IMetrics _metrics; 
    /// <summary>
    /// Вывести кассету по ID
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id:int}")]
    public CassetteResponse Get(int id)
    {
        var cassette = _cassettes.Find(cassette => cassette.Id == id);
        _metrics.Measure.Counter.Increment(MetricsRegistry.GettedById);
        var result = new CassetteResponse
        {
            Id = cassette.Id,
            Name = cassette.Name,
            Executor = cassette.Executor,
            Status = Enum.GetName(cassette.Status)
        };
        return result;
    }

    /// <summary>
    /// Вывести все кассеты
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public IActionResult GetAll()
    {
        _metrics.Measure.Counter.Increment(MetricsRegistry.GettedByAll);
        return Ok(_cassettes.Select(cassette => new CassetteResponse
        {
            Id = cassette.Id,
            Name = cassette.Name,
            Executor = cassette.Executor,
            Status = Enum.GetName(cassette.Status)
        }));
    }

    /// <summary>
    /// Вывести кассету по Status
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{status}")]
    public IActionResult GetByStatus(string status)
    {
        _metrics.Measure.Counter.Increment(MetricsRegistry.GettedByStatus);
        if (!Enum.TryParse<Status>(status, true, out var statusEnum) || !_cassettes.Exists(c => c.Status == statusEnum))
        {
            return BadRequest("Invalid status or cassette not found");
        }

        var cassette = _cassettes.First(c => c.Status == statusEnum);

        var result = new CassetteResponse
        {
            Id = cassette.Id,
            Name = cassette.Name,
            Executor = cassette.Executor,
            Status = status // Используйте переданный статус, который уже в строковом формате
        };

        return Ok(result);
    }



    /// <summary>
    /// Создание позиции кассеты
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost]
    public ActionResult Create(CreateCassetteCommand model)
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
        return Ok();
    }
    
    /// <summary>
    /// Обновить кассету по ID
    /// </summary>
    /// <returns></returns>
    [HttpPut("{id}")]
    public ActionResult UpdateCassette(int id, [FromBody] UpdateCassetteCommand updatedCassette)
    {
        _metrics.Measure.Counter.Increment(MetricsRegistry.UpdateCassette);
        var existingCassette = _cassettes.Find(c => c.Id == id);
        if (existingCassette == null)
        {
            return NoContent(); // Если кассета с указанным id не найдена
        }

        existingCassette.Name = updatedCassette.Name;
        existingCassette.Executor = updatedCassette.Executor;
        existingCassette.Status = Enum.Parse<Status>(updatedCassette.Status);
        
        return Ok(existingCassette); // Возвращаем обновленную кассету
    }

    /// <summary>
    /// Удалить кассету по ID
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id:int}")]
    public ActionResult Delete(int id)
    {
        _metrics.Measure.Counter.Increment(MetricsRegistry.RemovedCassette);
        var cassetteToRemove = _cassettes.Find(c => c.Id == id);
        if (cassetteToRemove == null)
        {
            return NotFound(); 
        }

        _cassettes.Remove(cassetteToRemove);
        return Ok(); 
    }
}