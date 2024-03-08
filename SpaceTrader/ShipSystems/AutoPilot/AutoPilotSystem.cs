using ConsoleGameEngine.DataStructures;
using ConsoleGameEngine.Layers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceTrader.ShipSystems.AutoPilot
{
    public class AutoPilotSystem : ShipSystem
    {
        private bool isActive = false;
        private Vec2 targetOrbitPosition;
        private float desiredOrbitDistance = 10.0f;

        public AutoPilotSystem(Ship ship, int statusPositionOffsetY) : base(ship, statusPositionOffsetY)
        {
        }

        public override void OnKeyPressed(ConsoleKeyInfo keyPress)
        {
            if (keyPress.Key == ConsoleKey.C)
            {
                isActive = !isActive;
            }
        }


        /*
AP: ON | Dist: 83.45 | Thrust: 0.05 | Dir: 0.13, 0.99 | Pos: X:45.75, Y:45.75 | Target Pos: X:35.00, Y:-37.00 | Grav Effect: 0.00057,0.00013
        Ship Position: X:  45.75, Y:  45.75 | Selected Star Distance: X: -10.00, Y: -82.00
        THR: 0.00003,0.00025 | GRAV: 0.00008,0.00002 | VEL: 0.01000,0.01000
        */
        public override void Update(float deltaT)
        {
            if (!isActive)
            {
                UpdateStatus("AP: OFF");
                return;
            }

            targetOrbitPosition = (Vec2)(ship.sensorsSystem.position);

            // Current position
            float posX = ship.positionF.x;
            float posY = ship.positionF.y;

            // Target position
            float targetX = targetOrbitPosition.x;
            float targetY = targetOrbitPosition.y;

            // Calculate direction to target
            float toTargetX = targetX - posX;
            float toTargetY = targetY - posY;

            // Calculate the distance to target using Pythagorean theorem
            float distanceToTarget = (float)Math.Sqrt(toTargetX * toTargetX + toTargetY * toTargetY);

            // Normalize the direction to target
            float normToTargetX = toTargetX / distanceToTarget;
            float normToTargetY = toTargetY / distanceToTarget;

            // Invert direction if moving directly to the target is desired
            // If inverting the direction is not necessary, remove the negation
            float desiredDirX = -normToTargetX;
            float desiredDirY = -normToTargetY;

            // Base magnitude for thrust
            float thrustMagnitude = 0.05f;

            // Calculate effective thrust considering gravity (if necessary)
            // Assuming gravityEffect is the acceleration due to gravity at the ship's current position
            float gravityEffectX = ship.solarSystemData.GetGravityAt(ship.positionF).x * deltaT;
            float gravityEffectY = ship.solarSystemData.GetGravityAt(ship.positionF).y * deltaT;

            // Adjust thrust direction for gravity
            float effectiveThrustX = desiredDirX + gravityEffectX;
            float effectiveThrustY = desiredDirY + gravityEffectY;

            // Normalize the effective thrust direction
            float magnitude = (float)Math.Sqrt(effectiveThrustX * effectiveThrustX + effectiveThrustY * effectiveThrustY);
            effectiveThrustX /= magnitude;
            effectiveThrustY /= magnitude;

            // Apply thrust
            ship.engineSystem.SetThrust(new Vec2(effectiveThrustX * thrustMagnitude, effectiveThrustY * thrustMagnitude));

            // Update status with detailed debug information
            UpdateStatus($"AP: ON | Dist: {distanceToTarget:F2} | Thrust: {thrustMagnitude:F2} | " +
                         $"Dir: {effectiveThrustX:F2}, {effectiveThrustY:F2} | " +
                         $"Pos: X:{posX:F2}, Y:{posY:F2} | " +
                         $"Target Pos: X:{targetX:F2}, Y:{targetY:F2} | " +
                         $"Grav Effect: {gravityEffectX:F5},{gravityEffectY:F5}");
        }




        public override void DrawTo(BaseLayer layer)
        {
            // draw status
            base.DrawTo(layer);

            if (!isActive) return;

            Vec2i windowOffset = ship.gameEngine.GetWindowOffset();

            // Draw the trajectory based on current velocity
            Vec2i currentPosition = ship.positionF.toVec2i() + windowOffset;
            Vec2i trajectoryEnd = currentPosition + (ship.engineSystem.velocity * 500).toVec2i(); // Scale for visibility
            DrawLine(layer, currentPosition, trajectoryEnd, new Vec3(0, 1, 0)); // Green for trajectory

            // Draw the ideal orbit around the target position
            Vec2i orbitCenter = targetOrbitPosition.toVec2i();
            DrawCircle(layer, orbitCenter, (int)desiredOrbitDistance, false, new Vec3(1, 0, 0), new Vec3(1, 0, 0)); // Red for orbit path
        }

        // The DrawCircle method as described, with minor adjustments for context
        private void DrawCircle(BaseLayer layer, Vec2i position, int radius, bool filled, Vec3 color0, Vec3 color1)
        {
            float aspectRatio = 2.25f; // Adjust based on your character dimensions for a circular appearance

            for (int y = -radius; y <= radius; y++)
            {
                for (int x = -radius; x <= radius; x++)
                {
                    // Calculate distance from the center point, adjusted for aspect ratio
                    float distance = (float)Math.Sqrt((x * x) + ((y * y) / (aspectRatio * aspectRatio)));

                    // Enhance condition to ensure top and bottom points are included
                    bool isOnBoundary = Math.Abs(distance - radius) < 0.75;
                    bool isVerticalExtreme = Math.Abs(y) == radius; // Check if the point is at the vertical extremes

                    if (isOnBoundary || isVerticalExtreme)
                    {
                        Vec2i drawPos = new Vec2i(position.x + x, position.y + y);
                        Vec3 color = isVerticalExtreme ? color1 : color0; // Use color1 for vertical extremes if you want them highlighted
                        layer.WriteChexel(new Chexel('*', color, new Vec3()), drawPos);
                    }
                }
            }
        }


        // Utility method to draw a line on the layer
        private void DrawLine(BaseLayer layer, Vec2i start, Vec2i end, Vec3 color)
        {
            // Bresenham's line algorithm
            int dx = Math.Abs(end.x - start.x), sx = start.x < end.x ? 1 : -1;
            int dy = -Math.Abs(end.y - start.y), sy = start.y < end.y ? 1 : -1;
            int err = dx + dy, e2;

            while (true)
            {
                layer.WriteChexel(new Chexel('*', color, new Vec3()), start); // Use '*' for the trajectory points
                if (start.x == end.x && start.y == end.y) break;
                e2 = 2 * err;
                if (e2 >= dy) { err += dy; start.x += sx; }
                if (e2 <= dx) { err += dx; start.y += sy; }
            }
        }

    }
}
