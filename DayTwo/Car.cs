public abstract class Car
{
    public string Make { get; set; }
    public string Model { get; set; }
    public int Year { get; set; }
    public DateTime LastMaintenanceDate { get; set; }

    public DateTime ScheduleMaintenance()
    {
        return LastMaintenanceDate.AddMonths(6);
    }

    public void DisplayDetails()
    {
        Console.WriteLine($"Car: {Make} {Model} ({Year})");
        Console.WriteLine($"Last Maintenance: {LastMaintenanceDate:yyyy-MM-dd}");
        Console.WriteLine($"Next Maintenance: {ScheduleMaintenance():yyyy-MM-dd}");
    }
}