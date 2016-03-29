using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        Dictionary<string, Dictionary<Vector2, Chest>> _connectedChestsCache = new Dictionary<string, Dictionary<Vector2, Chest>>();
        private readonly MaterialHelper _materialHelper;

        public bool AddBuildingsToLocations { get; set; }

        public MachinesProcessor(List<string> machineNamesToProcess, List<string> gameLocationsToSearch, bool addBuildingsToLocations)
        {
            AddBuildingsToLocations = addBuildingsToLocations;
            _machineNamesToProcess = machineNamesToProcess;
            _gameLocationsToSearch = gameLocationsToSearch;
            _gameLocationsToSearch.ForEach(gl => _connectedChestsCache.Add(gl, new Dictionary<Vector2, Chest>()));
            _materialHelper = new MaterialHelper();
            DailyReset();
        }

        public IEnumerable<GameLocation> GetLocations()
        {
            var configuredNames = _gameLocationsToSearch.Select(Game1.getLocationFromName);
            if (AddBuildingsToLocations)
            {
                return configuredNames.Concat(Game1.getFarm().buildings.Select(b => b.indoors)).Where(b=>b!=null);
            }
            return configuredNames;
        }

        private void BuildCacheForLocation(GameLocation gameLocation)
        {
            if (gameLocation != null)
            {
                var cacheToAdd = new Dictionary<Vector2, Chest>();

                var items = ItemFinder.FindObjectsWithName(gameLocation, _machineNamesToProcess);
                foreach (var valuePair in items)
                {
                    Vector2 location = valuePair.Key;
                    Object machine = valuePair.Value;
                    if (cacheToAdd.ContainsKey(location))
                    {
                        //already found in another search
                        continue;
                    }

                    var processedLocations = new List<Vector2>();
                    var chest = ItemFinder.FindConnectedChests(machine, gameLocation, processedLocations);
                    foreach (var connectedLocation in processedLocations)
                    {
                        cacheToAdd.Add(connectedLocation, chest);
                    }
                }
                lock (_connectedChestsCache)
                {
                    if (_connectedChestsCache.ContainsKey(gameLocation.Name))
                    {
                        // already ran?
                        _connectedChestsCache.Remove(gameLocation.Name);
                    }
                    _connectedChestsCache.Add(gameLocation.Name, new Dictionary<Vector2, Chest>());
                    foreach (var cache in cacheToAdd)
                    {
                        _connectedChestsCache[gameLocation.Name].Add(cache.Key, cache.Value);
                    }
                }
            }
        }

        public void ProcessMachines()
        {
            if (_connectedChestsCache == null)
            {
                _connectedChestsCache = new Dictionary<string, Dictionary<Vector2, Chest>>();
                Parallel.ForEach(GetLocations(), BuildCacheForLocation);
            }
            foreach (var gameLocation in GetLocations())
            {
                lock (_connectedChestsCache)
                {
                    if (!_connectedChestsCache.ContainsKey(gameLocation.Name))
                    {
                        // cache got invalidated
                        BuildCacheForLocation(gameLocation);
                    }
                }
                foreach (var valuePair in _connectedChestsCache[gameLocation.Name])
                {
                    Vector2 location = valuePair.Key;
                    Chest connectedChest = valuePair.Value;
                    if (connectedChest == null)
                    {
                        // no chest connected
                        continue;
                    }

                    var machine = gameLocation.objects[location];
                    if (MachineIsReady(machine))
                    {
                        if (connectedChest.addItem(machine.heldObject) == null)
                        {
                            SetMachineIdle(machine);
                        }
                    }
                    if (machine.minutesUntilReady == 0)
                    {
                        var refillable = _materialHelper.FindMaterialForMachine(machine.Name, connectedChest);
                        if (refillable != null)
                        {
                            PutItemInMachine(machine, refillable);
                            ItemHelper.RemoveItemFromChest(refillable, connectedChest);
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
            _connectedChestsCache = null;
        }

        public void InvalidateCacheForLocation(GameLocation location)
        {
            if (_connectedChestsCache.ContainsKey(location.Name))
            {
                _connectedChestsCache.Remove(location.Name);
            }
        }
    }
}
