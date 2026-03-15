using System;
using System.Text;

namespace AbstractFactoryExample
{
    // Абстрактні продукти
    abstract class Laptop { public abstract void ShowInfo(); }
    abstract class Netbook { public abstract void ShowInfo(); }
    abstract class EBook { public abstract void ShowInfo(); }
    abstract class Smartphone { public abstract void ShowInfo(); }

    // Конкретні продукти для бренду IProne
    class IProneLaptop : Laptop { public override void ShowInfo() => Console.WriteLine("IProne Laptop"); }
    class IProneNetbook : Netbook { public override void ShowInfo() => Console.WriteLine("IProne Netbook"); }
    class IProneEBook : EBook { public override void ShowInfo() => Console.WriteLine("IProne EBook"); }
    class IProneSmartphone : Smartphone { public override void ShowInfo() => Console.WriteLine("IProne Smartphone"); }

    // Конкретні продукти для бренду Kiaomi
    class KiaomiLaptop : Laptop { public override void ShowInfo() => Console.WriteLine("Kiaomi Laptop"); }
    class KiaomiNetbook : Netbook { public override void ShowInfo() => Console.WriteLine("Kiaomi Netbook"); }
    class KiaomiEBook : EBook { public override void ShowInfo() => Console.WriteLine("Kiaomi EBook"); }
    class KiaomiSmartphone : Smartphone { public override void ShowInfo() => Console.WriteLine("Kiaomi Smartphone"); }

    // Конкретні продукти для бренду Balaxy
    class BalaxyLaptop : Laptop { public override void ShowInfo() => Console.WriteLine("Balaxy Laptop"); }
    class BalaxyNetbook : Netbook { public override void ShowInfo() => Console.WriteLine("Balaxy Netbook"); }
    class BalaxyEBook : EBook { public override void ShowInfo() => Console.WriteLine("Balaxy EBook"); }
    class BalaxySmartphone : Smartphone { public override void ShowInfo() => Console.WriteLine("Balaxy Smartphone"); }

    // Абстрактна фабрика
    interface ITechFactory
    {
        Laptop CreateLaptop();
        Netbook CreateNetbook();
        EBook CreateEBook();
        Smartphone CreateSmartphone();
    }

    // Конкретні фабрики
    class IProneFactory : ITechFactory
    {
        public Laptop CreateLaptop() => new IProneLaptop();
        public Netbook CreateNetbook() => new IProneNetbook();
        public EBook CreateEBook() => new IProneEBook();
        public Smartphone CreateSmartphone() => new IProneSmartphone();
    }

    class KiaomiFactory : ITechFactory
    {
        public Laptop CreateLaptop() => new KiaomiLaptop();
        public Netbook CreateNetbook() => new KiaomiNetbook();
        public EBook CreateEBook() => new KiaomiEBook();
        public Smartphone CreateSmartphone() => new KiaomiSmartphone();
    }

    class BalaxyFactory : ITechFactory
    {
        public Laptop CreateLaptop() => new BalaxyLaptop();
        public Netbook CreateNetbook() => new BalaxyNetbook();
        public EBook CreateEBook() => new BalaxyEBook();
        public Smartphone CreateSmartphone() => new BalaxySmartphone();
    }

    // Клієнтський код
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.Unicode;
            Console.InputEncoding = Encoding.Unicode;

            ITechFactory factory;

            factory = new IProneFactory();
            factory.CreateLaptop().ShowInfo();
            factory.CreateSmartphone().ShowInfo();

            factory = new KiaomiFactory();
            factory.CreateNetbook().ShowInfo();
            factory.CreateEBook().ShowInfo();

            factory = new BalaxyFactory();
            factory.CreateLaptop().ShowInfo();
            factory.CreateSmartphone().ShowInfo();
        }
    }
}
