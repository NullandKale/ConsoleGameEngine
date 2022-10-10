using ConsoleGameEngine.DataStructures;
using ConsoleGameEngine.Entities;
using ConsoleGameEngine.Layers;
using ConsoleGameEngine.Window;
using System.Diagnostics;
using System.Drawing;

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
            window.SetOnWindowSizeUpdate(UpdateSize);

            thread = new Thread(threadAction);
            thread.IsBackground = true;

            timer = new Stopwatch();

            entityList = new Dictionary<int, HashSet<Entity>>();
            layers = new List<BaseLayer>();
        }

        public void UpdateSize(Vec2i size)
        {
            window.UpdateSize(size);

            foreach(var layer in layers)
            {
                layer.SetSize(size);
            }
        }

        public void SetWindow(IWindow window)
        {
            lock(this.window)
            {
                this.window = window;
                UpdateSize(window.size);
            }
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

        public void RemoveEntity(int layer, Entity entity) 
        {
            if(entityList.ContainsKey(layer))
            {
                entityList[layer].Remove(entity);
                if (entityList[layer].Count == 0)
                {
                    entityList.Remove(layer);
                }
            }
        }

        private void threadAction()
        {
            float deltaT = 0;

            while(run)
            {
                timer.Start();

                ClearLayers();

                Input.UpdateEventListners();

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

        protected virtual void ClearLayers()
        {
            for(int i = 0; i < layers.Count; i++)
            {
                layers[i].Clear();
            }
        }

        protected virtual void PreUpdate(float deltaT)
        {
            foreach(KeyValuePair<int, HashSet<Entity>> kvp in entityList)
            {
                HashSet<Entity> entities = new HashSet<Entity>(kvp.Value);
                foreach(Entity entity in entities)
                {
                    entity.PreUpdate(deltaT);
                }
            }
        }

        protected virtual void Update(float deltaT)
        {
            window.UpdateWindow();

            foreach (KeyValuePair<int, HashSet<Entity>> kvp in entityList)
            {
                foreach (Entity entity in kvp.Value)
                {
                    entity.Update(deltaT);
                    entity.DrawTo(GetLayer(kvp.Key));
                }
            }
        }

        protected virtual void Draw()
        {
            lock(window)
            {
                for (int i = 0; i < layers.Count; i++)
                {
                    window.DrawLayer(layers[i]);
                }
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
