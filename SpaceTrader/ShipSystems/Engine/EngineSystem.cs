using ConsoleGameEngine.DataStructures;
using ConsoleGameEngine.Layers;

namespace SpaceTrader.ShipSystems
{
    public abstract class EngineSystem : ShipSystem
    {
        public float baseThrust; // max rate of change when moving normally
        public float brakePower; // max rate of change when breaking

        public Vec2 velocity = new Vec2(); // Current ships velocity
        public Vec2 maxVelocity = new Vec2(); // Max Velocity;

        public Vec2 thrusters = new Vec2(); // Current acceleration due to thrusters
        public Vec2 gravityAcceleration = new Vec2(); // Current acceleration due to gravity

        public EngineSystem(Ship ship, int statusPositionOffsetY) : base(ship, statusPositionOffsetY)
        {
        }

        public override void OnKeyPressed(ConsoleKeyInfo keyPress)
        {
            Vec2 thrustDirection = new Vec2(0, 0);

            switch (keyPress.Key)
            {
                case ConsoleKey.W: thrustDirection.y -= 1; break;
                case ConsoleKey.S: thrustDirection.y += 1; break;
                case ConsoleKey.A: thrustDirection.x -= 1; break;
                case ConsoleKey.D: thrustDirection.x += 1; break;
                case ConsoleKey.Spacebar:
                    ApplyBreak();
                    break;
            }

            if(keyPress.Key == ConsoleKey.W
                || keyPress.Key == ConsoleKey.A
                || keyPress.Key == ConsoleKey.S
                || keyPress.Key == ConsoleKey.D)
            {
                ApplyThrust(thrustDirection);
            }

        }



        public void ApplyThrust(Vec2 thrustDirection)
        {
            if (thrustDirection.x != 0 || thrustDirection.y != 0)
            {
                thrustDirection = thrustDirection.Normalized();
            }
            thrusters += thrustDirection * (baseThrust / ship.weight);
        }

        public void ApplyBreak()
        {
            // Calculate the total force needed to counteract both the velocity and gravity
            Vec2 totalForceNeeded = new Vec2();

            // First, calculate the force needed to negate the current velocity
            if (velocity.Length() > 0)
            {
                Vec2 brakingForce = -velocity.Normalized() * (brakePower / ship.weight);
                totalForceNeeded += brakingForce;
            }

            // Then, calculate the additional force needed to counteract gravity
            // This is necessary to "hold" the ship against gravity once it has stopped moving
            // The gravity force is simply the gravity acceleration vector scaled by the ship's weight
            // because Force = mass (weight in this context) * acceleration
            Vec2 gravityForceToCounter = -gravityAcceleration * ship.weight;

            // Scale the gravity counteracting force by the same factor as braking force for consistency
            // Assuming brakePower is also a measure of how quickly we can adjust to counter gravity
            Vec2 gravityCounteractingForce = gravityForceToCounter * (brakePower / ship.weight);

            // Add the gravity counteracting force to the total force needed
            totalForceNeeded += gravityCounteractingForce;

            // Apply the total force to the thrusters
            // This adjustment allows the thrusters to provide enough force to both stop the ship and counteract gravity
            thrusters += totalForceNeeded;
        }



        public void SetThrust(Vec2 normalizedThrust)
        {
            thrusters = normalizedThrust * (100 * baseThrust / ship.weight);
        }

        public override void Update(float deltaT)
        {
            // Incorporate gravity effect
            gravityAcceleration = ship.solarSystemData.GetGravityAt(ship.positionF);
            Vec2 gravityVelocityChange = gravityAcceleration * deltaT;

            // Apply thrusters to velocity, then apply damping
            velocity += (thrusters / ship.weight + gravityVelocityChange) * deltaT;
            velocity *= 0.95f; // Apply damping factor

            // Clamp velocity to maxVelocity
            ClampVelocity();

            // Update ship's position based on the clamped velocity
            ship.positionF += velocity * deltaT;
            ship.position = ship.positionF.toVec2i();

            // Update the status with current velocity, thrusters, and gravity
            UpdateStatus($"THR: {thrusters.x,7:F5},{thrusters.y,7:F5} | " +
                         $"GRAV: {gravityAcceleration.x,7:F5},{gravityAcceleration.y,7:F5} | " +
                         $"VEL: {velocity.x,7:F5},{velocity.y,7:F5}");
        }

        private void ClampVelocity()
        {
            // Clamp the velocity in each dimension independently to respect maxVelocity
            velocity.x = Math.Max(Math.Min(velocity.x, maxVelocity.x), -maxVelocity.x);
            velocity.y = Math.Max(Math.Min(velocity.y, maxVelocity.y), -maxVelocity.y);
        }

    }
}
