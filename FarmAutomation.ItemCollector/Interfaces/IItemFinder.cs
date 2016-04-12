using System.Collections.Generic;
using Microsoft.Xna.Framework;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Objects;

namespace FarmAutomation.Common.Interfaces
{
    public interface IItemFinder
    {
        Chest FindChestInLocation(GameLocation location);

        void FindConnectedLocations(GameLocation location, Vector2 startPosition, List<ConnectedTile> processedLocations);

        bool HaveConnectorsInInventoryChanged(EventArgsInventoryChanged inventoryChange);
        IEnumerable<KeyValuePair<Vector2, Object>>  FindObjectsWithName(GameLocation gameLocation, List<string> machineNamesToProcess);
    }
}