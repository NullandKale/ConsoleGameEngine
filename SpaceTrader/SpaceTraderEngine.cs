using ConsoleGameEngine;
using ConsoleGameEngine.DataStructures;
using ConsoleGameEngine.Entities;
using ConsoleGameEngine.Window;

namespace SpaceTrader
{
    public class SpaceTraderEngine : BaseEngine
    {
        public StarLayer layer;
        public SolarSystem solarSystem;
        public Ship ship;
        public Random random;
        public Vec3 black = new Vec3();
        public Vec3 white = new Vec3(1, 1, 1);

        public SpaceTraderEngine() : base(new ConsoleWindow(ConsolePalette.SudoHSV))
        {
            layer = new StarLayer(new Vec2i(), window.size, this, 25, 0);
            layer.SetClear(new Chexel(' ', white, black));

            random = new Random();
            solarSystem = new SolarSystem(WindowCenter());
            solarSystem.LoadFromSeed(0, WindowCenter());

            ship = new Ship(this);

            AddLayer(layer);

            AddEntity(0, new FPSEntity(new Vec2i()));
            AddEntity(0, solarSystem);

            AddEntity(0, ship);

            CenterWindow(-10, 0);
        }

        public Vec2i WindowCenter()
        {
            return new Vec2i(window.size.x / 2, window.size.y / 2);
        }

        public void MoveWindow(int dx, int dy)
        {
            solarSystem.solarSystemData.center.x = solarSystem.solarSystemData.center.x + dx;
            solarSystem.solarSystemData.center.y = solarSystem.solarSystemData.center.y + dy;
        }

        public void CenterWindow(int x, int y)
        {
            solarSystem.solarSystemData.center.x = (window.size.x / 2) - x;
            solarSystem.solarSystemData.center.y = (window.size.y / 2) - y;
        }

        public Vec2i GetWindowOffset()
        {
            return solarSystem.solarSystemData.center;
        }
    }
}
