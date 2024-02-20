using ConsoleGameEngine.DataStructures;
using ConsoleGameEngine.Entities;
using ConsoleGameEngine.Layers;

namespace SpaceTrader
{
    public class OrbitalBody : Entity
    {
        public int radius;
        public float orbitalSpeed;
        public Vec2 offset;
        private double angle;
        private Vec2 floatPosition;

        private PlanetData? planetData;
        private StarData? starData;

        public OrbitalBody(Vec2i position, Vec2 offset, int radius, float orbitalSpeed, PlanetData? planetData = null, StarData? starData = null) : base(position)
        {
            this.radius = radius;
            this.orbitalSpeed = orbitalSpeed;
            this.offset = offset;
            this.angle = Math.Atan2(position.y, position.x);
            this.floatPosition = new Vec2(position.x, position.y);
            this.planetData = planetData;
            this.starData = starData;
        }

        public override void Update(float deltaT)
        {
            // Update the angle
            angle += orbitalSpeed * deltaT;

            // Calculate the new position using float for precision
            float orbitRadius = (float)Math.Sqrt(Math.Pow(floatPosition.x, 2) + Math.Pow(floatPosition.y, 2));
            floatPosition = new Vec2(
                orbitRadius * (float)Math.Cos(angle),
                orbitRadius * (float)Math.Sin(angle)
            );

            // Update the integer position for rendering
            position = new Vec2i((int)(floatPosition.x + offset.x), (int)(floatPosition.y + offset.y));
        }

        public override void DrawTo(BaseLayer layer)
        {
            var screenSize = layer.GetSize();
            if (IsOnScreen(screenSize))
            {
                Vec3 color;
                if (planetData.HasValue)
                {
                    color = GetPlanetColor(planetData.Value.planetClass);
                    DrawCircle(layer, position, radius, true, color, color); // Planets are filled with a single color

                    if (ShouldHaveRings(planetData.Value.planetClass))
                    {
                        Vec3 ringColor = GetRingColor(planetData.Value.planetClass);
                        DrawCircle(layer, position, radius + 3, false, ringColor, ringColor); // Draw rings
                    }
                }
                else if (starData.HasValue)
                {
                    color = GetStarColor(starData.Value.starClass);
                    DrawCircle(layer, position, radius, true, color, color); // Stars are filled with a single color
                }
            }
            else
            {
                DrawOffScreenArrow(layer, screenSize);
            }
        }


        private bool IsOnScreen(Vec2i screenSize)
        {
            return position.x >= 0 && position.x < screenSize.x && position.y >= 0 && position.y < screenSize.y;
        }

        private void DrawCircle(BaseLayer layer, Vec2i position, int radius, bool filled, Vec3 color0, Vec3 color1)
        {
            float aspectRatio = 2.25f; // Adjust if necessary for your character dimensions

            for (int y = -radius; y <= radius; y++)
            {
                for (int x = -radius; x <= radius; x++)
                {
                    // Adjust the x-coordinate by dividing by the aspect ratio for circular appearance
                    if ((x * x) + ((y * y) / (aspectRatio * aspectRatio)) <= radius * radius)
                    {
                        Vec2i drawPos = new Vec2i(position.x + x, position.y + y);
                        float lerpFactor = (float)(x + radius) / (2 * radius); // Calculate the lerp factor based on x position
                        Vec3 color = Vec3.Lerp(color0, color1, filled ? 1.0f : lerpFactor); // Use color0 if filled, otherwise blend between color0 and color1
                        layer.WriteChexel(new Chexel('*', color, new Vec3()), drawPos);
                    }
                }
            }
        }


        private void DrawPlanet(BaseLayer layer)
        {
            var color = GetPlanetColor(planetData.Value.planetClass);

            // The aspect ratio accounts for non-square characters (typically, characters are taller than wide)
            float aspectRatio = 2.25f;

            for (int y = -radius; y <= radius; y++)
            {
                for (int x = -radius; x <= radius; x++)
                {
                    // Applying aspect ratio to the x-coordinate and keeping calculations in float
                    float adjustedX = x / aspectRatio;
                    if (adjustedX * adjustedX + y * y <= radius * radius)
                    {
                        Vec2i drawPos = new Vec2i(position.x + x, position.y + y);
                        layer.WriteChexel(new Chexel('O', color, new Vec3()), drawPos);
                    }
                }
            }

            // Draw rings if applicable
            if (ShouldHaveRings(planetData.Value.planetClass))
            {
                DrawRings(layer, GetRingColor(planetData.Value.planetClass));
            }
        }

