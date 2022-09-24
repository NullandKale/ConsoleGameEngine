using ConsoleGameEngine.Entities;
using ConsoleGameEngine.Layers;
using ConsoleGameEngine.Window;
using System.Diagnostics;

namespace ConsoleGameEngine
{
    public abstract class BaseEngine
    {
        public bool isRunning = false;
        protected volatile bool run = false;
        protected volatile bool pause = false;

        protected Stopwatch timer;
        protected Thread thread;

        public IWindow window { get; protected set; }

        private Dictionary<int, HashSet<Entity>> entityList;
        protected List<BaseLayer> layers;

        protected BaseEngine(IWindow window)
        {
            this.window = window;

            thread = new Thread(threadAction);
            thread.IsBackground = true;

            timer = new Stopwatch();

            entityList = new Dictionary<int, HashSet<Entity>>();
            layers = new List<BaseLayer>();
        }

        public void AddEntity(int layer, Entity entity)
        {
            if(entityList.ContainsKey(layer))
            {
                entityList[layer].Add(entity);
            }
            else
            {
                HashSet<Entity> list = new HashSet<Entity>();
                list.Add(entity);

                entityList.Add(layer, list);
            }
        }

        public void RemoveEntity(int layer, Entity entity) { }

        private void threadAction()
        {
            float deltaT = 0;

            while(run)
            {
                timer.Start();

                PreUpdate(deltaT);

                if(!pause)
                {
                    Update(deltaT);
                }

                Draw();

                timer.Stop();
                deltaT = (float)timer.Elapsed.TotalMilliseconds;
                timer.Reset();
            }

            isRunning = false;
        }

        public virtual void Start()
        {
            run = true;
            pause = false;
            thread.Start();
            isRunning = true;
        }

        public virtual void PreUpdate(float deltaT)
        {
            foreach(KeyValuePair<int, HashSet<Entity>> kvp in entityList)
            {
                foreach(Entity entity in kvp.Value)
                {
                    entity.PreUpdate(deltaT);
                }
            }
        }

        public virtual void Update(float deltaT)
        {
            foreach (KeyValuePair<int, HashSet<Entity>> kvp in entityList)
            {
                foreach (Entity entity in kvp.Value)
                {
                    entity.Update(deltaT);
                    entity.DrawTo(GetLayer(kvp.Key));
                }
            }
        }

        public virtual void Draw()
        {
            for (int i = 0; i < layers.Count; i++)
            {
                window.DrawLayer(layers[i]);
            }
        }

        public int AddLayer(BaseLayer layer)
        {
            int id = layers.Count;
            layers.Add(layer);
            return id;
        }

        public BaseLayer GetLayer(int layer)
        {
            return layers[layer];
        }

    }
}
