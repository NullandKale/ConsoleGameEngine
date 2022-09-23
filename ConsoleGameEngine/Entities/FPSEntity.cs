﻿using ConsoleGameEngine.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGameEngine.Entities
{
    public class FPSEntity : Entity
    {
        private float deltaT = 0;

        public FPSEntity(Vec2i position) : base(position)
        {
        }

        public override void DrawTo(Layer layer)
        {
            layer.Write("" + (int)deltaT + "MS", new Vec3(1, 1, 1), new Vec3(), position);
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