using System;
using System.Collections.Generic;
using System.Text;

// 1. Базовий клас
public abstract class LightNode
{
    public abstract string OuterHTML();
    public abstract string InnerHTML();

    public virtual void OnCreated() { }
    public virtual void OnStylesApplied() { }
}

// 2. Текстовий вузол
public class LightTextNode : LightNode
{
    private string text;

    public LightTextNode(string text)
    {
        this.text = text;
    }

    public override string OuterHTML() => text;
    public override string InnerHTML() => text;
}

// 3. Елемент вузол
public class LightElementNode : LightNode
{
    public string TagName { get; }
    public string DisplayType { get; } // block / inline
    public string ClosingType { get; } // single / normal
    public List<string> CssClasses { get; }
    public List<LightNode> Children { get; }

    public LightElementNode(string tagName, string displayType = "block", string closingType = "normal")
    {
        TagName = tagName;
        DisplayType = displayType;
        ClosingType = closingType;
        CssClasses = new List<string>();
        Children = new List<LightNode>();
    }

    public void AddClass(string cssClass)
    {
        CssClasses.Add(cssClass);
    }

    public void AddChild(LightNode child)
    {
        Children.Add(child);
    }

    public override string OuterHTML()
    {
        OnCreated();

        StringBuilder sb = new StringBuilder();
        sb.Append("<" + TagName);

        if (CssClasses.Count > 0)
        {
            sb.Append(" class=\"");
            sb.Append(string.Join(" ", CssClasses));
            sb.Append("\"");

            OnStylesApplied();
        }

        if (ClosingType == "single")
        {
            sb.Append("/>");
        }
        else
        {
            sb.Append(">");
            sb.Append(InnerHTML());
            sb.Append("</" + TagName + ">");
        }

        return sb.ToString();
    }
    public override void OnCreated() => Console.WriteLine($"[Hook] Елемент <{TagName}> було створено.");
    public override void OnStylesApplied() => Console.WriteLine($"[Hook] Стилі для <{TagName}> успішно застосовано.");

    public override string InnerHTML()
    {
        StringBuilder sb = new StringBuilder();
        foreach (var child in Children)
        {
            sb.Append(child.OuterHTML());
        }
        return sb.ToString();
    }
}

// 4. Головний метод
class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        // Створимо список <ul> з елементами <li>
        LightElementNode ul = new LightElementNode("ul", "block", "normal");
        ul.AddClass("my-list");

        LightElementNode li1 = new LightElementNode("li");
        li1.AddChild(new LightTextNode("Перший елемент"));

        LightElementNode li2 = new LightElementNode("li");
        li2.AddChild(new LightTextNode("Другий елемент"));

        LightElementNode li3 = new LightElementNode("li");
        li3.AddChild(new LightTextNode("Третій елемент"));

        ul.AddChild(li1);
        ul.AddChild(li2);
        ul.AddChild(li3);

        Console.WriteLine("OuterHTML:");
        Console.WriteLine(ul.OuterHTML());

        Console.WriteLine("\nInnerHTML:");
        Console.WriteLine(ul.InnerHTML());
    }
}
