// See https://aka.ms/new-console-template for more information
using System.Drawing;
using Console = Colorful.Console;

List<IDrawable> list = new()
{
    new MyPoint(),
    new MyCircle(new Point(7,8),5,null)
};

foreach (var item in list)
{
    item.Draw();
}

Console.SetCursorPosition(0, 20);
Console.WriteLine((list[0] as MyPoint)!.CalcDistance(new MyPoint(new Point(4, 4), Color.White)));
Console.WriteLine((list[1] as MyCircle)!.CalcDistance(new MyPoint(new Point(4, 4), Color.White)));

public interface IDrawable
{
    public void Draw();
}

public class MyPoint : IDrawable
{
    public Point p;
    public Color color;
    public MyPoint(Point p, Color? color)
    {
        this.p = p;
        this.color = color ?? Color.Red;
    }
    public MyPoint()
    {
        p = new Point(1, 1);
        color = Color.Red;
    }

    public double CalcDistance(MyPoint mp)
    {

        return Math.Sqrt(Math.Pow(Math.Abs(p.X - mp.p.X), 2) + Math.Pow(Math.Abs(p.Y - mp.p.Y), 2));
    }

    public void Draw()
    {
        Console.SetCursorPosition(p.X * 2, p.Y);
        Console.Write("<>", color);
    }
}

public enum LocStatus
{
    Outside,
    OnBorder,
    Inside
}

public class MyCircle : IDrawable
{
    public Point p;
    public Color color;
    public int radius;
    public MyCircle(Point p, int radius, Color? color)
    {
        this.p = p;
        this.color = color ?? Color.Blue;
        this.radius = radius;
    }
    public MyCircle()
    {
        p = new Point(1, 1);
        color = Color.Blue;
    }

    public (double, LocStatus) CalcDistance(MyPoint mp)
    {
        double distance = Math.Sqrt(Math.Pow(Math.Abs(p.X - mp.p.X), 2) + Math.Pow(Math.Abs(p.Y - mp.p.Y), 2));
        return (distance, distance < radius ? LocStatus.Inside : distance > radius ? LocStatus.Outside : LocStatus.OnBorder);
    }

    public void Draw()
    {
        for (int x = p.X - radius; x <= p.X + radius; x++)
        {
            for (int y = p.Y - radius; y <= p.Y + radius; y++)
            {
                if ((x- p.X) * (x - p.X) + (y - p.Y) * (y - p.Y) <= radius * radius) {
                    Console.SetCursorPosition(x*2, y);
                    Console.Write("##", color);
                }
            }
        }
    }
}