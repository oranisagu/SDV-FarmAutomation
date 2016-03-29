using Microsoft.Xna.Framework;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using StardewModdingAPI.Events;
using StardewValley.Objects;
using Object = StardewValley.Object;

namespace FarmAutomation.Common
{
    public static class ItemFinder
    {
        public static List<string> ConnectorItems { get; set; }

        public static IEnumerable<Object> FindItemsofType(GameLocation location, Type itemType)
        {
            return location?.objects.Values.Where(o => o.GetType() == itemType);
        }

        public static IEnumerable<Object> FindItemsWithHeldObjects(GameLocation location)
        {
            return location?.objects.Values.Where(o => o.heldObject != null);
        }

        public static IEnumerable<KeyValuePair<Vector2, Object>> FindObjectsWithName(GameLocation location, IEnumerable<string> names)
        {
            return location?.objects.Where(o => names.Contains(o.Value.Name));
        }


        public static Chest FindChestInLocation(GameLocation location)
        {
            if (location == null)
            {
                return null;
            }
            return FindItemsofType(location, typeof(Chest)).FirstOrDefault() as Chest;
        }

        public static Chest FindConnectedChests(Object item, GameLocation location, List<Vector2> processedLocations = null)
        {
            var itemLocation = item.TileLocation;
            if (processedLocations == null)
            {
                processedLocations = new List<Vector2>();
            }
            processedLocations.Add(itemLocation);

            var adjecantItems = GetAdjecantItems(location, itemLocation, ConnectorItems).ToList();
            foreach (var adjecantItem in adjecantItems)
            {
                if (processedLocations.Contains(adjecantItem.Key))
                {
                    continue;
                }
                var chest = adjecantItem.Value as Chest;
                if (chest != null)
                {
                    return chest;
                }
                chest = FindConnectedChests(adjecantItem.Value, location, processedLocations);
                if (chest != null)
                {
                    return chest;
                }
            }
            return null;
        }

        public static IEnumerable<KeyValuePair<Vector2, Object>> GetAdjecantItems(GameLocation location, Vector2 startPosition, List<string> itemNamesToConsider = null )
        {
            var locations = new List<Vector2>();
            for (int x = -1; x <= 1; ++x)
            {
                for (int y = -1; y <= 1; ++y)
                {
                    var newPosition = new Vector2(startPosition.X + x, startPosition.Y + y);
                    if (!location.isTileOnMap(newPosition))
                    {
                        continue;
                    }
                    
                    if (newPosition != startPosition)
                    {
                        locations.Add(newPosition);
                    }
                }
            }
            if (itemNamesToConsider == null)
            {
                return location.objects.Where(o => locations.Contains(o.Key));
            }
            return
                location.objects.Where(o => locations.Contains(o.Key) && itemNamesToConsider.Contains(o.Value.name));
        }

        public static bool HaveConnectorsInInventoryChanged(EventArgsInventoryChanged inventoryChange)
        {
            var changes = inventoryChange.Added.Concat(inventoryChange.QuantityChanged).Concat(inventoryChange.Removed);
            if (changes.Any(i => ConnectorItems.Contains(i.Item.Name)))
            {
                return true;
            }
            return false;
        }
    }
}
