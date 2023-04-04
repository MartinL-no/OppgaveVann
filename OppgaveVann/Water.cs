namespace OppgaveVann;

public class Water
{
    public double Temperature { get; private set; }
    public readonly int Amount;
    public string State => GetState();
    public double? ProportionFirstState { get; private set; }
    public Water(int amount, double temperature, double? proportion = null)
    {
        if (temperature == 0 && proportion == null || temperature == 100 && proportion == null)
        {
            throw new ArgumentException("When temperature is 0 or 100, you must provide a value for proportion");
        }

        Temperature = temperature;
        Amount = amount;
        ProportionFirstState = proportion;
    }
    private string GetState()
    {
        switch (Temperature)
        {
            case double n when (n < 0):
                return WaterState.Ice;
            case double n when (n == 0 && ProportionFirstState != null):
                return WaterState.IceAndFluid;
            case double n when (n >= 0 && n < 100):
                return WaterState.Fluid;
            case double n when (n == 100 && ProportionFirstState != null):
                return WaterState.FluidAndGas;
            case double n when (n >= 100):
                return WaterState.Gas;
            default:
                break;
        }
        return null;
    }

    public void AddEnergy(double calories)
    {
        Console.WriteLine("Amount: " + Amount);
        Console.WriteLine("Temperature: " + Temperature);
        Console.WriteLine("Calories: " + calories);
        Console.WriteLine("State: " + State);
        Console.WriteLine("proportion: " + ProportionFirstState);
        Console.WriteLine();

        if (Temperature < 0 && Temperature + (calories / Amount) >= 0)
        {
            CalculateTemperatureChangeWithCorrectBreakpoint(calories, 0, 80);
        }
        else if (Temperature < 100 && Temperature + (calories / Amount) >= 100)
        {
            CalculateTemperatureChangeWithCorrectBreakpoint(calories, 100, 600);
        }
        else
        {
            Temperature += (double) calories / Amount;
        }
    }
    private void CalculateTemperatureChangeWithCorrectBreakpoint(double calories, int breakpoint, int breakpointEnergy)
    {
        var caloriesToSubtractForToReachBreakingPoint = (breakpoint - Temperature) * Amount;
        calories -= caloriesToSubtractForToReachBreakingPoint;

        if (calories > 0)
        {
            var caloriesToConvert = Amount * breakpointEnergy;
            ProportionFirstState = 1 - calories / caloriesToConvert;
            ProportionFirstState = ProportionFirstState <= 0 ? null : ProportionFirstState;
            calories = ProportionFirstState < 1 ? 0 : calories - caloriesToConvert;
        }
        Temperature = breakpoint + (calories / Amount);
    }
}