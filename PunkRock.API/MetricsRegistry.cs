using App.Metrics;
using App.Metrics.Counter;

namespace PunkRock.API;

public class MetricsRegistry
{
    public static CounterOptions CreatedCassette => new CounterOptions
    {
        Name = "Created Cassette",
        Context = "CassetteApi",
        MeasurementUnit = Unit.Calls,
    };
    public static CounterOptions GettedById => new CounterOptions
    {
        Name = "Getted Cassette by Id",
        Context = "CassetteApi",
        MeasurementUnit = Unit.Calls,
    };
    public static CounterOptions GettedByAll => new CounterOptions
    {
        Name = "All Cassettes",
        Context = "CassetteApi",
        MeasurementUnit = Unit.Items,
    };
    public static CounterOptions GettedByStatus => new CounterOptions
    {
        Name = "Getted Cassette by Status",
        Context = "CassetteApi",
        MeasurementUnit = Unit.Calls,
    };
    public static CounterOptions UpdateCassette => new CounterOptions
    {
        Name = "Updated Cassette",
        Context = "CassetteApi",
        MeasurementUnit = Unit.Calls,
    };
    public static CounterOptions RemovedCassette => new CounterOptions
    {
        Name = "Removed Cassette",
        Context = "CassetteApi",
        MeasurementUnit = Unit.Calls,
    };
}