        private void DrawStar(BaseLayer layer)
        {
            var color = GetStarColor(starData.Value.starClass);

            float aspectRatio = 2.25f; // This is used to adjust the Y-coordinates

            for (int y = -radius; y <= radius; y++)
            {
                for (int x = -radius; x <= radius; x++)
                {
                    // Adjust the y-coordinate by dividing by the aspect ratio, to account for the taller height of characters.
                    // This ensures the circle's calculation remains accurate for the display's character dimensions.
                    if ((x * x) + ((y * y) / (aspectRatio * aspectRatio)) <= radius * radius)
                    {
                        // The drawing position does not need to be adjusted; the calculation ensures a circular shape.
                        Vec2i drawPos = new Vec2i(position.x + x, position.y + y);
                        layer.WriteChexel(new Chexel('*', color, new Vec3()), drawPos);
                    }
                }
            }
        }


        private void DrawRings(BaseLayer layer, Vec3 ringColor)
        {
            int ringInnerRadius = radius + 2;
            int ringOuterRadius = ringInnerRadius + 1; // Adjust for desired ring thickness

            float aspectRatioSquared = 2.25f * 2.25f; // Square of the aspect ratio

            for (int y = -ringOuterRadius; y <= ringOuterRadius; y++)
            {
                for (int x = -ringOuterRadius; x <= ringOuterRadius; x++)
                {
                    if (IsInRing(x, y, ringInnerRadius, ringOuterRadius, aspectRatioSquared))
                    {
                        Vec2i drawPos = new Vec2i(position.x + x, position.y + (int)(y * 2.25f));
                        layer.WriteChexel(new Chexel('o', ringColor, new Vec3()), drawPos);
                    }
                }
            }
        }

        private bool IsInRing(int x, int y, int innerRadius, int outerRadius, float aspectRatioSquared)
        {
            // Adjust x-coordinate by aspect ratio
            double distanceSquared = x * x + (y * y * aspectRatioSquared);
            return distanceSquared >= innerRadius * innerRadius && distanceSquared <= outerRadius * outerRadius;
        }


        private Vec3 GetPlanetColor(PlanetClass planetClass)
        {
            switch (planetClass)
            {
                case PlanetClass.A: return new Vec3(0.5f, 0.5f, 1f); // Blue
                case PlanetClass.B: return new Vec3(1f, 0.5f, 0f); // Orange
                case PlanetClass.C: return new Vec3(0f, 1f, 0f); // Green
                case PlanetClass.D: return new Vec3(0.5f, 0.5f, 0.5f); // Grey
                case PlanetClass.E: return new Vec3(1f, 1f, 1f); // White
                case PlanetClass.F: return new Vec3(1f, 0f, 0f); // Red
                default: return new Vec3(1f, 1f, 1f); // Default White
            }
        }

        private Vec3 GetStarColor(StarClass starClass)
        {
            switch (starClass)
            {
                case StarClass.BlackHole: return new Vec3(0.1f, 0.1f, 0.1f); // Very dark
                case StarClass.Neutron: return new Vec3(0.8f, 0.8f, 0.9f); // Light blueish
                case StarClass.A: return new Vec3(1f, 1f, 0.5f); // Yellowish
                case StarClass.B: return new Vec3(0f, 0f, 1f); // Blue
                case StarClass.C: return new Vec3(1f, 0f, 0f); // Red
                case StarClass.D: return new Vec3(0.5f, 0.5f, 0.5f); // Grey
                case StarClass.E: return new Vec3(1f, 1f, 1f); // White
                case StarClass.F: return new Vec3(1f, 0.5f, 0f); // Orange
                default: return new Vec3(1f, 1f, 1f); // Default White
            }
        }

        private bool ShouldHaveRings(PlanetClass planetClass)
        {
            return planetClass == PlanetClass.E;
        }

        private Vec3 GetRingColor(PlanetClass planetClass)
        {
            return new Vec3(0.7f, 0.7f, 0.7f);
        }

        private void DrawOffScreenArrow(BaseLayer layer, Vec2i screenSize)
        {
            int arrowX = Math.Max(1, Math.Min(screenSize.x - 2, position.x));
            int arrowY = Math.Max(1, Math.Min(screenSize.y - 2, position.y));
            char arrowChar = 'v';
            if (position.x < 0) arrowChar = '<';
            if (position.x >= screenSize.x) arrowChar = '>';
            if (position.y < 0) arrowChar = '^';

            layer.WriteChexel(new Chexel(arrowChar, new Vec3(1, 0, 0), new Vec3()), new Vec2i(arrowX, arrowY));
        }

        public override Vec2i GetSize()
        {
            return new Vec2i(radius * 2, radius * 2);
        }

        public override void PreUpdate(float deltaT)
        {
            // Implement any logic that needs to run before each frame update
        }
    }
}
