using System;
using Microsoft.Xna.Framework;
using StardewValley;

namespace FarmAutomation.Common.Interfaces
{
    public interface ILocationHelper
    {
        String GetName(GameLocation location);
        bool IsTileOnMap(GameLocation location, Vector2 position);
    }
}