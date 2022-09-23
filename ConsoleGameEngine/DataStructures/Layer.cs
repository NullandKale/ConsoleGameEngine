using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGameEngine.DataStructures
{
    public struct Layer
    {
        public Vec2i position;
        public Vec2i size;

        private Vec3[,] backgroundColors;
        private Vec3[,] foregroundColors;
        private char[,] characters;

        public Layer(Vec2i position, Vec2i size)
        {
            this.position = position;
            this.size = size;

            backgroundColors = new Vec3[size.x, size.y];
            foregroundColors = new Vec3[size.x, size.y];
            characters = new char[size.x, size.y];
        }

        public void Clear(Chexel clear)
        {
            for(int x = 0; x < size.x; x++)
            {
                for(int y = 0; y < size.y; y++)
                {
                    characters[x, y] = clear.character;
                    foregroundColors[x, y] = clear.foreground;
                    backgroundColors[x, y] = clear.background;
                }
            }
        }

        public Chexel Read(Vec2i pos)
        {
            return new Chexel(characters[pos.x, pos.y], foregroundColors[pos.x, pos.y], backgroundColors[pos.x, pos.y]);
        }

        public void Write(Chexel toWrite, Vec2i pos)
        {
            if(pos.IsWithin(size.x, size.y))
            {
                characters[pos.x, pos.y] = toWrite.character;
                foregroundColors[pos.x, pos.y] = toWrite.foreground;
                backgroundColors[pos.x, pos.y] = toWrite.background;
            }
        }

        public void Write(ReadOnlySpan<char> characters, Vec3 foreground, Vec3 background, Vec2i pos)
        {
            for(int i = 0; i < characters.Length; i++)
            {
                Write(new Chexel(characters[i], foreground, background), pos.Translate(new Vec2i(i, 0)));
            }
        }

        public void Write(ReadOnlySpan<char> characters, ReadOnlySpan<Vec3> foreground, Vec3 background, Vec2i pos)
        {
            for (int i = 0; i < characters.Length; i++)
            {
                Write(new Chexel(characters[i], foreground[i], background), pos.Translate(new Vec2i(i, 0)));
            }
        }

        public void Write(ReadOnlySpan<char> characters, ReadOnlySpan<Vec3> foreground, ReadOnlySpan<Vec3> background, Vec2i pos)
        {
            for (int i = 0; i < characters.Length; i++)
            {
                Write(new Chexel(characters[i], foreground[i], background[i]), pos.Translate(new Vec2i(i, 0)));
            }
        }
    }
}
