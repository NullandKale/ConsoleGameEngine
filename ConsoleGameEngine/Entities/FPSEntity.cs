using ConsoleGameEngine.DataStructures;
using ConsoleGameEngine.Layers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGameEngine.Entities
{
    public class FPSEntity : Entity
    {
        private string text = "";
        private float deltaT = 0;

        public FPSEntity(Vec2i position) : base(position)
        {

        }

        public override void DrawTo(BaseLayer layer)
        {
            layer.Write("" + (int)deltaT + "MS", new Vec3(1, 1, 1), new Vec3(), position);
        }

        public override Vec2i GetSize()
        {
            return new Vec2i(1, text.Length);
        }

        public override void PreUpdate(float deltaT)
        {
            this.deltaT = deltaT;
        }

        public override void Update(float deltaT)
        {

        }
    }
}
