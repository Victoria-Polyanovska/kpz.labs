using System;
using System.Text;

// Інтерфейс Посередника
interface ICommandCentre
{
    void RequestLanding(Aircraft aircraft);
    void RunwayAvailable(Runway runway);
    void RegisterRunway(Runway runway);
}

// Конкретний Посередник
class CommandCentre : ICommandCentre
{
    private Runway runway;

    public void RegisterRunway(Runway runway)
    {
        this.runway = runway;
    }

    public void RequestLanding(Aircraft aircraft)
    {
        if (runway != null && runway.IsFree)
        {
            Console.WriteLine($"Літак {aircraft.Name} отримав дозвіл на посадку.");
            runway.IsFree = false;
        }
        else
        {
            Console.WriteLine($"Літак {aircraft.Name} чекає, смуга зайнята.");
        }
    }

    public void RunwayAvailable(Runway runway)
    {
        Console.WriteLine("Злітна смуга звільнилася.");
        runway.IsFree = true;
    }
}

// Клас Літак
class Aircraft
{
    public string Name { get; private set; }
    private ICommandCentre commandCentre;

    public Aircraft(string name, ICommandCentre commandCentre)
    {
        Name = name;
        this.commandCentre = commandCentre;
    }

    public void RequestLanding()
    {
        commandCentre.RequestLanding(this);
    }
}

// Клас Злітна смуга
class Runway
{
    public bool IsFree { get; set; } = true;
    private ICommandCentre commandCentre;

    public Runway(ICommandCentre commandCentre)
    {
        this.commandCentre = commandCentre;
        commandCentre.RegisterRunway(this);
    }

    public void FreeRunway()
    {
        commandCentre.RunwayAvailable(this);
    }
}

class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.Unicode;
        Console.InputEncoding = Encoding.Unicode;

        CommandCentre centre = new CommandCentre();

        Runway runway = new Runway(centre);

        Aircraft aircraft1 = new Aircraft("Boeing-737", centre);
        Aircraft aircraft2 = new Aircraft("Airbus-A320", centre);

        // Сценарій роботи
        aircraft1.RequestLanding(); // Boeing сідає
        aircraft2.RequestLanding(); // Airbus чекає
        runway.FreeRunway();        // Смуга звільняється
        aircraft2.RequestLanding(); // Airbus сідає
    }
}
