using FarmAutomation.Common.Interfaces;
using Microsoft.Xna.Framework;
using StardewValley;

namespace FarmAutomation.Common
{
    public class LocationHelper : ILocationHelper
    {
        public string GetName(GameLocation location)
        {
            return location.uniqueName ?? location.Name;
        }

        public bool IsTileOnMap(GameLocation location, Vector2 position)
        {
            if (location.Objects.ContainsKey(position))
            {
                return true;
            }
            return location.terrainFeatures.ContainsKey(position);
        }
    }
}
