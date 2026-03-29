using System;

// 1. Інтерфейс рендерингу
public interface IRenderer
{
    void Render(string shapeName);
}

// 2. Конкретні реалізації рендерингу
public class VectorRenderer : IRenderer
{
    public void Render(string shapeName)
    {
        Console.WriteLine($"Drawing {shapeName} as vectors");
    }
}

public class RasterRenderer : IRenderer
{
    public void Render(string shapeName)
    {
        Console.WriteLine($"Drawing {shapeName} as pixels");
    }
}

// 3. Абстракція Shape
public abstract class Shape
{
    protected IRenderer renderer;

    protected Shape(IRenderer renderer)
    {
        this.renderer = renderer;
    }

    public abstract void Draw();
}

// 4. Конкретні фігури
public class Circle : Shape
{
    public Circle(IRenderer renderer) : base(renderer) { }

    public override void Draw()
    {
        renderer.Render("Circle");
    }
}

public class Square : Shape
{
    public Square(IRenderer renderer) : base(renderer) { }

    public override void Draw()
    {
        renderer.Render("Square");
    }
}

public class Triangle : Shape
{
    public Triangle(IRenderer renderer) : base(renderer) { }

    public override void Draw()
    {
        renderer.Render("Triangle");
    }
}

// 5. Головний метод
class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        IRenderer vector = new VectorRenderer();
        IRenderer raster = new RasterRenderer();

        Shape circle = new Circle(vector);
        Shape square = new Square(raster);
        Shape triangle = new Triangle(vector);

        circle.Draw();    // Drawing Circle as vectors
        square.Draw();    // Drawing Square as pixels
        triangle.Draw();  // Drawing Triangle as vectors

        // Можна легко змінити рендерер
        Shape triangleRaster = new Triangle(raster);
        triangleRaster.Draw(); // Drawing Triangle as pixels
    }
}
