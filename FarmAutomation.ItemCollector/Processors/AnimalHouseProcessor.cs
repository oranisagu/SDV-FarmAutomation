using System;
using System.Collections.Generic;
using System.Linq;
using FarmAutomation.Common;
using Microsoft.Xna.Framework;
using StardewValley;
using StardewValley.Buildings;
using StardewValley.Objects;
using Object = StardewValley.Object;

namespace FarmAutomation.ItemCollector.Processors
{
    internal class AnimalHouseProcessor
    {
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
            "Rabbits Foot"
        };

        private bool _dayliesDone;

        public AnimalHouseProcessor(bool petAnimals, int additionalFriendshipFromCollecting)
        {
            PetAnimals = petAnimals;
            AdditionalFriendshipFromCollecting = additionalFriendshipFromCollecting;
        }

        public bool PetAnimals { get; set; }
        public int AdditionalFriendshipFromCollecting { get; set; }

        public void ProcessAnimalBuildings()
        {
            var farm = Game1.getFarm();
            if (_dayliesDone)
            {
                return;
            }
            foreach (var building in farm.buildings)
            {
                if (PetAnimals && building.indoors is AnimalHouse)
                {
                    foreach (var farmAnimal in ((AnimalHouse)building.indoors).animals)
                    {
                        PetAnimal(farmAnimal.Value);
                    }
                }

                var chest = ItemFinder.FindChestInLocation(building.indoors);
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
                    foreach (var animal in ((AnimalHouse) building.indoors).animals.Values)
                    {
                        CollectBarnAnimalProduce(animal, chest);

                    }
                }
                if (building.indoors is SlimeHutch)
                {
                    // collect goop
                }
                _dayliesDone = true;
            }
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
                        // show message that the chest is full
                        return;
                    }
                    chest.addItem(
                        new Object(Vector2.Zero, animal.currentProduce, null, false, true, false, false));
                    animal.friendshipTowardFarmer = Math.Min(1000,
                        animal.friendshipTowardFarmer + AdditionalFriendshipFromCollecting);
                    animal.currentProduce = -1;
                }
            }
        }

        public void DailyReset()
        {
            _dayliesDone = false;
        }
    }
}