using ConsoleGameEngine.Entities.UI;
using System;
using System.Collections.Generic;

namespace SpaceTrader
{
    internal class Program
    {
        static void Main(string[] args)
        {
            SpaceTraderEngine sample = new SpaceTraderEngine();

            sample.Start();

            while (sample.isRunning)
            {
                Thread.Sleep(100);
            }
        }
    }
}
