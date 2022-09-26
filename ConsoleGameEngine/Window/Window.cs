using ConsoleGameEngine.DataStructures;
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
        public Vec2i size { get; private set; }
        private Action<Vec2i>? requestSizeUpdate;

        protected IWindow(Vec2i size)
        {
            this.size = size;
        }

        public virtual void SetOnWindowSizeUpdate(Action<Vec2i> requestSizeUpdate)
        {
            this.requestSizeUpdate = requestSizeUpdate;
        }

        public virtual void RequestSizeUpdate(Vec2i newSize)
        {
            if(requestSizeUpdate != null) 
            {
                requestSizeUpdate(newSize);
            }
        }

        public virtual void UpdateWindow()
        {

        }

        public virtual void UpdateSize(Vec2i size)
        {
            this.size = size;
        }

        public abstract void DrawLayer(BaseLayer layer);
    }
}
