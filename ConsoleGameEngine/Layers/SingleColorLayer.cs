using ConsoleGameEngine.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGameEngine.Layers
{
    public class SingleColorLayer : BaseLayer
    {
        public Vec3 backgroundColor { get; private set; }
        public Vec3 foregroundColor { get; private set; }
        private char[,] chars;

        public SingleColorLayer(Vec2i position, Vec2i size, Vec3 backgroundColor, Vec3 foregroundColor) : base(position, size)
        {
            this.backgroundColor = backgroundColor;
            this.foregroundColor = foregroundColor;
            chars = new char[size.x, size.y];
        }

        public override void SetSize(Vec2i size, Chexel clear)
        {
            chars = new char[size.x, size.y];
            base.SetSize(size, clear);
        }

        public override void Clear(Chexel clear)
        {
            backgroundColor = clear.background;
            foregroundColor = clear.foreground;

            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    chars[x, y] = clear.character;
                }
            }
        }

        protected override Chexel Read(Vec2i pos)
        {
            return new Chexel(chars[pos.x, pos.y], foregroundColor, backgroundColor);
        }

        protected override void Write(Chexel toWrite, Vec2i pos)
        {
            chars[pos.x, pos.y] = toWrite.character;
        }
    }
}
