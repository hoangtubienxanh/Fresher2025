public class ElectricCar(string make, string model, int year, DateTime lastMaintenanceDate)
    : Car(make, model, year, lastMaintenanceDate), IChargable
{
    private List<DateTime> _chargeHistory = new();
    public IReadOnlyList<DateTime> ChargeHistory => _chargeHistory.AsReadOnly();

    public void Charge(DateTime timeOfCharge)
    {
        _chargeHistory.Add(timeOfCharge);
        Console.WriteLine($"ElectricCar {Make} {Model} charged on {timeOfCharge:yyyy-MM-dd HH:mm}");
    }
}