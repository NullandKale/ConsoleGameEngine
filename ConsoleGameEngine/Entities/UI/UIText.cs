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
        public string text;
        Vec3 foreground;
        Vec3 background;

        Action<UIText, ConsoleKeyInfo>? onCharPressed;

        public UIText(Vec2i position, string text, Vec3 foreground, Vec3 background, Action<UIText, ConsoleKeyInfo> onCharPressed) : base(position)
        {
            this.text = text;
            this.foreground = foreground;
            this.background = background;
            enabled = true;
            this.onCharPressed = onCharPressed;

            Input.Add((Key) =>
            {
                this.onCharPressed(this, Key);
            });
        }

        public UIText(Vec2i position, string text, Vec3 foreground, Vec3 background) : base(position)
        {
            this.text = text;
            this.foreground = foreground;
            this.background = background;
            enabled = true;
            onCharPressed = null;
        }

        public override void DrawTo(BaseLayer layer)
        {
            if(enabled)
            {
                layer.Write(text, foreground, background, position);
            }
            else
            {
                layer.Write(text, background, foreground, position);
            }
        }

        public override void PreUpdate(float deltaT)
        {

        }

        public override void Update(float deltaT)
        {

        }

        public override Vec2i GetSize()
        {
            return new Vec2i(0, text.Length);
        }
    }
}
