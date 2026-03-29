using System;

// 1. Базовий інтерфейс героя
public interface IHero
{
    string GetDescription();
    int GetPower();
}

// Конкретні герої
public class Warrior : IHero
{
    public string GetDescription() => "Warrior";
    public int GetPower() => 10;
}

public class Mage : IHero
{
    public string GetDescription() => "Mage";
    public int GetPower() => 8;
}

public class Paladin : IHero
{
    public string GetDescription() => "Paladin";
    public int GetPower() => 12;
}

// 2. Базовий декоратор
public abstract class HeroDecorator : IHero
{
    protected IHero hero;

    public HeroDecorator(IHero hero)
    {
        this.hero = hero;
    }

    public virtual string GetDescription() => hero.GetDescription();
    public virtual int GetPower() => hero.GetPower();
}

// 3. Конкретні декоратори (інвентар)
public class Sword : HeroDecorator
{
    public Sword(IHero hero) : base(hero) { }

    public override string GetDescription() => hero.GetDescription() + " with Sword";
    public override int GetPower() => hero.GetPower() + 5;
}

public class Armor : HeroDecorator
{
    public Armor(IHero hero) : base(hero) { }

    public override string GetDescription() => hero.GetDescription() + " with Armor";
    public override int GetPower() => hero.GetPower() + 3;
}

public class Artifact : HeroDecorator
{
    public Artifact(IHero hero) : base(hero) { }

    public override string GetDescription() => hero.GetDescription() + " with Artifact";
    public override int GetPower() => hero.GetPower() + 7;
}

// 4. Головний метод
class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        IHero warrior = new Warrior();
        Console.WriteLine($"{warrior.GetDescription()} має силу {warrior.GetPower()}");

        // Додаємо інвентар
        IHero warriorWithSword = new Sword(warrior);
        IHero warriorWithSwordAndArmor = new Armor(warriorWithSword);
        IHero warriorFull = new Artifact(warriorWithSwordAndArmor);

        Console.WriteLine($"{warriorFull.GetDescription()} має силу {warriorFull.GetPower()}");

        IHero mage = new Mage();
        IHero mageWithArtifact = new Artifact(new Sword(mage));
        Console.WriteLine($"{mageWithArtifact.GetDescription()} має силу {mageWithArtifact.GetPower()}");

        IHero paladin = new Paladin();
        IHero paladinFull = new Armor(new Sword(new Artifact(paladin)));
        Console.WriteLine($"{paladinFull.GetDescription()} має силу {paladinFull.GetPower()}");
    }
}

