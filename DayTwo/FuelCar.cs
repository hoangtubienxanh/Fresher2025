public class FuelCar(string make, string model, int year, DateTime lastMaintenanceDate)
    : Car(make, model, year, lastMaintenanceDate), IFuelable
{
    private List<DateTime> _refuelHistory = new();
    public IReadOnlyList<DateTime> RefuelHistory => _refuelHistory.AsReadOnly();

    public void Refuel(DateTime timeOfRefuel)
    {
        _refuelHistory.Add(timeOfRefuel);
        Console.WriteLine($"FuelCar {Make} {Model} refueled on {timeOfRefuel:yyyy-MM-dd HH:mm}");
    }
}