using System.Collections.Generic;
using FarmAutomation.Common;
using FarmAutomation.Common.Configuration;
using FarmAutomation.ItemCollector.Interfaces;

namespace FarmAutomation.ItemCollector
{
    public class ItemConfiguration : IMaterialHelperConfiguration, IConfigurationBase
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
            FloorTiles.Add(new FloorTile { Name = "Wood Floor", FlooringType = 0, InventoryItemId = 328 });
            FloorTiles.Add(new FloorTile { Name = "Stone Floor", FlooringType = 1, InventoryItemId = 329 });
            FloorTiles.Add(new FloorTile { Name = "Weathered Floor", FlooringType = 2, InventoryItemId = 331 });
            FloorTiles.Add(new FloorTile { Name = "Crystal Floor", FlooringType = 3, InventoryItemId = 333 });
            FloorTiles.Add(new FloorTile { Name = "Straw Floor", FlooringType = 4, InventoryItemId = 401 });
            FloorTiles.Add(new FloorTile { Name = "Gravel Path", FlooringType = 5, InventoryItemId = 407 });
            FloorTiles.Add(new FloorTile { Name = "Wood Path", FlooringType = 6, InventoryItemId = 405 });
            FloorTiles.Add(new FloorTile { Name = "Crystal Path", FlooringType = 7, InventoryItemId = 409 });
            FloorTiles.Add(new FloorTile { Name = "Cobblestone Path", FlooringType = 8, InventoryItemId = 411 });
            FloorTiles.Add(new FloorTile { Name = "Stepping Stone Path", FlooringType = 9, InventoryItemId = 415 });

            var furnaceRefillables = new List<Refillable>
            {
                new Refillable ("Iron Ore")
                {
                    AmountNeeded = 5,
                    DependingItems = new List<Refillable> {new Refillable("Coal")}
                },
                new Refillable("Copper Ore")
                {
                    AmountNeeded = 5,
                    DependingItems = new List<Refillable> { new Refillable("Coal") }
                }
            };
            MachineRefillables.Add("Furnace", furnaceRefillables);

            List<Refillable> crabPotRefillables = new List<Refillable>
            {
                new Refillable("Bait")
            };
            MachineRefillables.Add("Crab Pot", crabPotRefillables);
        }
    }
}