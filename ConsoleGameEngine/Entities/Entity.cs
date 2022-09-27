using ConsoleGameEngine.DataStructures;
using ConsoleGameEngine.Layers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGameEngine.Entities
{
    public abstract class Entity
    {
        public bool enabled;
        public Vec2i position;
        protected Entity(Vec2i position) 
        {
            this.position = position;
        }

        public abstract Vec2i GetSize();

        public virtual void CenterOn(Vec2i newCenter)
        {
            Vec2i size = GetSize();

            position.x = (size.x / 2) + newCenter.x;
            position.y = (size.y / 2) + newCenter.y;
        }

        public abstract void PreUpdate(float deltaT);
        public abstract void Update(float deltaT);
        public abstract void DrawTo(BaseLayer layer);
    }
}
