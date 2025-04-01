public class FuelCar : Car, IFuelable
{
    public void Refuel(DateTime timeOfRefuel)
    {
        Console.WriteLine($"FuelCar {Make} {Model} refueled on {timeOfRefuel:yyyy-MM-dd HH:mm}");
    }
}