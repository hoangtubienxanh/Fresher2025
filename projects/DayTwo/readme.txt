Objective:
Implement an abstract class Car, create concrete classes FuelCar and ElectricCar, and use DateTime for
tracking maintenance and fueling/charging history.

Requirements:
1. Abstract Class: Car
   o Properties:
   ▪ Make (string)
   ▪ Model (string)
   ▪ Year (int, must be valid)
   ▪ LastMaintenanceDate (DateTime, must be valid)
   o Methods:
   ▪ ScheduleMaintenance() → Calculates the next maintenance date (e.g., after 6
   months).
   ▪ DisplayDetails() → Displays car details.

2. Concrete Classes:
   o FuelCar
   ▪ Implements IFuelable (Method: Refuel(DateTime timeOfRefuel))
   o ElectricCar
   ▪ Implements IChargable (Method: Charge(DateTime timeOfCharge))

3. Interfaces:
   o IFuelable → Method Refuel(DateTime timeOfRefuel)
   o IChargable → Method Charge(DateTime timeOfCharge)
4. Implementation:
   o Users must input valid Year and LastMaintenanceDate.
   o The program calculates next maintenance date from the last maintenance.
   o Allows refueling/charging with user-input date validation.

Example Screen:

Enter car make: Ford
Enter car model: Mustang

Enter car year (e.g., 2020): abc
Invalid year! Please enter a valid year between 1886 and the current year.
Enter car year (e.g., 2020): 1700
Invalid year! Please enter a valid year between 1886 and the current year.
Enter car year (e.g., 2020): 2022

Enter last maintenance date (yyyy-MM-dd): 2022-15-01
Invalid date format! Please enter a valid date.
Enter last maintenance date (yyyy-MM-dd): hello
Invalid date format! Please enter a valid date.
Enter last maintenance date (yyyy-MM-dd): 2022-06-10

Is this a FuelCar or Electriccar? (F/E): G
Invalid input! Please enter 'F' for Fuelcar or 'E' for Electriccar.
Is this a FuelCar or Electriccar? (F/E): fuel
Invalid input! Please enter 'F' for Fuelcar or 'E' for Electriccar.
Is this a Fuelcar or Electriccar? (F/E): F

Car: Ford Mustang (2022)
Last Maintenance: 2022-06-10
Next Maintenance: 2022-12-10

Do you want to refuel/charge? (Y/N): Y

Enter refuel/charge date and time (yyyy-MM-dd HH:mm): today
Invalid date format! Please enter a valid date.
Enter refuel/charge date and time (yyyy-MM-dd HH:mm): 2024-03-20 14:30
FuelCar Ford Mustang refueled on 2024-03-20 14:30
