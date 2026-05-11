using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public abstract class LightNode
{
    public abstract string OuterHTML();
    public abstract string InnerHTML();

    public virtual void OnCreated() { }
    public virtual void OnStylesApplied() { }

    public IEnumerable<LightNode> Enumerate() => new LightNodeIterator(this);
}

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

        OnCreated();
    }

    public void AddClass(string cssClass)
    {
        CssClasses.Add(cssClass);
        OnStylesApplied();
    }

    public void AddChild(LightNode child)
    {
        Children.Add(child);
    }

    public override void OnCreated() => Console.WriteLine($"[Hook] Елемент <{TagName}> було створено.");
    public override void OnStylesApplied() => Console.WriteLine($"[Hook] Стилі для <{TagName}> успішно оновлено.");

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

public class LightNodeIterator : IEnumerable<LightNode>
{
    private readonly LightNode _root;

    public LightNodeIterator(LightNode root)
    {
        _root = root;
    }

    public IEnumerator<LightNode> GetEnumerator()
    {
        Stack<LightNode> stack = new Stack<LightNode>();
        stack.Push(_root);

        while (stack.Count > 0)
        {
            var current = stack.Pop();
            yield return current;

            if (current is LightElementNode element)
            {
                for (int i = element.Children.Count - 1; i >= 0; i--)
                {
                    stack.Push(element.Children[i]);
                }
            }
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        Console.WriteLine("=== Створення HTML структури ===\n");

        LightElementNode ul = new LightElementNode("ul", "block", "normal");
        ul.AddClass("my-list");

        LightElementNode li1 = new LightElementNode("li");
        li1.AddChild(new LightTextNode("Перший елемент"));

        LightElementNode li2 = new LightElementNode("li");
        li2.AddChild(new LightTextNode("Другий елемент"));

        ul.AddChild(li1);
        ul.AddChild(li2);

        Console.WriteLine("\n=== Результуючий HTML (OuterHTML) ===");
        Console.WriteLine(ul.OuterHTML());

        Console.WriteLine("\n=== Обхід дерева через Ітератор ===");
        foreach (var node in ul.Enumerate())
        {
            if (node is LightElementNode el)
                Console.WriteLine($"[Node] Елемент: <{el.TagName}>");
            else
                Console.WriteLine($"[Text] Вміст: \"{node.OuterHTML()}\"");
        }
    }
}