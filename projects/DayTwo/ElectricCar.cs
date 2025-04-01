public class ElectricCar : Car, IChargable
{
    public void Charge(DateTime timeOfCharge)
    {
        Console.WriteLine($"ElectricCar {Make} {Model} charged on {timeOfCharge:yyyy-MM-dd HH:mm}");
    }
}