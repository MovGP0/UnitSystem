using System.Diagnostics;

namespace QsGraphics;

public abstract class Shape
{
    protected Stopwatch Timer = new Stopwatch();

    public abstract void Draw(Graphics graphics, float pixelPerMeter);

    internal void Reset()
    {
        Timer.Stop();
        Timer.Reset();
    }

    public string Name
    {
        get
        {
            return GetType().Name;
        }
    }
}