using ConsoleGameEngine.DataStructures;
using ConsoleGameEngine.Layers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGameEngine.Window
{
    public class ScreenShotWindow : IWindow
    {
        Bitmap bitmap;

        public ScreenShotWindow(Vec2i size) : base(size)
        {
            bitmap = new Bitmap(size.x, size.y);
        }

        public override void UpdateSize(Vec2i size)
        {
            bitmap = new Bitmap(size.x, size.y);
            base.UpdateSize(size);
        }

        public void SaveBitmap(string path)
        {
            bitmap.Save(path, ImageFormat.Png);
        }

        public override void DrawLayer(BaseLayer layer)
        {
            for(int i = 0; i < size.x; i++)
            {
                for(int  j = 0; j < size.y; j++)
                {
                    Vec2i pos = new Vec2i(i, j) + layer.position;

                    if(pos.IsWithin(size))
                    {
                        Chexel? c = layer.ReadChexel(new Vec2i(i, j));
                        if(c != null)
                        {
                            Color color = ((c.Value.background + c.Value.foreground) / 2f);
                            bitmap.SetPixel(pos.x, pos.y, color);
                        }
                    }
                }
            }


        }
    }
}
