using ConsoleGameEngine.DataStructures;
using ConsoleGameEngine.Layers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGameEngine.Window
{
    public enum ConsolePalette
    {
        BitBased,
        SudoHSV,
        NameSearch,
        GreyScale,
    }

    public class ConsoleWindow : IWindow
    {
        public ConsolePalette palette = ConsolePalette.BitBased;
        public Vec2i previousSize;

        public ConsoleWindow(ConsolePalette palette) : base(new Vec2i(Console.WindowWidth - 1, Console.WindowHeight))
        {
            this.palette = palette;
        }

        private ConsoleColor ColorConvert(Vec3 c, ConsolePalette palette)
        {
            float max = Math.Max(c.x, Math.Max(c.y, c.z));
            if (max > 1)
            {
                c = c * (1.0f / max);
            }

            int r = (int)(Math.Max(0.0, Math.Min(1, c.x)) * 255.0);
            int g = (int)(Math.Max(0.0, Math.Min(1, c.y)) * 255.0);
            int b = (int)(Math.Max(0.0, Math.Min(1, c.z)) * 255.0);

            Color color = Color.FromArgb(r, g, b);

            switch (palette)
            {
                case ConsolePalette.BitBased:
                    {
                        int index = (r > 128 | g > 128 | b > 128) ? 8 : 0; // Bright bit

                        index |= (r > 64) ? 4 : 0; // Red bit
                        index |= (g > 64) ? 2 : 0; // Green bit
                        index |= (b > 64) ? 1 : 0; // Blue bit

                        return (ConsoleColor)index;
                    }
                case ConsolePalette.SudoHSV:
                    {
                        if (color.GetSaturation() < 0.5)
                        {
                            // we have a grayish color
                            switch ((int)(color.GetBrightness() * 3.5))
                            {
                                case 0: return ConsoleColor.Black;
                                case 1: return ConsoleColor.DarkGray;
                                case 2: return ConsoleColor.Gray;
                                default: return ConsoleColor.White;
                            }
                        }
                        int hue = (int)Math.Round(color.GetHue() / 60, MidpointRounding.AwayFromZero);
                        if (color.GetBrightness() < 0.4)
                        {
                            // dark color
                            switch (hue)
                            {
                                case 1: return ConsoleColor.DarkYellow;
                                case 2: return ConsoleColor.DarkGreen;
                                case 3: return ConsoleColor.DarkCyan;
                                case 4: return ConsoleColor.DarkBlue;
                                case 5: return ConsoleColor.DarkMagenta;
                                default: return ConsoleColor.DarkRed;
                            }
                        }
                        // bright color
                        switch (hue)
                        {
                            case 1: return ConsoleColor.Yellow;
                            case 2: return ConsoleColor.Green;
                            case 3: return ConsoleColor.Cyan;
                            case 4: return ConsoleColor.Blue;
                            case 5: return ConsoleColor.Magenta;
                            default: return ConsoleColor.Red;
                        }
                    }
                case ConsolePalette.NameSearch:
                    {
                        ConsoleColor ret = 0;
                        double rr = r, gg = g, bb = b, delta = double.MaxValue;

                        foreach (ConsoleColor cc in Enum.GetValues(typeof(ConsoleColor)))
                        {
                            string? n = Enum.GetName(typeof(ConsoleColor), cc);
                            Color c1 = Color.FromName(n == "DarkYellow" ? "Orange" : n); // bug fix
                            double t = Math.Pow(c1.R - rr, 2.0) + Math.Pow(c1.G - gg, 2.0) + Math.Pow(c1.B - bb, 2.0);
                            if (t == 0.0)
                                return cc;
                            if (t < delta)
                            {
                                delta = t;
                                ret = cc;
                            }
                        }
                        return ret;
                    }
                case ConsolePalette.GreyScale:
                    {
                        switch ((int)(color.GetBrightness() * 7))
                        {
                            case 0: return ConsoleColor.Black;
                            case 1: return ConsoleColor.DarkGray;
                            case 2: return ConsoleColor.Gray;
                            default: return ConsoleColor.White;
                        }
                    }
                default:
                    {
                        return ConsoleColor.Black;
                    }
            }
        }

        private void FastDrawSingleColorLayer(SingleColorLayer layer)
        {
            Console.ForegroundColor = ColorConvert(layer.foregroundColor, palette);
            Console.BackgroundColor = ColorConvert(layer.backgroundColor, palette);

            Vec2i currentPosition = layer.position;

            char[] toPrint = new char[layer.size.x];

            for (int y = 0; y < layer.size.y; y++)
            {
                if (currentPosition.IsWithin(size))
                {
                    Console.SetCursorPosition(currentPosition.x, currentPosition.y);
                }

                for (int x = 0; x < layer.size.x; x++)
                {
                    if (currentPosition.IsWithin(size))
                    {
                        Chexel chexel = layer.ReadUnsafe(currentPosition);
                        toPrint[x] = chexel.character;
                    }
                    currentPosition.x++;
                }

                Console.Write(toPrint);
                currentPosition.x = layer.position.x;
                currentPosition.y++;
            }
        }

        private void DefaultDrawLayer(BaseLayer layer)
        {
            ConsoleColor currentForeground = Console.ForegroundColor;
            ConsoleColor currentBackground = Console.BackgroundColor;

            Vec2i currentPosition = layer.position;

            for (int y = 0; y < layer.size.y; y++)
            {
                currentPosition.x = layer.position.x;

                if (currentPosition.IsWithin(size))
                {
                    Console.SetCursorPosition(currentPosition.x, currentPosition.y);
                }

                for (int x = 0; x < layer.size.x; x++)
                {
                    if (currentPosition.IsWithin(size))
                    {
                        Chexel chexel = layer.ReadUnsafe(currentPosition);

                        ConsoleColor foreground = ColorConvert(chexel.foreground, palette);
                        ConsoleColor background = ColorConvert(chexel.background, palette);

                        if (currentForeground != foreground)
                        {
                            Console.ForegroundColor = foreground;
                            currentForeground = foreground;
                        }

                        if (currentBackground != background)
                        {
                            Console.BackgroundColor = background;
                            currentBackground = background;
                        }

                        Console.Write(chexel.character);
                    }

                    currentPosition.x++;
                }

                currentPosition.y++;
            }
        }

        public override void DrawLayer(BaseLayer layer)
        {
            if (layer is SingleColorLayer)
            {
                FastDrawSingleColorLayer((SingleColorLayer)layer);
            }
            else
            {
                DefaultDrawLayer(layer);
            }
        }

        public override void UpdateWindow()
        {
            int currentWidth = Console.WindowWidth;
            int currentHeight = Console.WindowHeight - 1;

            if(currentWidth != size.x || currentHeight != size.y)
            {
                RequestSizeUpdate(new Vec2i(currentWidth, currentHeight));
            }
        }
    }
}
