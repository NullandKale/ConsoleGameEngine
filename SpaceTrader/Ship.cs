using ConsoleGameEngine;
using ConsoleGameEngine.DataStructures;
using ConsoleGameEngine.Entities;
using ConsoleGameEngine.Layers;
using SpaceTrader.ShipSystems;
using SpaceTrader.ShipSystems.Engine;
using SpaceTrader.ShipSystems.Sensors;

namespace SpaceTrader
{


    public class Ship : Entity
    {
        public SpaceTraderEngine gameEngine;

        public SolarSystemData solarSystemData;

        private List<ShipSystem> systems = new List<ShipSystem>();

        public string STATUS = "DEFAULT";
        public Vec2 positionF; // Position using float for precision

        public float weight = 100; // Weight of the ship

        public Ship(SpaceTraderEngine engine, float shipWeight = 1) : base(new Vec2i())
        {
            gameEngine = engine;
            solarSystemData = engine.solarSystem.solarSystemData;
            positionF = new Vec2(); // Initialize float position
            weight = shipWeight; // Set the ship's weight
            
            systems.Add(new StandardEngine(this, 1));
            systems.Add(new StandardSensors(this, 2));
                
            Input.Add(OnKey);
        }

        private void OnKey(ConsoleKeyInfo keyPress)
        {
            foreach (var system in systems)
            {
                if (ShipSystem.CanProcessInput(system))
                {
                    system.OnKeyPressed(keyPress);
                }
            }
        }

        public override void Update(float deltaT)
        {
            solarSystemData = gameEngine.solarSystem.solarSystemData;

            foreach (var system in systems)
            {
                if (ShipSystem.CanProcessInput(system))
                {
                    system.Update(deltaT);
                }
            }

            gameEngine.CenterWindow(position.x, position.y);
        }

        public override void DrawTo(BaseLayer layer)
        {
            // draw @ position on map
            Vec2i windowOffset = gameEngine.GetWindowOffset();

            layer.WriteChexel(new Chexel('@', new Vec3(1, 0, 1), new Vec3()), position + windowOffset);

            foreach (var system in systems)
            {
                if (ShipSystem.CanProcessInput(system))
                {
                    system.DrawTo(layer);
                }
            }
        }

        public override Vec2i GetSize()
        {
            return new Vec2i(1, 1);
        }

        public override void PreUpdate(float deltaT)
        {
        }
    }
}
