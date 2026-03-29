using System;
using System.Collections.Generic;
using System.Text;

// Базовий вузол
public abstract class LightNode
{
    public abstract string OuterHTML();
    public abstract string InnerHTML();
}

// Текстовий вузол
public class LightTextNode : LightNode
{
    private string text;
    public LightTextNode(string text) { this.text = text; }
    public override string OuterHTML() => text;
    public override string InnerHTML() => text;
}

// Елемент вузол
public class LightElementNode : LightNode
{
    public string TagName { get; }
    public List<LightNode> Children { get; }

    public LightElementNode(string tagName)
    {
        TagName = tagName;
        Children = new List<LightNode>();
    }

    public void AddChild(LightNode child) => Children.Add(child);

    public override string OuterHTML()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("<" + TagName + ">");
        foreach (var child in Children)
            sb.Append(child.OuterHTML());
        sb.Append("</" + TagName + ">");
        return sb.ToString();
    }

    public override string InnerHTML()
    {
        StringBuilder sb = new StringBuilder();
        foreach (var child in Children)
            sb.Append(child.OuterHTML());
        return sb.ToString();
    }
}

// Flyweight фабрика для тегів
public class FlyweightFactory
{
    private Dictionary<string, LightElementNode> elements = new Dictionary<string, LightElementNode>();

    public LightElementNode GetElement(string tagName)
    {
        if (!elements.ContainsKey(tagName))
            elements[tagName] = new LightElementNode(tagName);
        return elements[tagName];
    }

    public int UniqueTagsCount => elements.Count;
}

// Демонстрація роботи
public static class FlyweightExample
{
    public static void Run()
    {
        Console.OutputEncoding = Encoding.UTF8;

        string[] lines = {
            "Romeo and Juliet",
            "ACT V",
            "Scene I. Mantua. A Street.",
            " Scene II. Friar Lawrence’s Cell.",
            "This is a longer line of text that should be a paragraph."
        };

        FlyweightFactory factory = new FlyweightFactory();
        List<LightNode> document = new List<LightNode>();

        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i];
            string tag;

            if (i == 0)
                tag = "h1";
            else if (line.Length < 20)
                tag = "h2";
            else if (line.StartsWith(" "))
                tag = "blockquote";
            else
                tag = "p";

            // Беремо тег з фабрики (Flyweight)
            LightElementNode element = factory.GetElement(tag);

            // Створюємо новий вузол з текстом
            LightElementNode wrapper = new LightElementNode(element.TagName);
            wrapper.AddChild(new LightTextNode(line.Trim()));
            document.Add(wrapper);
        }

        Console.WriteLine("HTML верстка:");
        foreach (var node in document)
        {
            Console.WriteLine(node.OuterHTML());
        }

        Console.WriteLine($"\nВсього вузлів у дереві: {document.Count}");
        Console.WriteLine($"Унікальних тегів (Flyweight): {factory.UniqueTagsCount}");
    }
}