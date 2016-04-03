using System.Collections.Generic;
using System.Linq;
using FarmAutomation.Common.Configuration;
using FarmAutomation.Common.Interfaces;
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
        public List<int> FlooringsToConsiderConnectors { get; set; }
        public string LocationsToSearch { get; set; }
        public bool AddBuildingsToLocations { get; set; }

        public ItemCollectorConfiguration()
        {
            FlooringsToConsiderConnectors = new List<int>();
        }

        public List<string> GetConnectorItems()
        {
            return new List<string>(ItemsToConsiderConnectors.Split(',').Select(v => v.Trim()));
        }

        public void InitializeDefaults()
        {
            PetAnimals = true;
            AdditionalFriendshipFromCollecting = 5;
            EnableMod = true;
            MachinesToCollectFrom = "Keg, Preserves Jar, Cheese Press, Mayonnaise Machine, Loom, Oil Maker, Recycling Machine, Crystalarium, Worm Bin, Bee House, Strange Capsule, Tapper";
            ItemsToConsiderConnectors = "Keg, Preserves Jar, Cheese Press, Mayonnaise Machine, Loom, Oil Maker, Recycling Machine, Crystalarium, Worm Bin, Bee House, Strange Capsule, Tapper, Chest";
            LocationsToSearch = "Farm, Greenhouse, FarmHouse, FarmCave";
            FlooringsToConsiderConnectors = new List<int> {6};
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