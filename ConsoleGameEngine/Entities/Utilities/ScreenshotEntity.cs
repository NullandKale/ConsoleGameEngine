using ConsoleGameEngine.DataStructures;
using ConsoleGameEngine.Layers;
using ConsoleGameEngine.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGameEngine.Entities.Utilities
{
    public class ScreenshotEntity : Entity
    {
        BaseEngine engine;
        IWindow oldWindow;
        ScreenShotWindow screenShotWindow;
        bool triggered = false;
        bool reset = false;

        public ScreenshotEntity(Vec2i position, Vec2i size, BaseEngine engine) : base(position)
        {
            this.engine = engine;
            screenShotWindow = new ScreenShotWindow(size);

            Input.Add(Key =>
            {
                if(Key.Key == ConsoleKey.F1)
                {
                    lock(engine.window)
                    {
                        oldWindow = engine.window;
                        engine.SetWindow(screenShotWindow);
                        triggered = true;
                    }
                }
            });
        }

        public override void DrawTo(BaseLayer layer)
        {
            return;
        }

        public override Vec2i GetSize()
        {
            return new Vec2i();
        }

        public override void PreUpdate(float deltaT)
        {

        }

        public override void Update(float deltaT)
        {
            if(triggered)
            {
                if (reset)
                {
                    lock(oldWindow)
                    {
                        engine.SetWindow(oldWindow);
                        screenShotWindow.SaveBitmap("test.png");
                        triggered = false;
                        reset = false;
                    }
                }

                reset = true;
            }
        }
    }
}
