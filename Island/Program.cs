using ConsoleGameEngine;
using ConsoleGameEngine.DataStructures;
using ConsoleGameEngine.Entities.UI;
using ConsoleGameEngine.Entities.Utilities;
using ConsoleGameEngine.Window;
using Island.Agents;
using System.Drawing;

namespace Island
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Sample sample = new Sample();

            sample.Start();

            while (sample.isRunning)
            {
                Thread.Sleep(100);
            }
        }
    }

    public class Sample : BaseEngine
    {
        // test backdrop
        public Vec2i windowOffset;
        public Vec2i windowCenter;

        public Ground layer;

        public AgentManager agentManager;
        public Agent agent;

        public Sample() : base(new ConsoleWindow(ConsolePalette.BitBased))
        {
            layer = new Ground(this, new Vec2i(), window.size);
            layer.SetClear(new Chexel(' ', new Vec3(1, 1, 1), new Vec3(0, 0, 0)));

            agentManager = new AgentManager(this);

            // add player
            agent = new Agent(agentManager, new Vec2i(0, 0), true);
            agentManager.AddAgent(agent);

            for(int i = 0; i < 150; i++)
            {
                agentManager.SpawnRandomAgent(windowOffset, window.size);
            }

            AddLayer(layer);

            Input.Add(OnKey);
        }

        private void OnKey(ConsoleKeyInfo key)
        {
            switch(key.Key)
            {
                case ConsoleKey.Enter:
                    agentManager.SpawnRandomAgent(windowOffset, window.size);
                    break;
                case ConsoleKey.Escape:
                    run = false;
                    break;
                case ConsoleKey.UpArrow:
                    windowOffset.y -= 1;
                    break;
                case ConsoleKey.DownArrow:
                    windowOffset.y += 1;
                    break;
                case ConsoleKey.LeftArrow:
                    windowOffset.x -= 1;
                    break;
                case ConsoleKey.RightArrow:
                    windowOffset.x += 1;
                    break;
                case ConsoleKey.W:
                    agent.velocity.y -= 1;
                    break;
                case ConsoleKey.S:
                    agent.velocity.y += 1;
                    break;
                case ConsoleKey.A:
                    agent.velocity.x -= 1;
                    break;
                case ConsoleKey.D:
                    agent.velocity.x += 1;
                    break;
            }
        }

        protected override void Update(float deltaT)
        {
            windowCenter = windowOffset - (window.size / 2);
            layer.SetOffset(windowOffset);
            agentManager.SetOffset(windowOffset);

            agentManager.Update(deltaT);

            // updates all entities and draws
            base.Update(deltaT);
        }
    }
}