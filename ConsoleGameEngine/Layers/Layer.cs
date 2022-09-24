using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using ConsoleGameEngine.DataStructures;

namespace ConsoleGameEngine.Layers
{
    public class Layer : BaseLayer
    {
        private Vec3[,] backgroundColors;
        private Vec3[,] foregroundColors;
        private char[,] characters;

        public Layer(Vec2i position, Vec2i size) : base(position, size)
        {
            backgroundColors = new Vec3[size.x, size.y];
            foregroundColors = new Vec3[size.x, size.y];
            characters = new char[size.x, size.y];

        }

        public override void Clear(Chexel clear)
        {
            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    characters[x, y] = clear.character;
                    foregroundColors[x, y] = clear.foreground;
                    backgroundColors[x, y] = clear.background;
                }
            }
        }

        protected override Chexel Read(Vec2i pos)
        {
            return new Chexel(characters[pos.x, pos.y], foregroundColors[pos.x, pos.y], backgroundColors[pos.x, pos.y]);
        }

        public override void SetSize(Vec2i size, Chexel clear)
        {
            backgroundColors = new Vec3[size.x, size.y];
            foregroundColors = new Vec3[size.x, size.y];
            characters = new char[size.x, size.y];

            base.SetSize(size, clear);
        }

        protected override void Write(Chexel toWrite, Vec2i pos)
        {
            characters[pos.x, pos.y] = toWrite.character;
            foregroundColors[pos.x, pos.y] = toWrite.foreground;
            backgroundColors[pos.x, pos.y] = toWrite.background;
        }
    }

    public abstract class BaseLayer
    {
        public Vec2i position;
        public Vec2i size;

        protected BaseLayer(Vec2i position, Vec2i size)
        {
            this.position = position;
            this.size = size;
        }

        public virtual Vec2i GetSize()
        {
            return size;
        }

        public virtual void SetSize(Vec2i size, Chexel clear)
        {
            this.size = size;

            Clear(clear);
        }

        public virtual Vec2i GetPosition()
        {
            return position;
        }

        public virtual void SetPosition(Vec2i position)
        {
            this.position = position;
        }

        public abstract void Clear(Chexel clear);

        protected abstract Chexel Read(Vec2i pos);

        public Chexel ReadUnsafe(Vec2i pos)
        {
            return Read(pos);
        }

        public virtual Chexel? ReadChexel(Vec2i pos)
        {
            if(pos.IsWithin(size.x, size.y))
            {
                return Read(pos);
            }
            else
            {
                return null;
            }
        }

        protected abstract void Write(Chexel toWrite, Vec2i pos);

        public virtual void WriteChexel(Chexel toWrite, Vec2i pos)
        {
            if (pos.IsWithin(size.x, size.y))
            {
                Write(toWrite, pos);
            }
        }

        public virtual void Write(ReadOnlySpan<char> characters, Vec3 foreground, Vec3 background, Vec2i pos)
        {
            for (int i = 0; i < characters.Length; i++)
            {
                Vec2i newPos = pos.Translate(new Vec2i(i, 0));
                if (newPos.IsWithin(size.x, size.y))
                {
                    Write(new Chexel(characters[i], foreground, background), newPos);
                }
            }
        }

        public virtual void Write(ReadOnlySpan<char> characters, ReadOnlySpan<Vec3> foreground, Vec3 background, Vec2i pos)
        {
            for (int i = 0; i < characters.Length; i++)
            {
                Vec2i newPos = pos.Translate(new Vec2i(i, 0));
                if (newPos.IsWithin(size.x, size.y))
                {
                    Write(new Chexel(characters[i], foreground[i], background), newPos);
                }
            }
        }

        public virtual void Write(ReadOnlySpan<char> characters, ReadOnlySpan<Vec3> foreground, ReadOnlySpan<Vec3> background, Vec2i pos)
        {
            for (int i = 0; i < characters.Length; i++)
            {
                Vec2i newPos = pos.Translate(new Vec2i(i, 0));
                if (newPos.IsWithin(size.x, size.y))
                {
                    Write(new Chexel(characters[i], foreground[i], background[i]), newPos);
                }
            }
        }
    }
}
