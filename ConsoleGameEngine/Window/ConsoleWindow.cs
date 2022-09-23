using ConsoleGameEngine.DataStructures;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGameEngine.Window
{
    public class ConsoleWindow : IWindow
    {

        public ConsoleWindow() : base(Console.WindowWidth - 1, Console.WindowHeight)
        {
        }

        private static ConsoleColor ColorConvert(Vec3 c, int pallet)
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

            switch (pallet)
            {
                case 0:
                    {
                        int index = (r > 128 | g > 128 | b > 128) ? 8 : 0; // Bright bit

                        index |= (r > 64) ? 4 : 0; // Red bit
                        index |= (g > 64) ? 2 : 0; // Green bit
                        index |= (b > 64) ? 1 : 0; // Blue bit

                        return (ConsoleColor)index;
                    }
                case 1:
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
                case 2:
                    {
                        ConsoleColor ret = 0;
                        double rr = r, gg = g, bb = b, delta = double.MaxValue;

                        foreach (ConsoleColor cc in Enum.GetValues(typeof(ConsoleColor)))
                        {
                            var n = Enum.GetName(typeof(ConsoleColor), cc);
                            var c1 = Color.FromName(n == "DarkYellow" ? "Orange" : n); // bug fix
                            var t = Math.Pow(c1.R - rr, 2.0) + Math.Pow(c1.G - gg, 2.0) + Math.Pow(c1.B - bb, 2.0);
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
                default:
                    {
                        return ConsoleColor.Black;
                    }
            }
        }

        public override void DrawLayer(int toDraw)
        {
            Layer layer = layers[toDraw];

            ConsoleColor currentForeground = Console.ForegroundColor;
            ConsoleColor currentBackground = Console.BackgroundColor;

            Vec2i currentPosition = layer.position;

            for (int y = 0; y < layer.size.y; y++)
            {
                currentPosition.x = layer.position.x;

                if (currentPosition.IsWithin(width, height))
                {
                    Console.SetCursorPosition(currentPosition.x, currentPosition.y);
                }

                for (int x = 0; x < layer.size.x; x++)
                {
                    if(currentPosition.IsWithin(width, height))
                    {
                        Chexel chexel = layer.Read(currentPosition);

                        ConsoleColor foreground = ColorConvert(chexel.foreground, 0);
                        ConsoleColor background = ColorConvert(chexel.background, 0);

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
    }
}
