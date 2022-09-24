using ConsoleGameEngine.Layers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGameEngine.Window
{
    public abstract class IWindow
    {
        public int width;
        public int height;



        protected IWindow(int width, int height)
        {
            this.width = width;
            this.height = height;
        }

        public virtual void UpdateSize(int width, int height)
        {
            this.width = width;
            this.height= height;
        }

        public abstract void DrawLayer(BaseLayer layer);
    }
}
