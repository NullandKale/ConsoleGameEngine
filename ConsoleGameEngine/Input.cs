using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGameEngine
{
    public static class Input
    {
        public static HashSet<Action<ConsoleKeyInfo>> EventListeners = new HashSet<Action<ConsoleKeyInfo>>();

        public static void Add(Action<ConsoleKeyInfo> toAdd)
        {
            EventListeners.Add(toAdd);
        }

        public static void Remove(Action<ConsoleKeyInfo> toRemove) 
        {
            EventListeners.Remove(toRemove);
        }

        public static void Reset()
        {
            EventListeners.Clear();
        }

        public static void UpdateEventListners()
        {
            while (Console.KeyAvailable)
            {
                ConsoleKeyInfo key = Console.ReadKey();
                foreach(Action<ConsoleKeyInfo> listener in EventListeners)
                {
                    listener(key);
                }
            }
        }

    }
}
