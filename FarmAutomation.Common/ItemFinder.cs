using Microsoft.Xna.Framework;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using FarmAutomation.Common.Interfaces;
using StardewModdingAPI.Events;
using StardewValley.Objects;
using StardewValley.TerrainFeatures;
using Object = StardewValley.Object;

namespace FarmAutomation.Common
{
    public class ItemFinder : IItemFinder
    {
        private readonly IItemFinderConfiguration _config;
        private readonly ILocationHelper _locationHelper;

        public ItemFinder(IItemFinderConfiguration config, ILocationHelper locationHelper)
        {
            _config = config;
            _locationHelper = locationHelper;
        }

        private IEnumerable<Object> FindItemsofType(GameLocation location, Type itemType)
        {
            return location?.objects.Values.Where(o => o.GetType() == itemType);
        }

        public IEnumerable<KeyValuePair<Vector2, Object>> FindObjectsWithName(GameLocation location, List<string> names)
        {
            return location?.objects.Where(o => names.Contains(o.Value.Name));
        }

        public Chest FindChestInLocation(GameLocation location)
        {
            if (location == null)
            {
                return null;
            }
            return FindItemsofType(location, typeof(Chest)).FirstOrDefault() as Chest;
        }

        public void FindConnectedLocations(GameLocation location, Vector2 startPosition,
            List<ConnectedTile> processedLocations)
        {
            var adjecantTiles = GetAdjecantTiles(location, startPosition);
            foreach (var adjecantTile in adjecantTiles.Where(t => processedLocations.All(l => l.Location != t)))
            {
                Object item = null;
                if (location.objects.ContainsKey(adjecantTile))
                {
                    item = location.objects[adjecantTile];
                }
                
                if (item != null && (item is Chest || _config.GetConnectorItems().Contains(item.Name)))
                {
                    var connectedTile = new ConnectedTile
                    {
                        Location = adjecantTile,
                    };
                    var chest = item as Chest;
                    if (chest != null)
                    {
                        connectedTile.Chest = chest;
                    }
                    else
                    {
                        connectedTile.Object = item;
                    }
                    processedLocations.Add(connectedTile);
                    FindConnectedLocations(location, adjecantTile, processedLocations);
                }
                else if (location.terrainFeatures.ContainsKey(adjecantTile))
                {
                    var feature = location.terrainFeatures[adjecantTile] as Flooring;
                    if (feature == null)
                    {
                        continue;
                    }
                    if (_config.FlooringsToConsiderConnectors.Contains(feature.whichFloor))
                    {
                        processedLocations.Add(new ConnectedTile { Location = adjecantTile });
                        FindConnectedLocations(location, adjecantTile, processedLocations);
                    }
                }
            }
        }


        private IEnumerable<Vector2> GetAdjecantTiles(GameLocation location, Vector2 startPosition)
        {
            for (int x = -1; x <= 1; ++x)
            {
                for (int y = -1; y <= 1; ++y)
                {
                    if (y == x || y == -x)
                    {
                        //ignore diagonals
                        continue;
                    }
                    var vector = new Vector2(startPosition.X + x, startPosition.Y + y);
                    if (vector != startPosition && _locationHelper.IsTileOnMap(location, vector))
                    {
                        yield return vector;
                    }
                }
            }
        }

        public bool HaveConnectorsInInventoryChanged(EventArgsInventoryChanged inventoryChange)
        {
            var changes = inventoryChange.Added.Concat(inventoryChange.QuantityChanged).Concat(inventoryChange.Removed);
            if (changes.Any(i => _config.GetConnectorItems().Contains(i.Item.Name) || i.Item.category == Object.furnitureCategory))
            {
                return true;
            }
            return false;
        }
    }
}
