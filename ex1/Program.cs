using System;
using System.Text;

// Абстрактний Handler
abstract class SupportHandler
{
    protected SupportHandler nextHandler;

    public void SetNextHandler(SupportHandler nextHandler)
    {
        this.nextHandler = nextHandler;
    }

    public abstract bool HandleRequest(int choice);
}

// Рівень 1: Загальні питання
class GeneralSupport : SupportHandler
{
    public override bool HandleRequest(int choice)
    {
        if (choice == 1)
        {
            Console.WriteLine("Ви підключені до загальної підтримки.");
            return true;
        }
        else if (nextHandler != null)
        {
            return nextHandler.HandleRequest(choice);
        }
        return false;
    }
}

// Рівень 2: Технічна підтримка
class TechnicalSupport : SupportHandler
{
    public override bool HandleRequest(int choice)
    {
        if (choice == 2)
        {
            Console.WriteLine("Ви підключені до технічної підтримки.");
            return true;
        }
        else if (nextHandler != null)
        {
            return nextHandler.HandleRequest(choice);
        }
        return false;
    }
}

// Рівень 3: Фінансові питання
class BillingSupport : SupportHandler
{
    public override bool HandleRequest(int choice)
    {
        if (choice == 3)
        {
            Console.WriteLine("Ви підключені до фінансової підтримки.");
            return true;
        }
        else if (nextHandler != null)
        {
            return nextHandler.HandleRequest(choice);
        }
        return false;
    }
}

// Рівень 4: Адміністративні питання
class AdminSupport : SupportHandler
{
    public override bool HandleRequest(int choice)
    {
        if (choice == 4)
        {
            Console.WriteLine("Ви підключені до адміністративної підтримки.");
            return true;
        }
        else if (nextHandler != null)
        {
            return nextHandler.HandleRequest(choice);
        }
        return false;
    }
}

class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.Unicode;
        Console.InputEncoding = Encoding.Unicode;
       
        SupportHandler general = new GeneralSupport();
        SupportHandler technical = new TechnicalSupport();
        SupportHandler billing = new BillingSupport();
        SupportHandler admin = new AdminSupport();

        general.SetNextHandler(technical);
        technical.SetNextHandler(billing);
        billing.SetNextHandler(admin);

        while (true)
        {
            Console.WriteLine("\nЛаскаво просимо до системи підтримки!");
            Console.WriteLine("Оберіть потрібний розділ:");
            Console.WriteLine("1 - Загальні питання");
            Console.WriteLine("2 - Технічна підтримка");
            Console.WriteLine("3 - Фінансові питання");
            Console.WriteLine("4 - Адміністративні питання");
            Console.WriteLine("0 - Вихід");

            int choice;
            if (!int.TryParse(Console.ReadLine(), out choice))
            {
                Console.WriteLine("Некоректний ввід. Спробуйте ще раз.");
                continue;
            }

            if (choice == 0)
            {
                Console.WriteLine("Дякуємо за звернення! Гарного дня!");
                break;
            }

            bool handled = general.HandleRequest(choice);

            if (!handled)
            {
                Console.WriteLine("Ваш запит не знайдено. Спробуйте ще раз.");
            }
        }
    }
}
