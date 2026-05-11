using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public interface IVisitor
{
    void VisitElement(LightElementNode element);
    void VisitText(LightTextNode textNode);
}

public class StatisticsVisitor : IVisitor
{
    public int ElementsCount { get; private set; } = 0;
    public int TextLength { get; private set; } = 0;

    public void VisitElement(LightElementNode element)
    {
        ElementsCount++;
        Console.WriteLine($"[Visitor] Аналіз тега: <{element.TagName}>");
    }

    public void VisitText(LightTextNode textNode)
    {
        TextLength += textNode.TextContent.Length;
        Console.WriteLine($"[Visitor] Аналіз тексту довжиною: {textNode.TextContent.Length}");
    }
}

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
            sb.Append(" class=\"" + string.Join(" ", node.CssClasses) + "\"");

        if (node.ClosingType == "single") sb.Append("/>");
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
    public string Render(LightElementNode node) => $"";
}

public interface ICommand
{
    void Execute();
    void Undo();
}

public class AddClassCommand : ICommand
{
    private readonly LightElementNode _node;
    private readonly string _class;
    public AddClassCommand(LightElementNode n, string c) { _node = n; _class = c; }
    public void Execute() => _node.AddClass(_class);
    public void Undo() => _node.CssClasses.Remove(_class);
}

public abstract class LightNode
{
    public abstract string OuterHTML();
    public abstract string InnerHTML();
    public virtual void OnCreated() { }
    public virtual void OnStylesApplied() { }

    public IEnumerable<LightNode> Enumerate() => new LightNodeIterator(this);
    public abstract void Accept(IVisitor visitor);
}

public class LightTextNode : LightNode
{
    public string TextContent { get; }
    public LightTextNode(string text) => TextContent = text;
    public override string OuterHTML() => TextContent;
    public override string InnerHTML() => TextContent;
    public override void Accept(IVisitor visitor) => visitor.VisitText(this);
}

public class LightElementNode : LightNode
{
    public string TagName { get; }
    public string ClosingType { get; }
    public List<string> CssClasses { get; } = new List<string>();
    public List<LightNode> Children { get; } = new List<LightNode>();
    private INodeState _state = new VisibleState();

    public LightElementNode(string tag, string closing = "normal")
    {
        TagName = tag;
        ClosingType = closing;
        OnCreated();
    }

    public void SetState(INodeState s) => _state = s;
    public void AddClass(string c) { if (!CssClasses.Contains(c)) { CssClasses.Add(c); OnStylesApplied(); } }
    public void AddChild(LightNode c) => Children.Add(c);

    public override void OnCreated() => Console.WriteLine($"[Hook] <{TagName}> створено.");
    public override void OnStylesApplied() => Console.WriteLine($"[Hook] Стилі <{TagName}> оновлено.");

    public override string OuterHTML() => _state.Render(this);
    public override string InnerHTML()
    {
        StringBuilder sb = new StringBuilder();
        foreach (var child in Children) sb.Append(child.OuterHTML());
        return sb.ToString();
    }

    public override void Accept(IVisitor visitor)
    {
        visitor.VisitElement(this);
        foreach (var child in Children) child.Accept(visitor);
    }
}

public class LightNodeIterator : IEnumerable<LightNode>
{
    private readonly LightNode _root;
    public LightNodeIterator(LightNode r) => _root = r;
    public IEnumerator<LightNode> GetEnumerator()
    {
        Stack<LightNode> stack = new Stack<LightNode>();
        stack.Push(_root);
        while (stack.Count > 0)
        {
            var cur = stack.Pop(); yield return cur;
            if (cur is LightElementNode el)
                for (int i = el.Children.Count - 1; i >= 0; i--) stack.Push(el.Children[i]);
        }
    }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.WriteLine("=== ТЕСТУВАННЯ УСІХ ШАБЛОНІВ (КРОК 5: VISITOR) ===\n");

        var body = new LightElementNode("body");
        var h1 = new LightElementNode("h1");
        h1.AddChild(new LightTextNode("Вітаю у моєму HTML!"));
        body.AddChild(h1);
        body.AddChild(new LightTextNode("Текст під заголовком."));

        var stats = new StatisticsVisitor();
        body.Accept(stats);

        Console.WriteLine("\n--- РЕЗУЛЬТАТИ ВІДВІДУВАЧА ---");
        Console.WriteLine($"Всього тегів: {stats.ElementsCount}");
        Console.WriteLine($"Загальна довжина тексту: {stats.TextLength} симв.");

        Console.WriteLine("\n--- Перевірка State (Hidden) ---");
        h1.SetState(new HiddenState());
        Console.WriteLine(body.OuterHTML());
    }
}