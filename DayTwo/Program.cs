// See https://aka.ms/new-console-template for more information

using System.Globalization;

Console.Write("Enter car make: ");
var make = Console.ReadLine() ?? string.Empty;

Console.Write("Enter car model: ");
var model = Console.ReadLine() ?? string.Empty;

var year = CreateYearForm();
var lastMaintenanceDate = CreateMaintainanceDateForm();

Car? car = null;
while (car is null)
{
    Console.Write("Is this a FuelCar or Electric car? (F/E): ");
    var carType = Console.ReadLine()?.Trim().ToUpper();

    switch (carType)
    {
        case "F":
            car = new FuelCar { Make = make, Model = model, Year = year, LastMaintenanceDate = lastMaintenanceDate };
            break;
        case "E":
            car = new ElectricCar
            {
                Make = make, Model = model, Year = year, LastMaintenanceDate = lastMaintenanceDate
            };
            break;
        default:
            Console.WriteLine("Invalid input! Please enter 'F' for Fuel car or 'E' for Electric car.");
            break;
    }
}

car.DisplayDetails();

while (true)
{
    Console.Write("Do you want to refuel/charge? (Y/N): ");
    var actionInput = Console.ReadLine()?.Trim().ToUpper();

    if (actionInput == "N")
    {
        break;
    }

    if (actionInput == "Y")
    {
        DateTime actionTime;
        const string dateTimeFormat = "yyyy-MM-dd HH:mm";
        while (true)
        {
            Console.Write($"Enter refuel/charge date and time ({dateTimeFormat}): ");
            var dateTimeInput = Console.ReadLine();
            if (DateTime.TryParseExact(dateTimeInput, dateTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None,
                    out actionTime))
            {
                break;
            }

            Console.WriteLine($"Invalid date format! Please enter a valid date and time in {dateTimeFormat} format.");
        }

        switch (car)
        {
            case IFuelable fuelableCar:
                fuelableCar.Refuel(actionTime);
                break;
            case IChargable chargableCar:
                chargableCar.Charge(actionTime);
                break;
        }

        break;
    }

    Console.WriteLine("Invalid input. Please enter 'Y' or 'N'.");
}

return;

static int ValidateYear(int year)
{
    var currentYear = DateTime.Now.Year;
    const int firstCarYear = 1886;
    if (year < firstCarYear || year > currentYear)
    {
        throw new ArgumentOutOfRangeException(nameof(year),
            $"Year must be between {firstCarYear} and the current year.");
    }

    return year;
}

static int CreateYearForm()
{
    int year;
    while (true)
    {
        Console.Write($"Enter car year (e.g., 2020): ");
        var yearInput = Console.ReadLine();
        if (int.TryParse(yearInput, out year) && year >= 1886 && year <= DateTime.Now.Year)
        {
            break;
        }

        Console.WriteLine($"Invalid year! Please enter a valid year between 1886 and the current year.");
    }

    return year;
}

static DateTime CreateMaintainanceDateForm()
{
    DateTime lastMaintenanceDate;
    while (true)
    {
        Console.Write($"Enter last maintenance date (yyyy-MM-dd): ");
        var dateInput = Console.ReadLine();
        if (DateTime.TryParseExact(dateInput, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None,
                out lastMaintenanceDate))
        {
            break;
        }

        Console.WriteLine("Invalid date format! Please enter a valid date.");
    }

    return lastMaintenanceDate;
}