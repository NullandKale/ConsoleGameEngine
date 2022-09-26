using ConsoleGameEngine.DataStructures;
using ConsoleGameEngine.Layers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGameEngine.Entities.UI
{
    public class UIText : Entity
    {
        public bool enabled;
        string text;
        Vec3 foreground;
        Vec3 background;

        public UIText(Vec2i position, string text, Vec3 foreground, Vec3 background) : base(position)
        {
            this.text = text;
            this.foreground = foreground;
            this.background = background;
            enabled = true;
        }

        public UIText(Vec2i position, string text, Vec3 foreground, Vec3 background, Func<ConsoleKeyInfo, string?> onKeyPressed) : base(position)
        {
            this.text = text;
            this.foreground = foreground;
            this.background = background;
            enabled = true;

            Input.Add((Key) =>
            {
                if(enabled)
                {
                    string? text = onKeyPressed(Key);

                    if (text != null)
                    {
                        this.text = text;
                    }
                }
            });
        }


        public override void DrawTo(BaseLayer layer)
        {
            layer.Write(text, foreground, background, position);
        }

        public override void PreUpdate(float deltaT)
        {

        }

        public override void Update(float deltaT)
        {

        }
    }
}
