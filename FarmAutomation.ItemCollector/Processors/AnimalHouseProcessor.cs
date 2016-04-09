using System;
using System.Collections.Generic;
using System.Linq;
using FarmAutomation.Common.Interfaces;
using FarmAutomation.ItemCollector.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValley.Buildings;
using StardewValley.Objects;
using Object = StardewValley.Object;

namespace FarmAutomation.ItemCollector.Processors
{
    internal class AnimalHouseProcessor : IAnimalHouseProcessor
    {
        private readonly ILog _logger;
        private readonly IItemFinder _itemFinder;
        private readonly IAnimalHouseProcessorConfiguration _configuration;
        private readonly ISoundHelper _soundHelper;

        private readonly List<string> _barnTools = new List<string>
        {
            "Milk Pail",
            "Shears"
        };

        private readonly List<string> _coopCollectibles = new List<string>
        {
            "Egg",
            "Large Egg",
            "Duck Egg",
            "Wool",
            "Duck Feather",
            "Rabbit's Foot",
            "Void Egg",
            "Dinosaur Egg"
        };

        private bool _dailiesDone;

        public AnimalHouseProcessor(ILog logger, IItemFinder itemFinder, IAnimalHouseProcessorConfiguration configuration, ISoundHelper soundHelper)
        {
            _logger = logger;
            _itemFinder = itemFinder;
            _configuration = configuration;
            _soundHelper = soundHelper;
        }

        public void ProcessAnimalBuildings()
        {
            var farm = Game1.getFarm();
            if (_dailiesDone)
            {
                return;
            }
            if (_configuration.MuteAnimalsWhenCollecting)
            {
                _soundHelper.MuteTemporary(2000);
            }
            _logger.Info("Petting animals and processing their buildings to collect items");
            if (_configuration.PetAnimals)
            {
                var allAnimals = farm.animals.Values.Concat(farm.buildings.Where(b => b.indoors is AnimalHouse).SelectMany(i => ((AnimalHouse)i.indoors).animals.Values));
                foreach (var animal in allAnimals)
                {
                    PetAnimal(animal);
                }
                _logger.Info("All animals have been petted.");
            }
            foreach (var building in farm.buildings)
            {
                var chest = _itemFinder.FindChestInLocation(building.indoors);
                if (chest == null)
                {
                    continue;
                }
                if (building is Coop)
                {
                    // collect eggs
                    CollectItemsFromBuilding(building, chest, _coopCollectibles);
                }
                if (building is Barn)
                {
                    int outsideAnimalCount = 0;
                    foreach (var outsideAnimal in farm.animals.Values.Where(a => a.home is Barn && a.home == building))
                    {
                        CollectBarnAnimalProduce(outsideAnimal, chest);
                        ++outsideAnimalCount;
                    }
                    if (outsideAnimalCount > 0)
                    {
                        _logger.Debug($"Found {outsideAnimalCount} animals wandering outside. collected their milk or wool and put it in the chest in their {building.buildingType}");
                    }
                    int insideAnimalCount = 0;
                    foreach (var animal in ((AnimalHouse)building.indoors).animals.Values)
                    {
                        CollectBarnAnimalProduce(animal, chest);
                        ++insideAnimalCount;
                    }
                    if (insideAnimalCount > 0)
                    {
                        _logger.Debug($"Found {insideAnimalCount} animals in the {building.buildingType}. Collected their milk or wool and put it in the chest in their home.");
                    }
                }
                if (building.indoors is SlimeHutch)
                {
                    // collect goop
                    _logger.Info("You have a slime hutch, but unfortunately we cannot collect the slime there yet. This feature will be added in the future.");
                }
            }
            _dailiesDone = true;
        }

        private void CollectItemsFromBuilding(Building building, Chest chest, List<string> coopCollectibles)
        {
            var collectibles =
                building.indoors.Objects.Where(o => coopCollectibles.Contains(o.Value.name)).ToList();

            collectibles.ForEach(c =>
            {
                if (chest.addItem(c.Value) == null)
                {
                    building.indoors.Objects.Remove(c.Key);
                }
                _logger.Debug($"Collected a {c.Value.Name} and put it into the chest in your {building.buildingType}");
            });
        }

        private void PetAnimal(FarmAnimal animal)
        {
            if (!animal.wasPet)
            {
                animal.pet(Game1.player);
            }
        }

        private void CollectBarnAnimalProduce(FarmAnimal animal, Chest chest)
        {
            if (_barnTools.Contains(animal.toolUsedForHarvest))
            {
                //var farmer = Game1.player;
                if (
                    animal.currentProduce > 0
                    && animal.age >= animal.ageWhenMature
                    )
                {
                    if (chest.items.Count >= 36)
                    {
                        _logger.Error($"A {animal.type} is ready for harvesting it's produce. Unfortunately the chest in it's home is already full.");
                        // show message that the chest is full
                        return;
                    }
                    if (animal.showDifferentTextureWhenReadyForHarvest)
                    {
                        animal.sprite.Texture = Game1.content.Load<Texture2D>("Animals\\Sheared" + animal.type);
                    }
                    chest.addItem(
                        new Object(Vector2.Zero, animal.currentProduce, null, false, true, false, false));
                    animal.friendshipTowardFarmer = Math.Min(1000,
                        animal.friendshipTowardFarmer + _configuration.AdditionalFriendshipFromCollecting);
                    animal.currentProduce = -1;
                }
            }
        }

        public void DailyReset()
        {
            _dailiesDone = false;
        }
    }
}