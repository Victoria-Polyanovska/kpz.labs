using System;
using System.Collections.Generic;
using System.Text;

// Інтерфейс спостерігача
interface IEventListener
{
    void HandleEvent(string eventType, LightElement element);
}

// Базовий HTML елемент
class LightElement
{
    public string TagName { get; private set; }
    private Dictionary<string, List<IEventListener>> listeners = new Dictionary<string, List<IEventListener>>();

    public LightElement(string tagName)
    {
        TagName = tagName;
    }

    // Додаємо слухача на певний тип події
    public void AddEventListener(string eventType, IEventListener listener)
    {
        if (!listeners.ContainsKey(eventType))
        {
            listeners[eventType] = new List<IEventListener>();
        }
        listeners[eventType].Add(listener);
    }

    // Викликаємо подію
    public void TriggerEvent(string eventType)
    {
        Console.WriteLine($"Подія \"{eventType}\" викликана на елементі <{TagName}>");
        if (listeners.ContainsKey(eventType))
        {
            foreach (var listener in listeners[eventType])
            {
                listener.HandleEvent(eventType, this);
            }
        }
    }
}

// Конкретний слухач
class ClickListener : IEventListener
{
    public void HandleEvent(string eventType, LightElement element)
    {
        Console.WriteLine($"Обробка події {eventType} на елементі <{element.TagName}>");
    }
}

class MouseOverListener : IEventListener
{
    public void HandleEvent(string eventType, LightElement element)
    {
        Console.WriteLine($"Наведення миші на елемент <{element.TagName}>");
    }
}

class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.Unicode;
        Console.InputEncoding = Encoding.Unicode;

        // Створюємо HTML елемент
        LightElement button = new LightElement("button");

        // Додаємо слухачів
        button.AddEventListener("click", new ClickListener());
        button.AddEventListener("mouseover", new MouseOverListener());

        // Викликаємо події
        button.TriggerEvent("click");
        button.TriggerEvent("mouseover");
    }
}
