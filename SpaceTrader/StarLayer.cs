using ConsoleGameEngine;
using ConsoleGameEngine.DataStructures;
using ConsoleGameEngine.Entities;
using ConsoleGameEngine.Layers;
using System;
using System.Collections.Generic;
using System.Security.Principal;

namespace SpaceTrader
{
    public class StarLayer : FullScreenLayer
    {
        private class Star
        {
            public Vec2i Position;
            public Vec3 Color;
            public char Character;

            public Star(Vec2i position, Vec3 color, char character)
            {
                Position = position;
                Color = color;
                Character = character;
            }
        }

        private Dictionary<Vec2i, Star> starMap; // Use a dictionary for fast lookup
        private List<Star> stars;
        private Random random;
        private int seed;
        private int starCount;
        private float starSpeed = 0.01f;
        public float totalTimeMS = 0;
        private static readonly Vec3 blackColor = new Vec3(0, 0, 0);
        private static readonly char[] starChars = new char[] { '.', '+', 'o', '@' };

        public StarLayer(Vec2i position, Vec2i size, BaseEngine engine, int starCount, int seed) : base(position, size)
        {
            engine.AddEntity(0, new UpdateEntity((deltaT) =>
            {
                totalTimeMS += deltaT;
                UpdateStars(deltaT);
            }));

            stars = new List<Star>();
            starMap = new Dictionary<Vec2i, Star>(); // Initialize the dictionary
            random = new Random(seed);
            this.seed = seed;
            this.starCount = starCount;
            InitializeStars(size);
        }

        private void InitializeStars(Vec2i size)
        {
            stars.Clear();
            starMap.Clear(); // Clear previous entries
            for (int i = 0; i < starCount; i++)
            {
                int x = random.Next(size.x);
                int y = random.Next(size.y);
                float brightness = (float)(random.NextDouble() * 0.4) + 0.5f;
                Vec3 color = new Vec3(brightness, brightness, brightness); // Shades of gray
                char character = starChars[random.Next(starChars.Length)];
                var star = new Star(new Vec2i(x, y), color, character);
                stars.Add(star);
                starMap[star.Position] = star; // Map position to star for quick lookup
            }
        }

        public override void SetSize(Vec2i size)
        {
            base.SetSize(size);
            InitializeStars(size);
        }

        private void UpdateStars(float deltaT)
        {
            foreach (var star in stars)
            {
                starMap.Remove(star.Position); // Remove the old position from the map

                star.Position.x += (int)(starSpeed * deltaT);
                if (star.Position.x >= size.x)
                {
                    star.Position.x = 0;
                    star.Position.y = random.Next(size.y);
                }

                starMap[star.Position] = star; // Update the new position in the map
            }
        }

        private Chexel GetStarField(Vec2i pos)
        {
            Vec3 back = blackColor; // Consistent black background

            if (starMap.TryGetValue(pos, out Star star))
            {
                return new Chexel(star.Character, star.Color, back);
            }

            return new Chexel(' ', new Vec3(), back); // Empty space
        }

        protected override Chexel Read(Vec2i pos)
        {
            Chexel c = base.Read(pos);

            if (c.character == clear.character)
            {
                return GetStarField(pos);
            }

            return base.Read(pos);
        }

        private class UpdateEntity : Entity
        {
            Action<float> action;

            public UpdateEntity(Action<float> action) : base(new Vec2i())
            {
                this.action = action;
            }

            public override void DrawTo(BaseLayer layer)
            {
                // no draw
            }

            public override Vec2i GetSize()
            {
                return new Vec2i();
            }

            public override void PreUpdate(float deltaT)
            {
                action(deltaT);
            }

            public override void Update(float deltaT)
            {
                // no update
            }
        }
    }
}
