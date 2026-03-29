using System;
using System.IO;
using System.Text.RegularExpressions;

// 1. SmartTextReader
public class SmartTextReader
{
    public char[][] ReadFile(string path)
    {
        string[] lines = File.ReadAllLines(path);
        char[][] result = new char[lines.Length][];

        for (int i = 0; i < lines.Length; i++)
        {
            result[i] = lines[i].ToCharArray();
        }

        return result;
    }
}

// 2. Проксі з логуванням
public class SmartTextChecker
{
    private SmartTextReader reader;

    public SmartTextChecker(SmartTextReader reader)
    {
        this.reader = reader;
    }

    public char[][] ReadFile(string path)
    {
        Console.WriteLine($"Opening file: {path}");
        char[][] content = reader.ReadFile(path);
        Console.WriteLine("File successfully read.");

        int totalLines = content.Length;
        int totalChars = 0;
        foreach (var line in content)
        {
            totalChars += line.Length;
        }

        Console.WriteLine($"Total lines: {totalLines}, Total characters: {totalChars}");
        Console.WriteLine("Closing file.");

        return content;
    }
}

// 3. Проксі з обмеженням доступу
public class SmartTextReaderLocker
{
    private SmartTextReader reader;
    private Regex regex;

    public SmartTextReaderLocker(SmartTextReader reader, string pattern)
    {
        this.reader = reader;
        regex = new Regex(pattern, RegexOptions.IgnoreCase);
    }

    public char[][] ReadFile(string path)
    {
        if (regex.IsMatch(path))
        {
            Console.WriteLine("Access denied!");
            return null;
        }
        return reader.ReadFile(path);
    }
}

// 4. Головний метод
class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        SmartTextReader baseReader = new SmartTextReader();

        // Використання SmartTextChecker
        SmartTextChecker checker = new SmartTextChecker(baseReader);
        checker.ReadFile("example.txt"); // файл має існувати

        // Використання SmartTextReaderLocker
        SmartTextReaderLocker locker = new SmartTextReaderLocker(baseReader, @"secret.*\.txt");
        locker.ReadFile("secret_data.txt"); // буде Access denied!
        locker.ReadFile("public_data.txt"); // буде прочитано
    }
}
