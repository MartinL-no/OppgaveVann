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
            case double n when (n < 0 || n == 0 && ProportionFirstState == 0):
                return WaterState.Ice;
            case double n when (n == 0 && ProportionFirstState < 1):
                return WaterState.IceAndFluid;
            case double n when (n == 0 && ProportionFirstState >= 1):
                return WaterState.Fluid;
            case double n when (n > 0 && n < 100):
                return WaterState.Fluid;
            case double n when (n == 100 && ProportionFirstState == 0):
                return WaterState.Fluid;
            case double n when (n == 100 && ProportionFirstState > 0 && ProportionFirstState < 1):
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
        var temperatureWithCaloriesAdded = Temperature + (calories / Amount);

        if (Temperature < 0 && temperatureWithCaloriesAdded >= 0)
        {
            CalculateTemperatureAndProportionWithCorrectBreakpoint(calories, 0, 80);
        }
        else if (Temperature < 100 && temperatureWithCaloriesAdded >= 100)
        {
            CalculateTemperatureAndProportionWithCorrectBreakpoint(calories, 100, 600);
        }
        else
        {
            Temperature = temperatureWithCaloriesAdded;
        }
 
    }
    private void CalculateTemperatureAndProportionWithCorrectBreakpoint(double calories, int breakpoint, int breakpointEnergy)
    {
        var caloriesToSubtractToReachBreakpoint = (breakpoint - Temperature) * Amount;
        calories -= caloriesToSubtractToReachBreakpoint;

        if (calories > 0)
        {
            var caloriesToConvert = Amount * breakpointEnergy;
            ProportionFirstState = calories / caloriesToConvert;
            ProportionFirstState = ProportionFirstState >= 1 ? 1 : 1 - ProportionFirstState;

            calories = ProportionFirstState < 1 ? 0 : calories - caloriesToConvert;
            Temperature = breakpoint + (calories / Amount);
        }
        else
        {
            Temperature += calories / Amount;
        }  
    }
}