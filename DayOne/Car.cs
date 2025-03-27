using System.Text;

public class Car
{
    public enum FuelType
    {
        Electric = 1,
        Fuel = 2
    }

    public string? Manufacturer { get; set; }
    public string? Model { get; set; }
    public int Year { get; set; }
    public FuelType Fuel { get; set; }

    public override string ToString()
    {
        var stringBuilder = new StringBuilder();
        stringBuilder.Append("Car");
        stringBuilder.Append(" { ");
        if (PrintMembers(stringBuilder))
        {
            stringBuilder.Append(' ');
        }

        stringBuilder.Append('}');
        return stringBuilder.ToString();
    }

    private bool PrintMembers(StringBuilder builder)
    {
        builder.Append("Manufacturer = ");
        builder.Append((object)Manufacturer);
        builder.Append(", Model = ");
        builder.Append((object)Model);
        builder.Append(", Year = ");
        builder.Append(Year.ToString());
        builder.Append(", Fuel = ");
        builder.Append(Fuel.ToString());
        return true;
    }
}