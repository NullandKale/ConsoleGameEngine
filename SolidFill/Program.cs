using ConsoleGameEngine;
using ConsoleGameEngine.DataStructures;
using ConsoleGameEngine.Entities;
using ConsoleGameEngine.Layers;
using ConsoleGameEngine.Window;

namespace SolidFill
{
    public class Sample : BaseEngine
    {
        public BaseLayer layer;

        public Sample() : base(new ConsoleWindow())
        {
            layer = new SingleColorLayer(new Vec2i(), new Vec2i(window.width, window.height), new Vec3(1,0,1), new Vec3());
            layer.Clear(new Chexel('X', new Vec3(1, 0, 1), new Vec3(0, 0, 0)));

            AddLayer(layer);
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