// See https://aka.ms/new-console-template for more information

List<Car> cars = [];

while (!ApplicationHost.Cancelled)
{
    Console.WriteLine("""
                      Menu:
                      1. Add a Car
                      2. View All Cars
                      3. Search Car by Make
                      4. Filter Cars by Type
                      5. Remove a Car by Model
                      6. Exit
                      Enter your choice:
                      """);
    Console.Write("> ");
    var success = int.TryParse(Console.ReadLine(), out var command);
    if (success && command is <= 6 and >= 1)
    {
        bool validatedState = true;
        switch (command)
        {
            case 1:
                validatedState = CreateCarForm(out var car);
                if (validatedState)
                {
                    cars.Add(new Car
                    {
                        Manufacturer = car.Manufacturer, Model = car.Model, Fuel = car.Fuel, Year = car.Year
                    });
                }

                break;
            case 2:
                Console.WriteLine(string.Join(Environment.NewLine, cars));

                break;
            case 3:
                Console.WriteLine("Enter Make:");
                Console.Write("> ");
                var manufacturer = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(manufacturer))
                {
                    validatedState = false;
                }
                else
                {
                    Console.WriteLine(string.Join(Environment.NewLine,
                        cars.Where(t => string.Equals(t.Manufacturer, manufacturer))));
                }

                break;
            case 4:
                Console.WriteLine("Enter car type (Fuel/Electric):");
                Console.Write("> ");
                if (!Enum.TryParse(typeof(Car.FuelType), Console.ReadLine(), true, out var fuel))
                {
                    validatedState = false;
                }
                else
                {
                    Console.WriteLine(string.Join(Environment.NewLine,
                        cars.Where(t => Equals(t.Manufacturer, fuel))));
                }

                break;
            case 5:
                Console.WriteLine("Enter Model:");
                Console.Write("> ");
                var model = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(model))
                {
                    validatedState = false;
                }
                else
                {
                    var toBeRemoved = cars.FirstOrDefault(t => string.Equals(t.Model, model));
                    if (toBeRemoved is not null)
                    {
                        cars.Remove(toBeRemoved);
                    }
                    else
                    {
                        Console.WriteLine("No car with the specified Model to be removed");
                    }
                }

                break;
        }

        if (!validatedState)
        {
            Console.WriteLine("Form validation failed....");
        }
    }
    else
    {
        Console.WriteLine("Invalid Command ^!*?");
    }
}

return;

static bool CreateCarForm(out Car car)
{
    car = new Car();
    Console.WriteLine("Enter car type (Fuel/Electric):");
    Console.Write("> ");
    if (!Enum.TryParse(typeof(Car.FuelType), Console.ReadLine(), true, out var fuel))
    {
        return false;
    }

    car.Fuel = (Car.FuelType)fuel;

    Console.WriteLine("Enter Make:");
    Console.Write("> ");
    var manufacturer = Console.ReadLine();
    if (string.IsNullOrWhiteSpace(manufacturer))
    {
        return false;
    }

    car.Manufacturer = manufacturer;

    Console.WriteLine("Enter Model:");
    Console.Write("> ");
    var model = Console.ReadLine();
    if (string.IsNullOrWhiteSpace(model))
    {
        return false;
    }

    car.Model = model;

    Console.WriteLine("Enter Year:");
    Console.Write("> ");
    if (!int.TryParse(Console.ReadLine(), out var year))
    {
        return false;
    }

    car.Year = year;
    return true;
}