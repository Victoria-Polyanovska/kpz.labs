using System;
using System.Text;
using System.Collections.Generic;

namespace PrototypeExample
{
    class Virus
    {
        public double Weight { get; set; }
        public int Age { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public List<Virus> Children { get; set; } = new List<Virus>();

        // Конструктор
        public Virus(string name, string type, double weight, int age)
        {
            Name = name;
            Type = type;
            Weight = weight;
            Age = age;
        }

        // Метод для додавання дітей
        public void AddChild(Virus child)
        {
            Children.Add(child);
        }

        // Метод для відображення інформації
        public void ShowInfo(string indent = "")
        {
            Console.WriteLine($"{indent}Вірус: {Name}, Вид: {Type}, Вага: {Weight}, Вік: {Age}");
            foreach (var child in Children)
            {
                child.ShowInfo(indent + "   ");
            }
        }

        // Метод клонування (глибоке копіювання)
        public Virus Clone()
        {
            Virus clone = new Virus(Name, Type, Weight, Age);
            foreach (var child in Children)
            {
                clone.AddChild(child.Clone());
            }
            return clone;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.Unicode;

            // Створюємо "сімейство" вірусів
            Virus parent = new Virus("Alpha", "Retrovirus", 1.2, 5);
            Virus child1 = new Virus("Beta", "Retrovirus", 0.8, 2);
            Virus child2 = new Virus("Gamma", "Retrovirus", 0.9, 3);
            Virus grandchild = new Virus("Delta", "Retrovirus", 0.5, 1);

            child1.AddChild(grandchild);
            parent.AddChild(child1);
            parent.AddChild(child2);

            Console.WriteLine("Оригінал:");
            parent.ShowInfo();

            // Клонування
            Virus clone = parent.Clone();
            Console.WriteLine("\nКлон:");
            clone.ShowInfo();
        }
    }
}
