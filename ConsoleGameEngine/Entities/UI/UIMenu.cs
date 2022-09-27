using ConsoleGameEngine.DataStructures;
using ConsoleGameEngine.Layers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGameEngine.Entities.UI
{
    public class UIMenu : Entity
    {
        Vec2i size = new Vec2i();
        bool sizeInvalid = true;

        List<Entity> lines;
        int selected = 0;

        public UIMenu(Vec2i position) : base(position)
        {
            lines = new List<Entity>();

            Input.Add(OnKey);
        }

        public void AddEntity(Entity toAdd)
        {
            toAdd.position = position + new Vec2i(0, lines.Count);
            lines.Add(toAdd);
        }

        public void OnKey(ConsoleKeyInfo key)
        {
            switch (key.Key)
            {
                case ConsoleKey.UpArrow:
                    {
                        if(selected > 0)
                        {
                            selected--;
                        }
                        break;
                    }
                case ConsoleKey.DownArrow:
                    {
                        if (selected < lines.Count)
                        {
                            selected++;
                        }
                        break;
                    }
            }
        }

        public override void DrawTo(BaseLayer layer)
        {
            for (int i = 0; i < lines.Count; i++)
            {
                Entity line = lines[i];
                line.DrawTo(layer);
            }
        }

        public override void PreUpdate(float deltaT)
        {
            for (int i = 0; i < lines.Count; i++)
            {
                Entity line = lines[i];
                line.PreUpdate(deltaT);
            }
        }

        public override void Update(float deltaT)
        {
            for (int i = 0; i < lines.Count; i++)
            {
                Entity line = lines[i];
                line.enabled = selected == i;
                line.Update(deltaT);
            }
        }
        public override Vec2i GetSize()
        {
            if(sizeInvalid)
            {
                Vec2i newSize = new Vec2i();

                for(int i = 0; i < lines.Count; i++)
                {
                    Entity line = lines[i];
                    Vec2i lineSize = line.GetSize();
                    newSize.x += lineSize.x;
                    if(lineSize.y > newSize.y)
                    {
                        newSize.y = lineSize.y;
                    }
                }

                size = newSize;
                return size;
            }
            else
            {
                return size;
            }
        }
    }
}
