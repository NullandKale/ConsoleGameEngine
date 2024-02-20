using ConsoleGameEngine.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceTrader.ShipSystems.Sensors
{
    public class SensorsSystem : ShipSystem
    {
        public float SensorDistance;
        private bool isStarSelected = true; // Indicates if the selected body is a star
        private int selectedIndex = 0; // Index of the selected star or planet
        private bool detailMode = false;

        public SensorsSystem(Ship ship, int statusPositionOffsetY)
            : base(ship, statusPositionOffsetY)
        {
        }        

        public override void OnKeyPressed(ConsoleKeyInfo keyPress)
        {
            int starCount = ship.solarSystemData.stars.Count;
            int planetCount = ship.solarSystemData.planets.Count;

            switch (keyPress.Key)
            {
                case ConsoleKey.Q: // Previous body
                    if (isStarSelected)
                    {
                        if (selectedIndex > 0)
                        {
                            selectedIndex--;
                        }
                        else
                        {
                            isStarSelected = false;
                            selectedIndex = planetCount - 1;
                        }
                    }
                    else
                    {
                        if (selectedIndex > 0)
                        {
                            selectedIndex--;
                        }
                        else
                        {
                            isStarSelected = true;
                            selectedIndex = starCount - 1;
                        }
                    }
                    break;
                case ConsoleKey.E: // Next body
                    if (isStarSelected)
                    {
                        if (selectedIndex < starCount - 1)
                        {
                            selectedIndex++;
                        }
                        else
                        {
                            isStarSelected = false;
                            selectedIndex = 0;
                        }
                    }
                    else
                    {
                        if (selectedIndex < planetCount - 1)
                        {
                            selectedIndex++;
                        }
                        else
                        {
                            isStarSelected = true;
                            selectedIndex = 0;
                        }
                    }
                    break;
                case ConsoleKey.Z: // Toggle detail mode
                    detailMode = !detailMode;
                    break;
            }
        }

        public override void Update(float deltaT)
        {
            UpdateStatusBasedOnSelection();
        }

        private void UpdateStatusBasedOnSelection()
        {
            if (!detailMode)
            {
                Vec2i position;
                if (isStarSelected)
                {
                    if (selectedIndex >= 0 && selectedIndex < ship.solarSystemData.stars.Count)
                    {
                        var star = ship.solarSystemData.stars[selectedIndex];
                        position = star.position;
                        Vec2i distanceVector = position - ship.positionF.toVec2i();
                        UpdateStatus($"Ship Position: X:{ship.positionF.x,7:F2}, Y:{ship.positionF.y,7:F2} | Selected Star Distance: X:{distanceVector.x,7:F2}, Y:{distanceVector.y,7:F2}");
                    }
                }
                else
                {
                    if (selectedIndex >= 0 && selectedIndex < ship.solarSystemData.planets.Count)
                    {
                        var planet = ship.solarSystemData.planets[selectedIndex];
                        position = planet.position;
                        Vec2i distanceVector = position - ship.positionF.toVec2i();
                        UpdateStatus($"Ship Position: X:{ship.positionF.x,7:F2}, Y:{ship.positionF.y,7:F2} | Selected Planet Distance: X:{distanceVector.x,7:F2}, Y:{distanceVector.y,7:F2}");
                    }
                }
            }
            else
            {
                // Handle detail mode
                if (isStarSelected && selectedIndex < ship.solarSystemData.stars.Count)
                {
                    var star = ship.solarSystemData.stars[selectedIndex];
                    UpdateStatus($"Star Detail - Class: {star.starClass}, Radius: {star.radius}, Position: X:{star.position.x,7:F2}, Y:{star.position.y,7:F2}, Orbital Speed: {star.orbitalSpeed:F2}");
                }
                else if (!isStarSelected && selectedIndex < ship.solarSystemData.planets.Count)
                {
                    var planet = ship.solarSystemData.planets[selectedIndex];
                    UpdateStatus($"Planet Detail - Class: {planet.planetClass}, Atmosphere: {planet.atmosphereType}, Temp: {planet.averageTemperature:F2}, Position: X:{planet.position.x,7:F2}, Y:{planet.position.y,7:F2}, Orbital Speed: {planet.orbitalSpeed:F2}");
                }
            }
        }

    }

}
