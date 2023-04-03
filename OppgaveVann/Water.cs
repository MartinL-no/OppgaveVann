namespace OppgaveVann;

public class Water
{
    public int Temperature { get; private set; }
    public int Amount { get; private set; }
    public string State => Temperature < 0 ? "Ice" : Temperature < 100 ? "Fluid" : "Gas";
    public Water(int amount, int temperature)
    {
        Temperature = temperature;
        Amount = amount;
    }

    
}