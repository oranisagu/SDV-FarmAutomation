using System.Collections.Generic;
using System.Linq;
using FarmAutomation.Common;
using Microsoft.Xna.Framework;
using StardewValley;
using StardewValley.Objects;
using Object = StardewValley.Object;

namespace FarmAutomation.ItemCollector.Processors
{
    class MachinesProcessor
    {
        private readonly List<string> _machineNamesToProcess;
        private readonly List<string> _gameLocationsToSearch;
        readonly Dictionary<string, Dictionary<Vector2, Chest>> _connectedChestsCache = new Dictionary<string, Dictionary<Vector2, Chest>>();
        private readonly MaterialHelper _materialHelper;
        private readonly bool _searchOnlyOnceDailyForChests;

        public MachinesProcessor(List<string> machineNamesToProcess, List<string> gameLocationsToSearch, bool searchOnlyOnceDailyForChests)
        {
            _machineNamesToProcess = machineNamesToProcess;
            _gameLocationsToSearch = gameLocationsToSearch;
            _searchOnlyOnceDailyForChests = searchOnlyOnceDailyForChests;
            _gameLocationsToSearch.ForEach(gl=> _connectedChestsCache.Add(gl, new Dictionary<Vector2, Chest>()));
            _materialHelper = new MaterialHelper();
            DailyReset();
        }

        public IEnumerable<GameLocation> GetLocations()
        {
            return _gameLocationsToSearch.Select(Game1.getLocationFromName); //.Concat(Game1.getFarm().buildings.Select(b=>b.indoors));
        }

        public void ProcessMachines()
        {
            foreach (var gameLocation in GetLocations())
            {
                var items = ItemFinder.FindObjectsWithName(gameLocation, _machineNamesToProcess);
                foreach (var valuePair in items)
                {
                    Vector2 location = valuePair.Key;
                    Object machine = valuePair.Value;
                    
                    Chest chest;
                    if (_connectedChestsCache[gameLocation.Name].ContainsKey(location))
                    {
                        chest = _connectedChestsCache[gameLocation.Name][location];
                    }
                    else
                    {
                        chest = ItemFinder.FindConnectedChests(machine, gameLocation);
                        if (chest != null || _searchOnlyOnceDailyForChests)
                        {
                            _connectedChestsCache[gameLocation.Name].Add(location, chest);
                        }
                    }

                    if (chest != null)
                    {
                        if (MachineIsReady(machine))
                        {
                            if (chest.addItem(machine.heldObject) == null)
                            {
                                SetMachineIdle(machine);
                            }
                        }
                        if (machine.minutesUntilReady == 0)
                        {
                            var refillable = _materialHelper.FindMaterialForMachine(machine.Name, chest);
                            if (refillable != null)
                            {
                                PutItemInMachine(machine, refillable);
                                ItemHelper.RemoveItemFromChest(refillable, chest);
                            }
                        }
                    }
                }
            }
        }

        private bool MachineIsReady(Object machine)
        {
            return machine.heldObject != null && machine.minutesUntilReady == 0;
        }

        private void SetMachineIdle(Object machine)
        {
            machine.heldObject = null;
            machine.readyForHarvest = false;
            machine.showNextIndex = false;
        }

        private void PutItemInMachine(Object machine, Object refillable)
        {
            machine.performObjectDropInAction(refillable, false, Game1.player);
        }

        public void DailyReset()
        {
            foreach (var cache in _connectedChestsCache)
            {
                cache.Value.Clear();
            }
        }
    }
}
