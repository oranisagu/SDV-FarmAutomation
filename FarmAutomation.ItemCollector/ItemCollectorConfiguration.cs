using System.Collections.Generic;
using System.Linq;
using FarmAutomation.Common.Configuration;
using FarmAutomation.ItemCollector.Interfaces;

namespace FarmAutomation.ItemCollector
{
    public class ItemCollectorConfiguration : IConfigurationBase, IItemFinderConfiguration, IAnimalHouseProcessorConfiguration, IMachinesProcessorConfiguration
    {
        public bool EnableMod { get; set; }

        public bool PetAnimals { get; set; }
        public int AdditionalFriendshipFromCollecting { get; set; }
        public bool MuteAnimalsWhenCollecting { get; set; }

        public string MachinesToCollectFrom { get; set; }
        public string ItemsToConsiderConnectors { get; set; }
        public bool AllowDiagonalConnectionsForAllItems { get; set; }

        public string FlooringsToConsiderConnectors { get; set; }
        public string LocationsToSearch { get; set; }
        public bool AddBuildingsToLocations { get; set; }
        public int MuteWhileCollectingFromMachines { get; set; }

        public void InitializeDefaults()
        {
            PetAnimals = true;
            AdditionalFriendshipFromCollecting = 5;
            EnableMod = true;
            MachinesToCollectFrom = "Keg, Preserves Jar, Cheese Press, Mayonnaise Machine, Loom, Oil Maker, Recycling Machine, Crystalarium, Worm Bin, Bee House, Strange Capsule, Tapper, Statue Of Endless Fortune, Furnace, Seed Maker, Statue of Perfection, Crab Pot, Charcoal Kiln, Mushroom Box, Lightning Rod";
            ItemsToConsiderConnectors = "Keg, Preserves Jar, Cheese Press, Mayonnaise Machine, Loom, Oil Maker, Recycling Machine, Crystalarium, Worm Bin, Bee House, Strange Capsule, Tapper, Statue Of Endless Fortune, Furnace, Seed Maker, Statue of Perfection, Crab Pot, Charcoal Kiln, Mushroom Box, Lightning Rod, Chest";
            LocationsToSearch = "Farm, Greenhouse, FarmHouse, FarmCave, Beach";
            FlooringsToConsiderConnectors = "Wood Path";
            AddBuildingsToLocations = true;
        }

        public List<string> GetMachineNamesToProcess()
        {
            return MachinesToCollectFrom.Split(',').Select(m => m.Trim()).ToList();
        }

        public List<string> GetLocationsToSearch()
        {
            return LocationsToSearch.Split(',').Select(v => v.Trim()).ToList();
        }
    }
}