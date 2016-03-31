using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using StardewValley;

namespace FarmAutomation.Common
{
    public class LocationHelper
    {
        public static String GetName(GameLocation location)
        {
            return location.uniqueName ?? location.Name;
        }

        public static bool IsTileOnMap(GameLocation location, Vector2 position)
        {
            if (location.Objects.ContainsKey(position))
            {
                return true;
            }
            if (location.terrainFeatures.ContainsKey(position))
            {
                return true;
            }
            //if (position.X >= 0.0 && position.X <= location.map.Layers[0].LayerWidth && position.Y >= 0.0)
            //    return position.Y <= location.map.Layers[0].LayerHeight;
            return false;
    }
    }
}
