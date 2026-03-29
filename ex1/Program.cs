using System;
using System.Text;

// 1. Клас Logger
public class Logger
{
    public void Log(string message)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("[LOG]: " + message);
        Console.ResetColor();
    }

    public void Error(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("[ERROR]: " + message);
        Console.ResetColor();
    }

    public void Warn(string message)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("[WARN]: " + message);
        Console.ResetColor();
    }
}

// 2. Клас FileWriter
public class FileWriter
{
    private string filePath;

    public FileWriter(string path)
    {
        filePath = path;
    }

    public void Write(string text)
    {
        System.IO.File.AppendAllText(filePath, text);
    }

    public void WriteLine(string text)
    {
        System.IO.File.AppendAllText(filePath, text + Environment.NewLine);
    }
}

// 3. Адаптер – FileLogger
public interface ILogger
{
    void Log(string message);
    void Error(string message);
    void Warn(string message);
}

public class FileLoggerAdapter : ILogger
{
    private readonly FileWriter fileWriter;

    public FileLoggerAdapter(FileWriter writer)
    {
        fileWriter = writer;
    }

    public void Log(string message)
    {
        fileWriter.WriteLine("[LOG]: " + message);
    }

    public void Error(string message)
    {
        fileWriter.WriteLine("[ERROR]: " + message);
    }

    public void Warn(string message)
    {
        fileWriter.WriteLine("[WARN]: " + message);
    }
}

// 4. Головний метод
class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        // Використання консольного логера
        Logger consoleLogger = new Logger();
        consoleLogger.Log("Це інформаційне повідомлення");
        consoleLogger.Warn("Це попередження");
        consoleLogger.Error("Це помилка");

        // Використання файлового логера через адаптер
        FileWriter writer = new FileWriter("log.txt");
        ILogger fileLogger = new FileLoggerAdapter(writer);

        fileLogger.Log("Запис у файл: інформація");
        fileLogger.Warn("Запис у файл: попередження");
        fileLogger.Error("Запис у файл: помилка");

        Console.WriteLine("Перевір файл log.txt для результатів.");
    }
}
