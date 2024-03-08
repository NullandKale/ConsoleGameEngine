using ConsoleGameEngine;
using ConsoleGameEngine.DataStructures;
using ConsoleGameEngine.Entities;
using ConsoleGameEngine.Layers;
using SpaceTrader.ShipSystems;
using SpaceTrader.ShipSystems.AutoPilot;
using SpaceTrader.ShipSystems.Engine;
using SpaceTrader.ShipSystems.Sensors;

namespace SpaceTrader
{
    public class Ship : Entity
    {
        public SpaceTraderEngine gameEngine;

        public SolarSystemData solarSystemData;

        public EngineSystem engineSystem;
        public SensorsSystem sensorsSystem;
        public AutoPilotSystem autopilotSystem;

        public string STATUS = "DEFAULT";
        public Vec2 positionF; // Position using float for precision

        public float weight = 100; // Weight of the ship

        public Ship(SpaceTraderEngine engine, float shipWeight = 1) : base(new Vec2i())
        {
            gameEngine = engine;
            solarSystemData = engine.solarSystem.solarSystemData;
            positionF = new Vec2(); // Initialize float position
            weight = shipWeight; // Set the ship's weight
            
            engineSystem = new StandardEngine(this, 1);
            sensorsSystem = new StandardSensors(this, 2);
            autopilotSystem = new AutoPilotSystem(this, 3);
                
            Input.Add(OnKey);
        }

        private void OnKey(ConsoleKeyInfo keyPress)
        {
            engineSystem.OnKeyPressed(keyPress);
            sensorsSystem.OnKeyPressed(keyPress);
            autopilotSystem.OnKeyPressed(keyPress);
        }

        public override void Update(float deltaT)
        {
            solarSystemData = gameEngine.solarSystem.solarSystemData;

            engineSystem.Update(deltaT);
            sensorsSystem.Update(deltaT);
            autopilotSystem.Update(deltaT);

            gameEngine.CenterWindow(position.x, position.y);
        }

        public override void DrawTo(BaseLayer layer)
        {
            engineSystem.DrawTo(layer);
            sensorsSystem.DrawTo(layer);
            autopilotSystem.DrawTo(layer);

            // draw @ position on map
            Vec2i windowOffset = gameEngine.GetWindowOffset();
            layer.WriteChexel(new Chexel('@', new Vec3(1, 0, 1), new Vec3()), position + windowOffset);
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
