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
        List<UIText> lines;
        int selected = 0;

        public UIMenu(Vec2i position) : base(position)
        {
            lines = new List<UIText>();
        }

        public UIMenu(Vec2i position, List<UIText> lines) : base(position)
        {
            this.lines = lines;
        }

        public override void DrawTo(BaseLayer layer)
        {
            for (int i = 0; i < lines.Count; i++)
            {
                UIText line = lines[i];
                line.DrawTo(layer);
            }
        }

        public override void PreUpdate(float deltaT)
        {
            for (int i = 0; i < lines.Count; i++)
            {
                UIText line = lines[i];
                line.PreUpdate(deltaT);
            }
        }

        public override void Update(float deltaT)
        {
            for (int i = 0; i < lines.Count; i++)
            {
                UIText line = lines[i];
                line.enabled = selected == i;
                line.Update(deltaT);
            }
        }
    }
}
