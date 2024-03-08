using ConsoleGameEngine;
using ConsoleGameEngine.DataStructures;
using System;

namespace Island.Agents
{
    public class AgentManager
    {
        public BaseEngine engine;
        public Vec2i offset;
        public List<Agent> agents;
        public float time = 0;
        public float totalDayTimeMS = 10_000.0f;

        public AgentManager(BaseEngine engine)
        {
            this.engine = engine;
            offset = new Vec2i();
            agents = new List<Agent>();
        }

        public void SetOffset(Vec2i offset)
        {
            this.offset = offset;
        }

        public void AddAgent(Agent agent)
        {
            agents.Add(agent);
            engine.AddEntity(0, agent);
        }

        public List<Agent> GetAgentsInRadius(Vec2 position, float radius)
        {
            List<Agent> result = new List<Agent>();

            for (int i = 0; i < agents.Count; i++)
            {
                if ((agents[i].positionF - position).LengthSquared() < radius * radius)
                {
                    result.Add(agents[i]);
                }
            }

            return result;
        }

        public void SpawnRandomAgent(Vec2i offset, Vec2i size)
        {
            // Calculate the spawning bounds based on window offset and size
            int minX = offset.x;
            int maxX = offset.x + size.x;
            int minY = offset.y;
            int maxY = offset.y + size.y;

            // Generate a random position within the specified bounds
            int x = engine.random.Next(minX, maxX + 1);
            int y = engine.random.Next(minY, maxY + 1);
            Vec2i position = new Vec2i(x, y);

            // Create and add the new agent
            Agent newAgent = new Agent(this, position, false); // Assuming AI-controlled
            AddAgent(newAgent);
        }

        public void Update(float deltaT_MS)
        {
            // Increment the current time by the delta time
            time += deltaT_MS;

            // Loop time to simulate continuous day cycles
            if (time > totalDayTimeMS)
            {
                time = time % totalDayTimeMS; // Correctly loop the time
            }
        }

        public string GetTimeString()
        {
            // Use a locally scaled version of 'time' for display purposes
            float scaledTime = ((time % totalDayTimeMS) / totalDayTimeMS) * 24; // Scale to 24 hours
            int hourPart = (int)scaledTime;
            int minutePart = (int)((scaledTime - hourPart) * 60);

            return $"{hourPart:D2}:{minutePart:D2}";
        }


    }
}