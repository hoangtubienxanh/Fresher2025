public abstract class Car(string make, string model, int year, DateTime lastMaintenanceDate)
{
    public string Make { get; init; } = make;
    public string Model { get; init; } = model;
    public int Year { get; init; } = ValidateYear(year);
    public DateTime LastMaintenanceDate { get; init; } = ValidateDate(lastMaintenanceDate, nameof(LastMaintenanceDate));

    public DateTime ScheduleMaintenance()
    {
        return LastMaintenanceDate.AddMonths(6);
    }

    public virtual void DisplayDetails()
    {
        Console.WriteLine($"Car: {Make} {Model} ({Year})");
        Console.WriteLine($"Last Maintenance: {LastMaintenanceDate:yyyy-MM-dd}");
        Console.WriteLine($"Next Maintenance: {ScheduleMaintenance():yyyy-MM-dd}");
    }

    private static int ValidateYear(int year)
    {
        int currentYear = DateTime.Now.Year;
        const int firstCarYear = 1886;
        if (year < firstCarYear || year > currentYear)
        {
            throw new ArgumentOutOfRangeException(nameof(year), $"Year must be between {firstCarYear} and {currentYear}.");
        }
        return year;
    }

    private static DateTime ValidateDate(DateTime date, string paramName)
    {
        return date;
    }
}