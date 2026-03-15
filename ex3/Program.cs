using System;
using System.Text;

namespace SingletonExample
{
    // Клас-одинак
    sealed class Authenticator
    {
        // Приватний статичний екземпляр
        private static readonly Lazy<Authenticator> instance =
            new Lazy<Authenticator>(() => new Authenticator());

        // Приватний конструктор
        private Authenticator()
        {
            Console.WriteLine("Створено екземпляр Authenticator");
        }

        // Публічний доступ до єдиного екземпляра
        public static Authenticator Instance => instance.Value;

        // Метод для перевірки роботи
        public void Authenticate(string user)
        {
            Console.WriteLine($"Користувач {user} автентифікований.");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.Unicode;

            // Отримуємо екземпляр кілька разів
            var auth1 = Authenticator.Instance;
            auth1.Authenticate("Іван");

            var auth2 = Authenticator.Instance;
            auth2.Authenticate("Марія");

            // Перевірка: обидва посилання вказують на один і той самий об’єкт
            Console.WriteLine(Object.ReferenceEquals(auth1, auth2)
                ? "Це один і той самий екземпляр!"
                : "Це різні екземпляри!");
        }
    }
}
