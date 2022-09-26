using ConsoleGameEngine;
using ConsoleGameEngine.DataStructures;
using ConsoleGameEngine.Entities;
using ConsoleGameEngine.Entities.UI;
using ConsoleGameEngine.Entities.Utilities;
using ConsoleGameEngine.Layers;
using ConsoleGameEngine.Window;

namespace ColorPalette
{
    public class RainbowLayer : BaseLayer
    {
        private char[,] chars;
        public float totalTimeMS = 0;

        public bool overrideForeground = false;
        public bool overrideBackground = false;
        public Vec3 overrideForegroundColor;
        public Vec3 overrideBackgroundColor;

        public RainbowLayer(Vec2i position, Vec2i size, BaseEngine engine) : base(position, size)
        {
            chars = new char[size.x, size.y];

            engine.AddEntity(0, new UpdateEntity((deltaT) => 
            {
                totalTimeMS += deltaT;
            }));
        }

        public override void Clear()
        {
            chars = new char[size.x, size.y];

            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    chars[x, y] = clear.character;
                }
            }
        }

        public override void Resize(Vec2i size)
        {
            SetSize(size);
        }

        protected override Chexel Read(Vec2i pos)
        {
            float tx = (float)pos.x / (float)size.x;
            float ty = (float)pos.y / (float)size.y;

            Vec3 fore = Vec3.HsvToRgb((int)(tx * 360f), (MathF.Sin(totalTimeMS / 1000f) + 1f) / 2f, ty);
            Vec3 back = fore;

            if(overrideBackground)
            {
                back = overrideBackgroundColor;
            }

            if(overrideForeground)
            {
                fore = overrideForegroundColor;
            }

            return new Chexel(chars[pos.x, pos.y], fore, back);
        }

        protected override void Write(Chexel toWrite, Vec2i pos)
        {
            chars[pos.x, pos.y] = toWrite.character;
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
                //no draw
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

    public class Sample : BaseEngine
    {
        public RainbowLayer layer;

        public Sample() : base(new ConsoleWindow(ConsolePalette.BitBased))
        {
            layer = new RainbowLayer(new Vec2i(), window.size, this);
            layer.overrideForeground = true;
            layer.overrideForegroundColor = new Vec3(0, 0, 0);
            layer.SetClear(new Chexel(' ', new Vec3(1, 0, 1), new Vec3(0, 0, 0)));

            AddLayer(layer);

            AddEntity(0, new UIText(new Vec2i(0, window.size.y - 3), "Press 1 - 3 to change color palette, press f1 to take a screenshot", new Vec3(), new Vec3(1, 1, 1)));

            Entity fps = new FPSEntity(new Vec2i(0, window.size.y - 2));
            AddEntity(0, fps);

            Entity palette = new UIText(new Vec2i(0, window.size.y - 1), "Palette: BitBased", new Vec3(), new Vec3(1, 1, 1),  updatePalette);
            AddEntity(0, palette);

            AddEntity(0, new ScreenshotEntity(new Vec2i(), new Vec2i(533, 300), this));
        }

        public string? updatePalette(ConsoleKeyInfo key)
        {
            switch(key.Key)
            {
                case ConsoleKey.D1:
                    {
                        (window as ConsoleWindow).palette = ConsolePalette.BitBased;
                        return "Palette: BitBased";
                    }
                case ConsoleKey.D2:
                    {
                        (window as ConsoleWindow).palette = ConsolePalette.SudoHSV;
                        return "Palette: SudoHSV";
                    }
                case ConsoleKey.D3:
                    {
                        (window as ConsoleWindow).palette = ConsolePalette.NameSearch;
                        return "Palette: NameSearch";
                    }

            }

            return null;
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