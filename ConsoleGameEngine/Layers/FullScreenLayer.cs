using ConsoleGameEngine.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGameEngine.Layers
{
    public class FullScreenLayer : Layer
    {
        public FullScreenLayer(Vec2i position, Vec2i size) : base(position, size)
        {
        }

        public override void Resize(Vec2i size)
        {
            SetSize(size);
        }
    }
}
