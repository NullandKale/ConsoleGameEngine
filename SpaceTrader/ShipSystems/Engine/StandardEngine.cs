using ConsoleGameEngine.Layers;

namespace SpaceTrader.ShipSystems.Engine
{
    public class StandardEngine : EngineSystem
    {
        public StandardEngine(Ship ship, int statusOffset) : base(ship, statusOffset)
        {
            baseThrust = 0.00005f; // Example value, adjust as needed
            brakePower = 0.0008f; // Example value, adjust as needed
        }

    }
}
