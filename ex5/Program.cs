using System;
using System.Text;
using System.Collections.Generic;

namespace BuilderExample
{
    // Клас персонажа
    class Character
    {
        public string Name { get; set; }
        public string Height { get; set; }
        public string BodyType { get; set; }
        public string HairColor { get; set; }
        public string EyeColor { get; set; }
        public string Clothes { get; set; }
        public List<string> Inventory { get; set; } = new List<string>();
        public List<string> Deeds { get; set; } = new List<string>();

        public void ShowInfo()
        {
            Console.WriteLine($"Ім'я: {Name}");
            Console.WriteLine($"Зріст: {Height}, Статура: {BodyType}");
            Console.WriteLine($"Волосся: {HairColor}, Очі: {EyeColor}");
            Console.WriteLine($"Одяг: {Clothes}");
            Console.WriteLine("Інвентар: " + string.Join(", ", Inventory));
            Console.WriteLine("Справи: " + string.Join(", ", Deeds));
        }
    }

    // Інтерфейс будівельника
    interface ICharacterBuilder
    {
        ICharacterBuilder SetName(string name);
        ICharacterBuilder SetHeight(string height);
        ICharacterBuilder SetBodyType(string bodyType);
        ICharacterBuilder SetHairColor(string hairColor);
        ICharacterBuilder SetEyeColor(string eyeColor);
        ICharacterBuilder SetClothes(string clothes);
        ICharacterBuilder AddItem(string item);
        ICharacterBuilder AddDeed(string deed);
        Character Build();
    }

    // Будівельник героя
    class HeroBuilder : ICharacterBuilder
    {
        private Character hero = new Character();

        public ICharacterBuilder SetName(string name) { hero.Name = name; return this; }
        public ICharacterBuilder SetHeight(string height) { hero.Height = height; return this; }
        public ICharacterBuilder SetBodyType(string bodyType) { hero.BodyType = bodyType; return this; }
        public ICharacterBuilder SetHairColor(string hairColor) { hero.HairColor = hairColor; return this; }
        public ICharacterBuilder SetEyeColor(string eyeColor) { hero.EyeColor = eyeColor; return this; }
        public ICharacterBuilder SetClothes(string clothes) { hero.Clothes = clothes; return this; }
        public ICharacterBuilder AddItem(string item) { hero.Inventory.Add(item); return this; }
        public ICharacterBuilder AddDeed(string deed) { hero.Deeds.Add("Добра справа: " + deed); return this; }
        public Character Build() => hero;
    }

    // Будівельник ворога
    class EnemyBuilder : ICharacterBuilder
    {
        private Character enemy = new Character();

        public ICharacterBuilder SetName(string name) { enemy.Name = name; return this; }
        public ICharacterBuilder SetHeight(string height) { enemy.Height = height; return this; }
        public ICharacterBuilder SetBodyType(string bodyType) { enemy.BodyType = bodyType; return this; }
        public ICharacterBuilder SetHairColor(string hairColor) { enemy.HairColor = hairColor; return this; }
        public ICharacterBuilder SetEyeColor(string eyeColor) { enemy.EyeColor = eyeColor; return this; }
        public ICharacterBuilder SetClothes(string clothes) { enemy.Clothes = clothes; return this; }
        public ICharacterBuilder AddItem(string item) { enemy.Inventory.Add(item); return this; }
        public ICharacterBuilder AddDeed(string deed) { enemy.Deeds.Add("Зла справа: " + deed); return this; }
        public Character Build() => enemy;
    }

    // Директор
    class Director
    {
        public Character ConstructHero(ICharacterBuilder builder)
        {
            return builder
                .SetName("Артур")
                .SetHeight("Високий")
                .SetBodyType("Атлетичний")
                .SetHairColor("Блондин")
                .SetEyeColor("Блакитні")
                .SetClothes("Лицарські обладунки")
                .AddItem("Меч")
                .AddItem("Щит")
                .AddDeed("Врятував село")
                .AddDeed("Переміг дракона")
                .Build();
        }

        public Character ConstructEnemy(ICharacterBuilder builder)
        {
            return builder
                .SetName("Темний Лорд")
                .SetHeight("Середній")
                .SetBodyType("Худорлявий")
                .SetHairColor("Чорне")
                .SetEyeColor("Червоні")
                .SetClothes("Темна мантія")
                .AddItem("Чарівний посох")
                .AddDeed("Спалив місто")
                .AddDeed("Зрадив союзників")
                .Build();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.Unicode;

            Director director = new Director();

            Character hero = director.ConstructHero(new HeroBuilder());
            Character enemy = director.ConstructEnemy(new EnemyBuilder());

            Console.WriteLine("Герой:");
            hero.ShowInfo();

            Console.WriteLine("\nВорог:");
            enemy.ShowInfo();
        }
    }
}