using System;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using System.Collections.Generic;
using System.Linq;
using FarmAutomation.Common;
using FarmAutomation.ItemCollector.Processors;
using StardewValley;

namespace FarmAutomation.ItemCollector
{
    public class ItemCollectorMod : Mod
    {
        private bool _gameLoaded;
        private readonly MachinesProcessor _machinesProcessor;
        private readonly AnimalHouseProcessor _animalHouseProcessor;
        private readonly ItemCollectorConfiguration _config;

        public ItemCollectorMod()
        {
            Log.Info("Initalizing ItemCollector Mod");
            _config = ConfigurationBase.LoadConfiguration<ItemCollectorConfiguration>();
            ItemFinder.ConnectorItems = new List<string>(_config.ItemsToConsiderConnectors.Split(',').Select(v => v.Trim()));
            ItemFinder.ConnectorFloorings = _config.FlooringsToConsiderConnectors;

            var machinesToCollectFrom = _config.MachinesToCollectFrom.Split(',').Select(v => v.Trim()).ToList();
            var locationsToSearch = _config.LocationsToSearch.Split(',').Select(v => v.Trim()).ToList();
            _machinesProcessor = new MachinesProcessor(machinesToCollectFrom, locationsToSearch, _config.AddBuildingsToLocations);
            _animalHouseProcessor = new AnimalHouseProcessor(_config.PetAnimals, _config.AdditionalFriendshipFromCollecting);
        }

        public override void Entry(params object[] objects)
        {
            base.Entry(objects);
            GameEvents.GameLoaded += (s, e) => { _gameLoaded = true; };

            TimeEvents.DayOfMonthChanged += (s, e) =>
            {
                if (_config.EnableMod)
                {
                    _animalHouseProcessor.DailyReset();
                    _machinesProcessor.DailyReset();
                }
            };
            TimeEvents.TimeOfDayChanged += (s, e) =>
            {

                if (_gameLoaded && _config.EnableMod)
                {
                    try
                    {
                        _animalHouseProcessor.ProcessAnimalBuildings();
                        _machinesProcessor.ProcessMachines();
                    }
                    catch (Exception ex)
                    {
                        Log.Error("an error occured with the automation mod: {0}", ex);
                    }
                }
            };
            PlayerEvents.InventoryChanged += (s, c) =>
            {
                if (_gameLoaded && ItemFinder.HaveConnectorsInInventoryChanged(c))
                {
                    try
                    {
                        _animalHouseProcessor.DailyReset();
                        _machinesProcessor.InvalidateCacheForLocation(Game1.player.currentLocation);
                    }
                    catch (Exception ex)
                    {
                        Log.Error("an error occured with the automation mod: {0}", ex);
                    }
                }
            };
        }
    }
}
