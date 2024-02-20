using ConsoleGameEngine.DataStructures;
using ConsoleGameEngine.Entities;
using ConsoleGameEngine.Layers;
using System.Collections.Generic;

namespace SpaceTrader
{
    public class SolarSystem : Entity
    {
        public List<OrbitalBody> bodies;
        public SolarSystemData solarSystemData;

        public SolarSystem(Vec2i position) : base(position)
        {
            bodies = new List<OrbitalBody>();
        }

        public void LoadFromSeed(int seed, Vec2i center)
        {
            bodies = new List<OrbitalBody>();

            // Generate solar system data
            solarSystemData = SolarSystemData.LoadFromSeed(seed, center);

            // Create OrbitalBody objects based on the generated solar system data
            foreach (var starData in solarSystemData.stars)
            {
                bodies.Add(new OrbitalBody(starData.position, new Vec2(center.x, center.y), starData.radius, starData.orbitalSpeed, null, starData));
            }

            foreach (var planetData in solarSystemData.planets)
            {
                bodies.Add(new OrbitalBody(planetData.position, new Vec2(center.x, center.y), planetData.radius, planetData.orbitalSpeed, planetData, null));
            }
        }

        public override void DrawTo(BaseLayer layer)
        {
            foreach (var body in bodies)
            {
                body.DrawTo(layer);
            }
        }

        public override Vec2i GetSize()
        {
            int minX = int.MaxValue, minY = int.MaxValue, maxX = int.MinValue, maxY = int.MinValue;

            foreach (var body in bodies)
            {
                minX = Math.Min(minX, body.position.x - body.radius);
                minY = Math.Min(minY, body.position.y - body.radius);
                maxX = Math.Max(maxX, body.position.x + body.radius);
                maxY = Math.Max(maxY, body.position.y + body.radius);
            }

            return new Vec2i(maxX - minX, maxY - minY);
        }

        public override void PreUpdate(float deltaT)
        {
            foreach (var body in bodies)
            {
                body.PreUpdate(deltaT);
            }
        }

        public override void Update(float deltaT)
        {
            foreach (var body in bodies)
            {
                body.offset = new Vec2(solarSystemData.center.x, solarSystemData.center.y);
                body.Update(deltaT);
            }

            // Sync the updated positions back to solarSystemData
            for (int i = 0; i < solarSystemData.stars.Count; i++)
            {
                if (i < bodies.Count)
                {
                    var body = bodies[i];
                    var starData = solarSystemData.stars[i];
                    starData.position = body.position;
                    solarSystemData.stars[i] = starData;
                }
            }

            for (int i = 0; i < solarSystemData.planets.Count; i++)
            {
                int bodyIndex = solarSystemData.stars.Count + i;
                if (bodyIndex < bodies.Count)
                {
                    var body = bodies[bodyIndex];
                    var planetData = solarSystemData.planets[i];
                    planetData.position = body.position;
                    solarSystemData.planets[i] = planetData;
                }
            }
        }
    }
}