using System;
using System.Text;

// Інтерфейс стратегії
interface IImageLoaderStrategy
{
    void Load(string href);
}

// Стратегія завантаження з файлової системи
class FileSystemImageLoader : IImageLoaderStrategy
{
    public void Load(string href)
    {
        Console.WriteLine($" Завантаження з файлової системи: {href}");
        // Тут могла б бути логіка читання файлу
    }
}

// Стратегія завантаження з мережі
class NetworkImageLoader : IImageLoaderStrategy
{
    public void Load(string href)
    {
        Console.WriteLine($"Завантаження з мережі: {href}");
        // Тут могла б бути логіка HTTP-запиту
    }
}

// Базовий HTML елемент
class LightElement
{
    public string TagName { get; private set; }

    public LightElement(string tagName)
    {
        TagName = tagName;
    }

    public virtual void Render()
    {
        Console.WriteLine($"< {TagName} >");
    }
}

// Новий елемент Image з підтримкою стратегії
class Image : LightElement
{
    private string href;
    private IImageLoaderStrategy loaderStrategy;

    public Image(string href) : base("img")
    {
        this.href = href;

        // Вибір стратегії залежно від href
        if (href.StartsWith("http"))
        {
            loaderStrategy = new NetworkImageLoader();
        }
        else
        {
            loaderStrategy = new FileSystemImageLoader();
        }
    }

    public override void Render()
    {
        Console.WriteLine($"<img src='{href}'>");
        loaderStrategy.Load(href);
    }
}

class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.Unicode;
        Console.InputEncoding = Encoding.Unicode;

        // Завантаження з файлової системи
        Image localImage = new Image("C:/images/photo.png");
        localImage.Render();

        // Завантаження з мережі
        Image webImage = new Image("http://example.com/photo.png");
        webImage.Render();
    }
}
