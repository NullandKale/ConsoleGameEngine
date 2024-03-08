using ConsoleGameEngine;
using ConsoleGameEngine.DataStructures;
using ConsoleGameEngine.Entities;
using ConsoleGameEngine.Layers;

namespace Island
{
    public class Ground : FullScreenLayer
    {
        public Sample engine;
        public float totalTimeMS = 0;
        public Action<float> OnAspectRatioChanged;
        public Vec2i offset;

        public Ground(Sample engine, Vec2i position, Vec2i size) : base(position, size)
        {
            this.engine = engine;
            this.offset = new Vec2i(0, 0);

            engine.AddEntity(0, new GroundUpdateEntity(this, (deltaT) =>
            {
                totalTimeMS += deltaT;
            }));
        }

        public void SetOffset(Vec2i offset)
        {
            this.offset = offset;
        }

        protected override Chexel Read(Vec2i pos)
        {
            Chexel c = base.Read(pos);

            c.background = new Vec3(0, 1, 0);

            return c;
        }
        private class GroundUpdateEntity : Entity
        {
            Ground ground;
            Action<float> action;

            public GroundUpdateEntity(Ground ground, Action<float> action) : base(new Vec2i())
            {
                this.ground = ground;
                this.action = action;
            }

            public override void Update(float deltaT)
            {
                // do not read key here
            }

            public override void DrawTo(BaseLayer layer)
            {
                string STATUS = $"[{ground.offset.x}, {ground.offset.y}] @ {ground.engine.agentManager.GetTimeString()}";

                Vec2i layerSize = layer.GetSize();
                int maxLength = Math.Min(STATUS.Length, layerSize.x);
                int statusOutputPosition = layerSize.y - 1;
                for (int i = 0; i < maxLength; i++)
                {
                    layer.WriteChexel(new Chexel(STATUS[i], new Vec3(0, 0, 0), new Vec3(0, 0, 0)), new Vec2i(i, statusOutputPosition));
                }
            }

            public override Vec2i GetSize()
            {
                return new Vec2i();
            }

            public override void PreUpdate(float deltaT)
            {
                action.Invoke(deltaT);
            }
        }
    }
}