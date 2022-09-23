using ConsoleGameEngine;
using ConsoleGameEngine.DataStructures;
using ConsoleGameEngine.Entities;
using ConsoleGameEngine.Window;

namespace DrawDeltaT
{
    public class Sample : Engine
    {
        public Layer layer;

        public Sample() : base(new ConsoleWindow())
        {
            layer = new Layer(new Vec2i(), new Vec2i(window.width, window.height));
            layer.Clear(new Chexel(' ', new Vec3(1, 0, 1), new Vec3(0, 0, 0)));

            window.AddLayer(layer);

            AddEntity(0, new FPSEntity(new Vec2i()));
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Sample sample = new Sample();

            sample.Start();

            while (sample.isRunning)
            {
                Thread.Sleep(100);
            }
        }
    }
}