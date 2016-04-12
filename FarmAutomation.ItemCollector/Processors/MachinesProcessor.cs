using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FarmAutomation.Common;
using FarmAutomation.Common.Interfaces;
using FarmAutomation.ItemCollector.Interfaces;
using Microsoft.Xna.Framework;
using StardewValley;
using StardewValley.Objects;

namespace FarmAutomation.ItemCollector.Processors
{
    class MachinesProcessor : IMachinesProcessor
    {
        private readonly List<string> _machineNamesToProcess;
        private readonly List<string> _gameLocationsToSearch;
        Dictionary<string, Dictionary<Vector2, Chest>> _connectedChestsCache = new Dictionary<string, Dictionary<Vector2, Chest>>();
        private readonly ILocationHelper _locationHelper;
        private readonly IMachinesProcessorConfiguration _config;
        private readonly IMachineHelper _machineHelper;
        private readonly ILog _logger;
        private readonly IItemFinder _finder;
        private readonly ISoundHelper _soundHelper;

        public MachinesProcessor(ILocationHelper locationHelper, IMachinesProcessorConfiguration config, IMachineHelper machineHelper, ILog logger, IItemFinder finder, ISoundHelper soundHelper)
        {
            _machineNamesToProcess = config.GetMachineNamesToProcess();
            _gameLocationsToSearch = config.GetLocationsToSearch();
            _gameLocationsToSearch.ForEach(gl => _connectedChestsCache.Add(gl, new Dictionary<Vector2, Chest>()));
            _locationHelper = locationHelper;
            _config = config;
            _machineHelper = machineHelper;
            _logger = logger;
            _finder = finder;
            _soundHelper = soundHelper;
            DailyReset();
        }

        private IEnumerable<GameLocation> GetLocations()
        {
            var configuredNames = _gameLocationsToSearch.Select(Game1.getLocationFromName);
            if (_config.AddBuildingsToLocations)
            {
                return configuredNames.Concat(Game1.getFarm().buildings.Where(b => b.indoors != null).Select(b=>b.indoors));
            }
            return configuredNames;
        }

        private void BuildCacheForLocation(GameLocation gameLocation)
        {
            if (gameLocation != null)
            {
                var cacheToAdd = new Dictionary<Vector2, Chest>();
                _logger.Debug($"Starting search for connected locations at {_locationHelper.GetName(gameLocation)}");
                var items = _finder.FindObjectsWithName(gameLocation, _machineNamesToProcess);
                foreach (var valuePair in items)
                {
                    Vector2 location = valuePair.Key;
                    if (cacheToAdd.ContainsKey(location))
                    {
                        //already found in another search
                        continue;
                    }

                    List<ConnectedTile> processedLocations = new List<ConnectedTile>
                    {
                        new ConnectedTile {Location = location, Object = valuePair.Value}
                    };

                    _finder.FindConnectedLocations(gameLocation, location, processedLocations);
                    var chest = processedLocations.FirstOrDefault(c => c.Chest != null)?.Chest;
                    foreach (var connectedLocation in processedLocations)
                    {
                        cacheToAdd.Add(connectedLocation.Location, chest);
                    }
                }
                lock (_connectedChestsCache)
                {
                    if (_connectedChestsCache.ContainsKey(_locationHelper.GetName(gameLocation)))
                    {
                        // already ran?
                        _connectedChestsCache.Remove(_locationHelper.GetName(gameLocation));
                    }
                    _connectedChestsCache.Add(_locationHelper.GetName(gameLocation), new Dictionary<Vector2, Chest>());
                    foreach (var cache in cacheToAdd)
                    {
                        _connectedChestsCache[_locationHelper.GetName(gameLocation)].Add(cache.Key, cache.Value);
                    }
                }
                _logger.Debug($"Searched your {_locationHelper.GetName(gameLocation)} for machines to collect from and found a total of {_connectedChestsCache[_locationHelper.GetName(gameLocation)].Count} locations to look for");
            }
        }

        public void ProcessMachines()
        {
            if (_connectedChestsCache == null)
            {
                _connectedChestsCache = new Dictionary<string, Dictionary<Vector2, Chest>>();
                Parallel.ForEach(GetLocations(), BuildCacheForLocation);
            }
            if (_config.MuteWhileCollectingFromMachines > 0 && _config.MuteWhileCollectingFromMachines <= 5000)
            {
                _soundHelper.MuteTemporary(_config.MuteWhileCollectingFromMachines);
            }
            foreach (var gameLocation in GetLocations())
            {
                _machineHelper.SetLocation(gameLocation);
                lock (_connectedChestsCache)
                {
                    if (!_connectedChestsCache.ContainsKey(_locationHelper.GetName(gameLocation)))
                    {
                        // cache got invalidated
                        BuildCacheForLocation(gameLocation);
                    }
                }
                foreach (var valuePair in _connectedChestsCache[_locationHelper.GetName(gameLocation)])
                {
                    Vector2 location = valuePair.Key;
                    Chest connectedChest = valuePair.Value;
                    if (connectedChest == null)
                    {
                        // no chest connected
                        continue;
                    }
                    if (!gameLocation.objects.ContainsKey(location))
                    {
                        // skip connection without objects like floortiles etc
                        continue;
                    }
                    if (!_machineNamesToProcess.Contains(gameLocation.objects[location].Name))
                    {
                        continue;
                    }
                    _machineHelper.ProcessMachine(gameLocation.objects[location], connectedChest);
                }
            }
        }

        public void DailyReset()
        {
            if (Game1.hasLoadedGame)
            {
                _machineHelper.DailyReset();
            }
            _connectedChestsCache = null;
        }

        public void InvalidateCacheForLocation(GameLocation location)
        {
            if (_connectedChestsCache != null && _connectedChestsCache.ContainsKey(_locationHelper.GetName(location)))
            {
                _connectedChestsCache.Remove(_locationHelper.GetName(location));
            }
        }
    }
}
