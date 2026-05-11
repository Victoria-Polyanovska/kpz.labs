using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public interface ICommand
{
    void Execute();
    void Undo();
}

public class AddClassCommand : ICommand
{
    private readonly LightElementNode _node;
    private readonly string _cssClass;

    public AddClassCommand(LightElementNode node, string cssClass)
    {
        _node = node;
        _cssClass = cssClass;
    }

    public void Execute()
    {
        if (!_node.CssClasses.Contains(_cssClass))
        {
            _node.AddClass(_cssClass);
        }
    }

    public void Undo()
    {
        if (_node.CssClasses.Contains(_cssClass))
        {
            _node.CssClasses.Remove(_cssClass);
            Console.WriteLine($"[Command] Скасовано: Клас '{_cssClass}' видалено з <{_node.TagName}>");
        }
    }
}

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
    public LightTextNode(string text) { this.text = text; }
    public override string OuterHTML() => text;
    public override string InnerHTML() => text;
}

public class LightElementNode : LightNode
{
    public string TagName { get; }
    public string DisplayType { get; }
    public string ClosingType { get; }
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

    public void AddChild(LightNode child) => Children.Add(child);

    public override void OnCreated() => Console.WriteLine($"[Hook] Елемент <{TagName}> створено.");
    public override void OnStylesApplied() => Console.WriteLine($"[Hook] Стилі для <{TagName}> оновлено.");

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

        Console.WriteLine("=== ТЕСТУВАННЯ ШАБЛОНІВ (КРОК 3: COMMAND) ===\n");

        LightElementNode ul = new LightElementNode("ul");
        LightElementNode li = new LightElementNode("li");
        li.AddChild(new LightTextNode("Текст всередині команди"));
        ul.AddChild(li);

        var command = new AddClassCommand(ul, "highlighted-list");

        Console.WriteLine("\n--- Виконання команди (Add Class) ---");
        command.Execute();
        Console.WriteLine(ul.OuterHTML());

        Console.WriteLine("\n--- Скасування команди (Undo) ---");
        command.Undo();
        Console.WriteLine(ul.OuterHTML());

        Console.WriteLine("\n--- Перевірка Ітератора по всьому дереву ---");
        foreach (var node in ul.Enumerate())
        {
            string name = node is LightElementNode el ? el.TagName : "TextNode";
            Console.WriteLine($"Вузол у дереві: {name}");
        }
    }
}