using ConsoleGameEngine.DataStructures;
using ConsoleGameEngine.Layers;

namespace SpaceTrader.ShipSystems
{
    public abstract class ShipSystem
    {
        protected Ship ship;
        protected string STATUS = "DEFAULT"; // Status text
        protected int statusPositionOffsetY; // Vertical offset for status display

        // Static property to track which system has exclusive access (if any)
        protected static ShipSystem exclusiveAccessSystem = null;

        public ShipSystem(Ship ship, int statusPositionOffsetY)
        {
            this.ship = ship;
            this.statusPositionOffsetY = statusPositionOffsetY;
        }

        public abstract void Update(float deltaT);

        public virtual void DrawTo(BaseLayer layer)
        {
            Vec2i layerSize = layer.GetSize();
            int maxLength = Math.Min(STATUS.Length, layerSize.x);
            int statusOutputPosition = layerSize.y - statusPositionOffsetY; // Adjust based on offset
            for (int i = 0; i < maxLength; i++)
            {
                layer.WriteChexel(new Chexel(STATUS[i], new Vec3(1, 1, 1), new Vec3(0, 0, 0)), new Vec2i(i, statusOutputPosition));
            }
        }

        protected void UpdateStatus(string newStatus)
        {
            STATUS = newStatus;
        }

        // Method to handle input, to be overridden by subclasses if they need to process input
        public virtual void OnKeyPressed(ConsoleKeyInfo keyPress)
        {
            // Default implementation does nothing
        }

        // Method for a system to request exclusive access
        public void RequestExclusiveAccess()
        {
            exclusiveAccessSystem = this;
        }

        // Method to release exclusive access, ensuring it's released by the system that holds it
        public void ReleaseExclusiveAccess()
        {
            if (exclusiveAccessSystem == this)
            {
                exclusiveAccessSystem = null;
            }
        }

        // Static method to check if the calling system can process input
        public static bool CanProcessInput(ShipSystem system)
        {
            // If no system has exclusive access, or the calling system is the one with exclusive access, it can process input
            return exclusiveAccessSystem == null || exclusiveAccessSystem == system;
        }
    }
}
