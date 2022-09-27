using ConsoleGameEngine;
using ConsoleGameEngine.DataStructures;
using ConsoleGameEngine.Entities;
using ConsoleGameEngine.Entities.UI;
using ConsoleGameEngine.Entities.Utilities;
using ConsoleGameEngine.Layers;
using ConsoleGameEngine.Window;

namespace ColorPalette
{
    public class RainbowLayer : FullScreenLayer
    {
        public float totalTimeMS = 0;

        bool colorForeground = false;

        public RainbowLayer(Vec2i position, Vec2i size, BaseEngine engine) : base(position, size)
        {
            engine.AddEntity(0, new UpdateEntity((deltaT) => 
            {
                totalTimeMS += deltaT;
            }));
        }

        protected override Chexel Read(Vec2i pos)
        {
            float tx = (float)pos.x / (float)size.x;
            float ty = (float)pos.y / (float)size.y;

            Vec3 fore = foregroundColors[pos.x, pos.y];
            Vec3 back = backgroundColors[pos.x, pos.y];

            if(colorForeground)
            {
                fore = Vec3.HsvToRgb((int)(tx * 360f), (MathF.Sin(totalTimeMS / 1000f) + 1f) / 2f, ty);
            }
            else
            {
                back = Vec3.HsvToRgb((int)(tx * 360f), (MathF.Sin(totalTimeMS / 1000f) + 1f) / 2f, ty);
            }

            return new Chexel(characters[pos.x, pos.y], fore, back);
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

    public class Sample : BaseEngine
    {
        public RainbowLayer layer;
        public Sample() : base(new ConsoleWindow(ConsolePalette.BitBased))
        {
            layer = new RainbowLayer(new Vec2i(), window.size, this);
            layer.SetClear(new Chexel(' ', new Vec3(), new Vec3(1,1,1)));

            AddLayer(layer);
            AddEntity(0, new ScreenshotEntity(new Vec2i(), new Vec2i(533, 300), this));

            UIMenu menu = new UIMenu(new Vec2i(window.size.x / 2, window.size.y / 2));
            menu.AddEntity(new FPSEntity(new Vec2i()));
            menu.AddEntity(new UIText(new Vec2i(), "Press 1 - 4 to change color palette, press f1 to take a screenshot", new Vec3(), new Vec3(1, 1, 1)));
            menu.AddEntity(new UIText(new Vec2i(0, window.size.y - 1), "Palette: BitBased", new Vec3(), new Vec3(1, 1, 1), updatePalette));

            menu.CenterOn(new Vec2i(window.size.x / 2, window.size.y / 2));

            AddEntity(0, menu);
        }

        public void updatePalette(UIText entity, ConsoleKeyInfo key)
        {
            switch(key.Key)
            {
                case ConsoleKey.D1:
                    {
                        (window as ConsoleWindow)!.palette = ConsolePalette.BitBased;
                        entity.text = "Palette: BitBased";
                        break;
                    }
                case ConsoleKey.D2:
                    {
                        (window as ConsoleWindow)!.palette = ConsolePalette.SudoHSV;
                        entity.text = "Palette: SudoHSV";
                        break;
                    }
                case ConsoleKey.D3:
                    {
                        (window as ConsoleWindow)!.palette = ConsolePalette.NameSearch;
                        entity.text = "Palette: NameSearch";
                        break;
                    }
                case ConsoleKey.D4:
                    {
                        (window as ConsoleWindow)!.palette = ConsolePalette.GreyScale;
                        entity.text = "Palette: GreyScale";
                        break;
                    }

            }
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