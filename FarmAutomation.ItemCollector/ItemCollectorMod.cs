using System;
using Castle.MicroKernel.Registration;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using Castle.Windsor;
using FarmAutomation.Common;
using FarmAutomation.Common.Interfaces;
using FarmAutomation.ItemCollector.Interfaces;
using Microsoft.Xna.Framework.Input;
using StardewValley;

namespace FarmAutomation.ItemCollector
{
    public class ItemCollectorMod : Mod
    {
        private bool _gameLoaded;
        private readonly IMachinesProcessor _machinesProcessor;
        private readonly IAnimalHouseProcessor _animalHouseProcessor;
        private readonly ItemCollectorConfiguration _config;
        private readonly ILog _logger;
        private readonly IItemFinder _itemFinder;

        public ItemCollectorMod()
        {
            Log.Info($"Initalizing {nameof(ItemCollectorMod)}");
            try
            {
                var container = new WindsorContainer();
                container.Install(new WindsorInstallers());
                container.Install(new ItemCollecterInstallers());

                _config = container.Resolve<IConfigurator>().LoadConfiguration<ItemCollectorConfiguration>();
                container.Register(Component.For(
                    typeof (IMachinesProcessorConfiguration),
                    typeof (IAnimalHouseProcessorConfiguration),
                    typeof (ItemCollectorConfiguration),
                    typeof (IMachinesProcessorConfiguration),
                    typeof (IItemFinderConfiguration)
                    ).Instance(_config));

                _logger = container.Resolve<ILog>();
                _machinesProcessor = container.Resolve<IMachinesProcessor>();
                _animalHouseProcessor = container.Resolve<IAnimalHouseProcessor>();
                _itemFinder = container.Resolve<IItemFinder>();
            }
            catch (Exception ex)
            {
                Log.Error($"Could not initialize the {nameof(ItemCollectorMod)}: {ex}");
            }
        }

        public override void Entry(params object[] objects)
        {
            base.Entry(objects);
            GameEvents.GameLoaded += (s, e) => { _gameLoaded = true; };

            TimeEvents.DayOfMonthChanged += (s, e) =>
            {
                if (_config.EnableMod)
                {
                    _logger.Debug("It's a new day. Resetting the Item Collector mod");
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
                        _logger.Error($"an error occured with the automation mod: {ex}");
                    }
                }
            };
            PlayerEvents.InventoryChanged += (s, c) =>
            {
                if (_gameLoaded && _itemFinder.HaveConnectorsInInventoryChanged(c))
                {
                    try
                    {
                        _animalHouseProcessor.DailyReset();
                        _machinesProcessor.InvalidateCacheForLocation(Game1.player.currentLocation);
                    }
                    catch (Exception ex)
                    {
                        _logger.Error($"an error occured with the automation mod: {ex}");
                    }
                }
            };
#if DEBUG
            // allow keypresses to initiate events for easier debugging.
            ControlEvents.KeyPressed += (s, c) =>
            {
                if (_gameLoaded && c.KeyPressed == Keys.K)
                {
                    _animalHouseProcessor.ProcessAnimalBuildings();
                    _machinesProcessor.ProcessMachines();
                }
                if (_gameLoaded && c.KeyPressed == Keys.P)
                {
                    _animalHouseProcessor.DailyReset();
                    _machinesProcessor.DailyReset();
                }
            };
#endif
        }
    }
}
