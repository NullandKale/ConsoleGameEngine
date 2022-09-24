using ConsoleGameEngine;
using ConsoleGameEngine.DataStructures;
using ConsoleGameEngine.Entities;
using ConsoleGameEngine.Layers;
using ConsoleGameEngine.Window;

namespace DrawDeltaT
{
    public class Sample : BaseEngine
    {
        public BaseLayer layer;

        public Sample() : base(new ConsoleWindow())
        {
            layer = new SingleColorLayer(new Vec2i(), new Vec2i(window.width, window.height), new Vec3(1, 1, 1), new Vec3(0, 0, 0));
            layer.Clear(new Chexel(' ', new Vec3(1, 1, 1), new Vec3(0, 0, 0)));

            AddLayer(layer);

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