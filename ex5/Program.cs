using System;
using System.Collections.Generic;
using System.Text;

// Клас Memento — зберігає стан документа
class DocumentMemento
{
    public string State { get; private set; }

    public DocumentMemento(string state)
    {
        State = state;
    }
}

// Клас TextDocument — сам документ
class TextDocument
{
    public string Content { get; set; }

    public TextDocument(string content = "")
    {
        Content = content;
    }

    public DocumentMemento Save()
    {
        return new DocumentMemento(Content);
    }

    public void Restore(DocumentMemento memento)
    {
        Content = memento.State;
    }
}

// Клас TextEditor — працює з документом і історією змін
class TextEditor
{
    private TextDocument document;
    private Stack<DocumentMemento> history = new Stack<DocumentMemento>();

    public TextEditor(TextDocument doc)
    {
        document = doc;
    }

    public void Write(string text)
    {
        history.Push(document.Save()); // зберігаємо попередній стан
        document.Content += text;
    }

    public void Undo()
    {
        if (history.Count > 0)
        {
            var memento = history.Pop();
            document.Restore(memento);
        }
        else
        {
            Console.WriteLine("Немає попередніх станів для відновлення.");
        }
    }

    public void Show()
    {
        Console.WriteLine($" Поточний текст: \"{document.Content}\"");
    }
}

class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.Unicode;
        Console.InputEncoding = Encoding.Unicode;

        TextDocument doc = new TextDocument();
        TextEditor editor = new TextEditor(doc);

        editor.Write("Привіт, світ!");
        editor.Show();

        editor.Write(" Додаємо новий рядок.");
        editor.Show();

        Console.WriteLine("Виконуємо Undo...");
        editor.Undo();
        editor.Show();

        Console.WriteLine("Виконуємо Undo ще раз...");
        editor.Undo();
        editor.Show();
    }
}

