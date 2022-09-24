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
        public Vec2i position;
        protected Entity(Vec2i position) 
        {
            this.position = position;
        }

        public abstract void PreUpdate(float deltaT);
        public abstract void Update(float deltaT);
        public abstract void DrawTo(BaseLayer layer);
    }
}
