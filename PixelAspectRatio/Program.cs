using ConsoleGameEngine;
using ConsoleGameEngine.DataStructures;
using ConsoleGameEngine.Entities;
using ConsoleGameEngine.Entities.UI;
using ConsoleGameEngine.Entities.Utilities;
using ConsoleGameEngine.Layers;
using ConsoleGameEngine.Window;
using System.Drawing;

namespace PIxelAspectRatio
{
    public class RainbowLayer : FullScreenLayer
    {
        public float totalTimeMS = 0;
        public float aspectRatio = 2.15f; // Aspect ratio, adjusted with Q and E
        public Action<float> OnAspectRatioChanged;

        public RainbowLayer(Vec2i position, Vec2i size, BaseEngine engine) : base(position, size)
        {
            engine.AddEntity(0, new AspectRatioAdjusterEntity(this, (deltaT) =>
            {
                totalTimeMS += deltaT;
            }));
        }

        public string GetAspectRatioString()
        {
            return $"Aspect Ratio: {aspectRatio:F2}";
        }

        protected override Chexel Read(Vec2i pos)
        {
            Vec2i center = new Vec2i(size.x / 2, size.y / 2);

            // Define the centers for each shape
            Vec2i circleCenter = new Vec2i(center.x / 2, center.y);
            Vec2i squareCenter = new Vec2i(center.x, center.y);
            Vec2i triangleCenter = new Vec2i(3 * center.x / 2, center.y);

            float shapeSize = Math.Min(size.x, size.y) / 4.5f; // Size for each shape

            if (IsInCircle(pos, circleCenter, shapeSize))
            {
                // Circle with color 1
                return new Chexel('O', new Vec3(1, 0, 0), new Vec3(1, 0, 0));
            }
            else if (IsInSquare(pos, squareCenter, shapeSize))
            {
                // Square with color 2
                return new Chexel('#', new Vec3(0, 1, 0), new Vec3(0, 1, 0));
            }
            else if (IsInTriangle(pos, triangleCenter, shapeSize))
            {
                // Triangle with color 3
                return new Chexel('^', new Vec3(0, 0, 1), new Vec3(0, 0, 1));
            }

            return base.Read(pos);
        }

        private bool IsInCircle(Vec2i pos, Vec2i center, float radius)
        {
            Vec2 adjustedPos = AdjustForAspectRatio(pos, center);
            return adjustedPos.Length() <= radius;
        }

        private bool IsInSquare(Vec2i pos, Vec2i center, float size)
        {
            Vec2 adjustedPos = AdjustForAspectRatio(pos, center);
            return Math.Abs(adjustedPos.x) <= size && Math.Abs(adjustedPos.y) <= size;
        }

        private bool IsInTriangle(Vec2i pos, Vec2i center, float baseSize)
        {
            float desiredHeight = baseSize * 2f;
            float triangleSideLength = desiredHeight / (float)Math.Sqrt(3f) * 2f;
            float triangleHeight = (float)(Math.Sqrt(3f) / 2f * triangleSideLength);

            // Calculate the vertical position of the triangle's base
            // The centroid of the triangle should align with the center of the other shapes
            float triangleBaseY = (center.y / 2f) + (triangleHeight / 3f) - baseSize;

            Vec2 adjustedPos = AdjustForAspectRatio(pos, new Vec2i(center.x, (int)triangleBaseY));
            float dx = adjustedPos.x;
            float dy = triangleHeight - (adjustedPos.y - triangleBaseY); // Distance from the base

            // Check if the point is within the bounds of the equilateral triangle
            return dy >= 0 && dy <= triangleHeight && Math.Abs(dx) <= dy / (float)Math.Sqrt(3);
        }



        private Vec2 AdjustForAspectRatio(Vec2i pos, Vec2i center)
        {
            float adjustedX = (pos.x - center.x) / aspectRatio; // Stretch X coordinates
            float adjustedY = pos.y - center.y;
            return new Vec2(adjustedX, adjustedY);
        }

        private class AspectRatioAdjusterEntity : Entity
        {
            Action<float> action;

            public AspectRatioAdjusterEntity(RainbowLayer layer, Action<float> action) : base(new Vec2i())
            {
                this.action = action;

                Input.Add((ConsoleKeyInfo keyInfo) =>
                {
                    switch (keyInfo.Key)
                    {
                        case ConsoleKey.Q:
                            layer.aspectRatio = Math.Max(0.1f, layer.aspectRatio - 0.05f);
                            break;
                        case ConsoleKey.E:
                            layer.aspectRatio = Math.Min(3f, layer.aspectRatio + 0.05f);
                            break;
                    }
                });
            }

            public override void Update(float deltaT)
            {
                // do not read key here
            }

            public override void DrawTo(BaseLayer layer)
            {
                //no draw
            }

            public override Vec2i GetSize()
            {
                return new Vec2i();
            }

            public override void PreUpdate(float deltaT)
            {
                action.Invoke(deltaT);
            }
        }
    }

    public class Sample : BaseEngine
    {
        public RainbowLayer layer;
        private UIText aspectRatioText; // To display aspect ratio

        public Sample() : base(new ConsoleWindow(ConsolePalette.BitBased))
        {
            layer = new RainbowLayer(new Vec2i(), window.size, this);
            layer.SetClear(new Chexel(' ', new Vec3(1,1,1), new Vec3(0, 0, 0)));

            AddLayer(layer);
            AddEntity(0, new ScreenshotEntity(new Vec2i(), new Vec2i(533, 300), this));

            UIMenu menu = new UIMenu(new Vec2i(0, 0));
            menu.AddEntity(new FPSEntity(new Vec2i()));
            menu.AddEntity(new UIText(new Vec2i(), "Press Q/E to adjust aspect ratio, F1 to take a screenshot", new Vec3(1, 1, 1), new Vec3()));

            aspectRatioText = new UIText(new Vec2i(0, window.size.y - 1), $"Aspect Ratio: {layer.aspectRatio:F2}", new Vec3(1, 1, 1), new Vec3());
            menu.AddEntity(aspectRatioText);

            AddEntity(0, menu);
        }

        protected override void Update(float deltaT)
        {
            base.Update(deltaT);
            aspectRatioText.text = $"Aspect Ratio: {layer.aspectRatio:F2}"; // Update aspect ratio display
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Sample sample = new Sample();

            sample.Start();

            while (sample.isRunning)
            {
                Thread.Sleep(100);
            }
        }
    }
}