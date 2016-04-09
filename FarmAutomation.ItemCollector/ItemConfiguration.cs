using System.Collections.Generic;
using FarmAutomation.Common;
using FarmAutomation.Common.Configuration;

namespace FarmAutomation.ItemCollector
{
    public class ItemConfiguration : IConfigurationBase
    {
        public List<FloorTile> FloorTiles { get; set; }

        public Dictionary<string, List<Refillable>> MachineRefillables { get; set; }

        public ItemConfiguration()
        {
            FloorTiles = new List<FloorTile>();
            MachineRefillables = new Dictionary<string, List<Refillable>>();
        }

        public void InitializeDefaults()
        {
            FloorTiles.Add(new FloorTile { Name = "Wood Path", FlooringType = 6, InventoryItemId = 405 });
            var smelterRefillables = new List<Refillable>
            {
                new Refillable
                {
                    Name = "Iron Ore",
                    AmountNeeded = 3,
                    DependingItems = new List<Refillable>() {new Refillable() {Name = "Coal"}}
                }
            };
            MachineRefillables.Add("Smelter", smelterRefillables);
        }
    }
}