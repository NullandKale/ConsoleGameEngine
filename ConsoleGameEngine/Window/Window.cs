using ConsoleGameEngine.DataStructures;
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

        protected List<Layer> layers;

        protected IWindow(int width, int height)
        {
            this.width = width;
            this.height = height;

            layers = new List<Layer>();
        }

        public int AddLayer(Layer layer)
        {
            int id = layers.Count;
            layers.Add(layer);
            return id;
        }


        public virtual void UpdateSize(int width, int height)
        {
            this.width = width;
            this.height= height;
        }

        public virtual void Draw()
        {
            for(int i = 0; i < layers.Count; i++)
            {
                DrawLayer(i);
            }
        }

        public Layer GetLayer(int layer)
        {
            return layers[layer];
        }

        public abstract void DrawLayer(int toDraw);
    }
}
