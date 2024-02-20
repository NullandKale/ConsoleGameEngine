using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceTrader.ShipSystems.Sensors
{
    public class StandardSensors : SensorsSystem
    {
        public StandardSensors(Ship ship, int statusPositionOffsetY) : base(ship, statusPositionOffsetY)
        {
            SensorDistance = 1000;
        }
    }
}
