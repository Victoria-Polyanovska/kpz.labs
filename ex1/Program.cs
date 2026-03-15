using System;
using System.Text;

namespace VideoProviderFactory
{
    // Абстрактний клас підписки
    abstract class Subscription
    {
        public double MonthlyFee { get; protected set; }
        public int MinPeriod { get; protected set; }
        public string[] Channels { get; protected set; }

        public virtual void ShowInfo()
        {
            Console.WriteLine($"Щомісячна плата: {MonthlyFee} грн");
            Console.WriteLine($"Мінімальний період: {MinPeriod} міс.");
            Console.WriteLine("Канали:");

            foreach (var channel in Channels)
            {
                Console.WriteLine("- " + channel);
            }

            Console.WriteLine();
        }
    }

    // Domestic підписка
    class DomesticSubscription : Subscription
    {
        public DomesticSubscription()
        {
            MonthlyFee = 100;
            MinPeriod = 3;
            Channels = new string[]
            {
                "Новини",
                "Розважальні",
                "Спорт"
            };
        }
    }

    // Educational підписка
    class EducationalSubscription : Subscription
    {
        public EducationalSubscription()
        {
            MonthlyFee = 80;
            MinPeriod = 6;
            Channels = new string[]
            {
                "Документальні",
                "Наукові",
                "Освітні"
            };
        }
    }

    // Premium підписка
    class PremiumSubscription : Subscription
    {
        public PremiumSubscription()
        {
            MonthlyFee = 200;
            MinPeriod = 1;
            Channels = new string[]
            {
                "Усі канали",
                "HD контент",
                "Фільми без реклами"
            };
        }
    }

    // Абстрактна фабрика
    abstract class SubscriptionCreator
    {
        public abstract Subscription CreateSubscription();
    }

    // WebSite
    class WebSite : SubscriptionCreator
    {
        public override Subscription CreateSubscription()
        {
            Console.WriteLine("Створення підписки через WebSite");
            return new DomesticSubscription();
        }
    }

    // MobileApp
    class MobileApp : SubscriptionCreator
    {
        public override Subscription CreateSubscription()
        {
            Console.WriteLine("Створення підписки через MobileApp");
            return new EducationalSubscription();
        }
    }

    // ManagerCall
    class ManagerCall : SubscriptionCreator
    {
        public override Subscription CreateSubscription()
        {
            Console.WriteLine("Створення підписки через ManagerCall");
            return new PremiumSubscription();
        }
    }

    // Головна програма
    class ex1
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.Unicode;
            Console.InputEncoding = Encoding.Unicode;

            SubscriptionCreator creator;

            creator = new WebSite();
            Subscription sub1 = creator.CreateSubscription();
            sub1.ShowInfo();

            creator = new MobileApp();
            Subscription sub2 = creator.CreateSubscription();
            sub2.ShowInfo();

            creator = new ManagerCall();
            Subscription sub3 = creator.CreateSubscription();
            sub3.ShowInfo();
        }
    }
}