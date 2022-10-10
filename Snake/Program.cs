using ConsoleGameEngine;
using ConsoleGameEngine.DataStructures;
using ConsoleGameEngine.Entities;
using ConsoleGameEngine.Layers;
using ConsoleGameEngine.Window;

namespace Snake
{
    public class SnakeEngine : BaseEngine
    {
        public BaseLayer layer;
        public Random random;

        public SnakeEntity? snake;
        public Entity? apple;

        public SnakeEngine() : base(new ConsoleWindow())
        {
            layer = new Layer(new Vec2i(), window.size);
            layer.SetClear(new Chexel(' ', new Vec3(1, 1, 1), new Vec3(0, 0, 0)));

            random = new Random();

            AddLayer(layer);
            AddEntity(0, new FPSEntity(new Vec2i()));

            GameStart();
        }

        public void GameOver()
        {
            if(apple != null)
            {
                RemoveEntity(0, apple);
                apple = null;
            }

            if(snake != null)
            {
                RemoveEntity(0, snake);
                snake = null;
            }
        }

        public void GameStart()
        {
            snake = new SnakeEntity(window.size / 2, new Chexel('#', new Vec3(1, 1, 1), new Vec3(0, 0, 0)), this);
            AddEntity(0, snake);

            SpawnNewApple();
        }

        public void SpawnNewApple()
        {
            if (apple != null)
            {
                RemoveEntity(0, apple);
                apple = null;
            }

            int x = random.Next(0, window.size.x - 2);
            int y = random.Next(0, window.size.y - 2);
            apple = new AppleEntity(new Vec2i(x,y));

            AddEntity(0, apple);
        }
    }

    public class AppleEntity : Entity
    {
        public AppleEntity(Vec2i position) : base(position)
        {
        }

        public override void DrawTo(BaseLayer layer)
        {
            layer.Write("@", new Vec3(1, 0, 0), new Vec3(0.5, 1, 1), position);
        }

        public override Vec2i GetSize()
        {
            return new Vec2i(1, 1);
        }

        public override void PreUpdate(float deltaT)
        {
        }

        public override void Update(float deltaT)
        {
        }
    }

    public class SnakeEntity : Entity
    {
        // for optimal performance this should be an array backed double ended queue
        private List<Vec2i> bodysegments;
        private Chexel body;
        private SnakeEngine engine;
        private const float speed = 50f;
        private Vec2 velocity = new Vec2(0, -speed / 1.8f);
        private Vec2 currentPosition;

        public SnakeEntity(Vec2i position, Chexel body, SnakeEngine engine) : base(position)
        {
            this.currentPosition = (Vec2)position;
            bodysegments = new List<Vec2i>();
            this.body = body;
            this.engine = engine;

            Input.Add(GetInput);
        }

        private void GetInput(ConsoleKeyInfo keypress)
        {
            if(keypress.Key == ConsoleKey.UpArrow)
            {
                velocity = new Vec2(0, -speed / 1.8f);
            }
            else if(keypress.Key == ConsoleKey.DownArrow)
            {
                velocity = new Vec2(0, speed / 1.8f);
            }
            else if(keypress.Key == ConsoleKey.LeftArrow)
            {
                velocity = new Vec2(-speed, 0);
            }
            else if(keypress.Key == ConsoleKey.RightArrow)
            {
                velocity = new Vec2(speed, 0);
            }
        }

        public override void DrawTo(BaseLayer layer)
        {
            layer.WriteChexel(body, position);

            for(int i = 0; i < bodysegments.Count; i++)
            {
                layer.WriteChexel(body, bodysegments[i]);
            }
        }

        public override Vec2i GetSize()
        {
            Vec2i min = engine.window.size;
            Vec2i max = -engine.window.size;

            for(int i = 0; i < bodysegments.Count; i++)
            {
                Vec2i v = bodysegments[i];
                if(v.x < min.x)
                {
                    min.x = v.x;
                }

                if (v.x > max.x)
                {
                    max.x = v.x;
                }

                if (v.y < min.y)
                {
                    min.y = v.y;
                }

                if(v.y > max.y)
                {
                    max.y = v.y;
                }
            }

            return max - min;
        }

        public override void PreUpdate(float deltaT)
        {
            if(engine.apple != null)
            {
                if (position == engine.apple.position)
                {
                    bodysegments.Insert(0, position);
                    engine.SpawnNewApple();
                }
            }
        }

        private void updateHeadPosition(Vec2 newPosition)
        {
            if(position != (Vec2i)newPosition)
            {
                if(bodysegments.Count > 0)
                {
                    for(int i = 0; i < bodysegments.Count - 1; i++)
                    {
                        bodysegments[i] = bodysegments[i + 1];
                    }

                    bodysegments.RemoveAt(bodysegments.Count - 1);
                    bodysegments.Add(position);
                    position = (Vec2i)newPosition;
                }
                else
                {
                    position = (Vec2i)newPosition;
                }
            }
        }

        public override void Update(float deltaT)
        {
            if(!position.IsWithin(engine.window.size - new Vec2i(1, 1)))
            {
                position.Clamp(engine.window.size - new Vec2i(2, 2));
                currentPosition = (Vec2)position;
                velocity = new Vec2();
            }

            currentPosition += velocity * (deltaT / 1000f);
            updateHeadPosition(currentPosition);

        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            SnakeEngine sample = new SnakeEngine();

            sample.Start();

            while (sample.isRunning)
            {
                Thread.Sleep(100);
            }
        }
    }
}