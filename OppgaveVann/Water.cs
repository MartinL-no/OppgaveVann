namespace OppgaveVann;

public class Water
{
    public double Temperature { get; private set; }
    public readonly int Amount;
    public string State => GetState();
    public double ProportionFirstState { get; private set; }
    public Water(int amount, double temperature, double proportionFirstState = 0)
    {
        if (temperature == 0 && proportionFirstState == 0 || temperature == 100 && proportionFirstState == 0)
        {
            throw new ArgumentException("When temperature is 0 or 100, you must provide a value for proportion");
        }

        Temperature = temperature;
        Amount = amount;
        ProportionFirstState = proportionFirstState;
    }
    private string GetState()
    {
        switch (Temperature)
        {
            case double n when (n < 0):
                return WaterState.Ice;
            case double n when (n == 0 && ProportionFirstState != 0):
                return WaterState.IceAndFluid;
            case double n when (n >= 0 && n < 100):
                return WaterState.Fluid;
            case double n when (n == 100 && ProportionFirstState <= 0):
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
        if (Temperature < 0 && Temperature + (calories / Amount) >= 0)
        {
            CalculateTemperatureChangeWithCorrectBreakpoint(calories, 0, 80);
            Temperature += (double) calories / Amount;
        }
        else if (Temperature < 100 && Temperature + (calories / Amount) >= 100)
        {
            CalculateTemperatureChangeWithCorrectBreakpoint(calories, 100, 600);
        }
    }

    private void CalculateTemperatureChangeWithCorrectBreakpoint(double calories, int breakpoint, int breakpointEnergy)
    {
        var caloriesToSubtractForHeating = (breakpoint - Temperature) * Amount;
        calories -= caloriesToSubtractForHeating;

        var caloriesToConvert = Amount * breakpointEnergy;
        ProportionFirstState = 1 - calories / caloriesToConvert;

        calories = calories - caloriesToConvert < breakpoint ? breakpoint : (calories - caloriesToConvert);
        
        Temperature = breakpoint + (calories / Amount);
    }
}