using ConsoleGameEngine.DataStructures;
using ConsoleGameEngine.Entities;
using ConsoleGameEngine.Layers;

namespace Island.Agents
{
    public class Agent : Entity
    {
        private AgentManager manager;
        public Vec2 positionF;
        public Vec2 velocity;
        public Vec2 homePosition;
        public bool isPlayerControlled;
        private float wanderTimer = 0;
        private const float wanderInterval = 2000.0f;

        public Agent(AgentManager manager, Vec2i position, bool isPlayerControlled = false) : base(position)
        {
            this.manager = manager;
            this.isPlayerControlled = isPlayerControlled;
            positionF = (Vec2)position;
            homePosition = positionF; // Save the spawn position as the home position
            velocity = new Vec2();
        }

        public override void Update(float deltaT)
        {
            if (!isPlayerControlled)
            {
                AIUpdate(deltaT / 1000.0f); // Convert ms to seconds for AIUpdate

                positionF += velocity; // Adjust movement by deltaT converted to seconds
            }
            else
            {
                // move one at a time for player controlled character
                positionF += velocity;
                velocity = new Vec2();
            }
        }

        private void AIUpdate(float deltaTS)
        {
            // Calculate the scaled time as a proportion of the day
            
            float dayProgress = manager.time / manager.totalDayTimeMS;
            // Determine if it's night. Assuming night starts at 75% of the dayProgress and ends at 25%
            bool isNight = dayProgress > 0.75 || dayProgress < 0.25;

            if (isNight)
            {
                // Night behavior: Return to home if not already there
                MoveTowardsHomeIfNecessary();
            }
            else
            {
                // Day behavior: Attempt to congregate if any agents are nearby
                AttemptToCongregate(deltaTS);
            }

            AvoidOverlapping();
        }

        private void AvoidOverlapping()
        {
            float minimumSeparationDistance = 1f; // Minimal distance to maintain from other agents
            foreach (var otherAgent in manager.agents)
            {
                if (otherAgent != this && (otherAgent.positionF - positionF).LengthSquared() <= minimumSeparationDistance * minimumSeparationDistance)
                {
                    // Calculate a direction vector away from the other agent
                    Vec2 separationDirection = positionF - otherAgent.positionF;
                    if (separationDirection.LengthSquared() == 0)
                    {
                        // If agents are exactly overlapped, choose a random direction
                        separationDirection = new Vec2(manager.engine.GetRandom(-1f, 1f), manager.engine.GetRandom(-1f, 1f)).Normalized();
                    }
                    else
                    {
                        // Normalize the separation direction
                        separationDirection = separationDirection.Normalized();
                    }

                    // Apply a small velocity to move away from the other agent
                    velocity += separationDirection * 0.05f; // Adjust the multiplier as necessary for your game dynamics
                    break; // Optionally only avoid one agent per update to simplify behavior
                }
            }
        }


        private void SetVelocityTowards(Vec2 target, float speedFactor)
        {
            // Calculate the direction vector from the agent to the target
            Vec2 direction = target - positionF;
            // Normalize the direction vector to have a length of 1
            Vec2 normalizedDirection = direction.Normalized();
            // Set the agent's velocity towards the target, scaled by the speed factor
            velocity = normalizedDirection * speedFactor;
        }

        private void MoveTowardsHomeIfNecessary()
        {
            if ((homePosition - positionF).Length() > 0.1) // Check if not at home
            {
                SetVelocityTowards(homePosition, 0.02f); // Move towards home with a specified speed
            }
            else
            {
                velocity = new Vec2(); // Stop moving once home
            }
        }

        private void AttemptToCongregate(float deltaTS)
        {
            float detectionDistance = 20.0f; // Distance to look for other agents
            var nearbyAgents = manager.GetAgentsInRadius(positionF, detectionDistance);

            if (nearbyAgents.Count > 1 || (nearbyAgents.Count == 1 && nearbyAgents[0] != this))
            {
                Vec2 averagePosition = CalculateAveragePosition(nearbyAgents);
                MoveTowardsAveragePositionIfAppropriate(averagePosition);
            }
            else
            {
                velocity = new Vec2(); // No nearby agents, stop moving
            }
        }

        private Vec2 CalculateAveragePosition(List<Agent> agents)
        {
            Vec2 averagePosition = new Vec2();
            int count = 0;
            foreach (var agent in agents)
            {
                if (agent != this)
                {
                    averagePosition += agent.positionF;
                    count++;
                }
            }
            if (count > 0) averagePosition /= count;
            return averagePosition;
        }

        private void MoveTowardsAveragePositionIfAppropriate(Vec2 averagePosition)
        {
            float congregationDistance = 2.0f; // Maintain this distance from other agents
            if ((averagePosition - positionF).Length() > congregationDistance)
            {
                SetVelocityTowards(averagePosition, 0.01f); // Move towards the average position
            }
            else
            {
                velocity = new Vec2(); // Close enough to other agents, stop moving
            }
        }

        public override void DrawTo(BaseLayer layer)
        {
            position = positionF.toVec2i();
            layer.WriteChexel(new Chexel('@', new Vec3(1, 0, 1), new Vec3()), position + manager.offset);
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