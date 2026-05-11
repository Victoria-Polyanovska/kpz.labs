using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public interface INodeState
{
    string Render(LightElementNode node);
}

public class VisibleState : INodeState
{
    public string Render(LightElementNode node)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("<" + node.TagName);

        if (node.CssClasses.Count > 0)
        {
            sb.Append(" class=\"");
            sb.Append(string.Join(" ", node.CssClasses));
            sb.Append("\"");
        }

        if (node.ClosingType == "single")
        {
            sb.Append("/>");
        }
        else
        {
            sb.Append(">");
            sb.Append(node.InnerHTML());
            sb.Append("</" + node.TagName + ">");
        }
        return sb.ToString();
    }
}

public class HiddenState : INodeState
{
    public string Render(LightElementNode node)
    {
        return $"";
    }
}

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

    public void Execute() => _node.AddClass(_cssClass);
    public void Undo() => _node.CssClasses.Remove(_cssClass);
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

    private INodeState _state;

    public LightElementNode(string tagName, string displayType = "block", string closingType = "normal")
    {
        TagName = tagName;
        DisplayType = displayType;
        ClosingType = closingType;
        CssClasses = new List<string>();
        Children = new List<LightNode>();

        _state = new VisibleState(); 
        OnCreated();
    }

    public void SetState(INodeState state)
    {
        _state = state;
        Console.WriteLine($"[State] Стан елемента <{TagName}> змінено на {state.GetType().Name}");
    }

    public void AddClass(string cssClass)
    {
        if (!CssClasses.Contains(cssClass))
        {
            CssClasses.Add(cssClass);
            OnStylesApplied();
        }
    }

    public void AddChild(LightNode child) => Children.Add(child);

    public override void OnCreated() => Console.WriteLine($"[Hook] <{TagName}> створено.");
    public override void OnStylesApplied() => Console.WriteLine($"[Hook] Стилі <{TagName}> оновлено.");

    public override string OuterHTML() => _state.Render(this);

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
    public LightNodeIterator(LightNode root) { _root = root; }
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
                    stack.Push(element.Children[i]);
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

        Console.WriteLine("=== ТЕСТУВАННЯ ШАБЛОНУ STATE ===\n");

        LightElementNode div = new LightElementNode("div");
        div.AddClass("container");
        div.AddChild(new LightTextNode("Цей текст може зникнути!"));

        Console.WriteLine("\n--- Початковий стан (Visible) ---");
        Console.WriteLine(div.OuterHTML());

        Console.WriteLine("\n--- Зміна стану на Hidden ---");
        div.SetState(new HiddenState());
        Console.WriteLine(div.OuterHTML());

        Console.WriteLine("\n--- Повернення до Visible ---");
        div.SetState(new VisibleState());
        Console.WriteLine(div.OuterHTML());
    }
}