using ConsoleGameEngine.DataStructures;

namespace SpaceTrader
{
    public enum StarClass
    {
        BlackHole,
        Neutron,
        A,
        B,
        C,
        D,
        E,
        F
    }

    public enum PlanetClass
    {
        A,
        B,
        C,
        D,
        E,
        F
    }

    public enum PlanetAtmosphereType
    {
        A,
        B,
        C,
        D,
        E,
        F,
        V
    }

    public struct StarData
    {
        public StarClass starClass;
        public int radius;
        public Vec2i position;
        public float orbitalSpeed;
    }

    public struct PlanetData
    {
        public int radius;
        public Vec2i position;
        public float orbitalSpeed;
        public PlanetClass planetClass;
        public PlanetAtmosphereType atmosphereType;
        public float averageTemperature;
    }
    public struct SolarSystemData
    {
        public Vec2i center;
        public int seed;
        public List<StarData> stars;
        public List<PlanetData> planets;

        // Calculate the gravitational force at a given position
        public Vec2 GetGravityAt(Vec2 position)
        {
            Vec2 totalGravityForce = new Vec2(0, 0);
            float G = 0.05f; // Adjusted gravitational constant
            float epsilon = 0.01f; // To prevent division by zero
            float p = 1.5f; // Gravity fall-off rate control

            foreach (var planet in planets)
            {
                Vec2 planetPos = new Vec2(planet.position.x, planet.position.y);
                Vec2 direction = position - planetPos;
                float distanceSquared = direction.x * direction.x + direction.y * direction.y + epsilon;
                float distancePowered = (float)System.Math.Pow(distanceSquared, p / 2.0f);
                float forceMagnitude = G * planet.radius / distancePowered; // Simplified for example, adjust as needed

                Vec2 forceDirection = direction.Normalized();
                Vec2 gravityForce = forceDirection * forceMagnitude;
                totalGravityForce += gravityForce;
            }

            return totalGravityForce;
        }

        public static SolarSystemData LoadFromSeed(int seed, Vec2i center)
        {
            Random random = new Random(seed);
            SolarSystemData solarSystemData = new SolarSystemData
            {
                center = center,
                seed = seed,
                stars = new List<StarData>(),
                planets = new List<PlanetData>()
            };

            int starCount = random.Next(1, 3); // 1 or 2 stars

            // Variables for binary star system
            double firstStarAngle = 0;
            int orbitRadius = starCount > 1 ? random.Next(9, 11) : 0; // Only needed for binary

            // Create star data
            for (int i = 0; i < starCount; i++)
            {
                int starRadius = random.Next(4, 7);
                float orbitalSpeed = 0;
                Vec2i starPosition;

                if (starCount == 1)
                {
                    // Single star at the center
                    starPosition = new Vec2i();
                }
                else
                {
                    // Binary star system
                    if (i == 0)
                    {
                        firstStarAngle = random.NextDouble() * Math.PI * 2; // Angle for the first star
                    }
                    double angle = i == 0 ? firstStarAngle : firstStarAngle + Math.PI; // Opposite angle for the second star
                    starPosition = new Vec2i(
                        (int)(orbitRadius * Math.Cos(angle)),
                        (int)(orbitRadius * Math.Sin(angle))
                    );
                    orbitalSpeed = (float)random.NextDouble() * 0.0005f;
                }

                solarSystemData.stars.Add(new StarData
                {
                    starClass = GenerateStarClass(random),
                    position = starPosition,
                    radius = starRadius,
                    orbitalSpeed = orbitalSpeed
                });
            }

            // Create planet data
            int planetCount = random.Next(4, 50); // 1 to 11 planets
            for (int i = 0; i < planetCount; i++)
            {
                orbitRadius = random.Next(200, 500);
                double angle = random.NextDouble() * Math.PI * 2;
                Vec2i planetPosition = new Vec2i(
                    (int)(orbitRadius * Math.Cos(angle)),
                    (int)(orbitRadius * Math.Sin(angle))
                );
                int planetRadius = random.Next(1, 3);
                float orbitalSpeed = (float)random.NextDouble() * 0.0000005f;

                solarSystemData.planets.Add(new PlanetData
                {
                    planetClass = GeneratePlanetClass(random),
                    atmosphereType = GenerateAtmosphereType(random),
                    averageTemperature = GenerateAverageTemperature(random),
                    position = planetPosition,
                    radius = planetRadius,
                    orbitalSpeed = orbitalSpeed
                });
            }

            return solarSystemData;
        }
        private static StarClass GenerateStarClass(Random random)
        {
            // Implement your logic to determine the star class
            return (StarClass)random.Next(Enum.GetNames(typeof(StarClass)).Length);
        }

        private static PlanetClass GeneratePlanetClass(Random random)
        {
            // Implement your logic to determine the planet class
            return (PlanetClass)random.Next(Enum.GetNames(typeof(PlanetClass)).Length);
        }

        private static PlanetAtmosphereType GenerateAtmosphereType(Random random)
        {
            // Implement your logic to determine the planet's atmosphere type
            return (PlanetAtmosphereType)random.Next(Enum.GetNames(typeof(PlanetAtmosphereType)).Length);
        }

        private static float GenerateAverageTemperature(Random random)
        {
            // Implement your logic to generate the average temperature
            return (float)random.NextDouble() * 100; // Example: 0 to 100 degrees
        }
    }
}