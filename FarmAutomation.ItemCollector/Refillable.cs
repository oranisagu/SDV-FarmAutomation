using System.Collections.Generic;
using System.Linq;
using StardewValley;
using StardewValley.Objects;

namespace FarmAutomation.ItemCollector
{
    public class Refillable
    {
        public string Name { get; set; }
        public int AmountNeeded { get; set; }
        public List<Refillable> DependingItems { get; set; }

        private bool InChest(Chest chest)
        {
            return chest.items.Any(i => i.Name == Name && i.Stack >= AmountNeeded && DependingItems.All(di => di.InChest(chest)));
        }

        public IEnumerable<Refillable> GetMatchingRefillables(Chest chest)
        {
            if (InChest(chest))
            {
                return GetFlatList(this);
            }
            return null;
        }

        private static IEnumerable<Refillable> GetFlatList(Refillable parent)
        {
            yield return parent;
            foreach (var dependingItem in parent.DependingItems)
            {
                foreach (var child in GetFlatList(dependingItem))
                {
                    yield return child;
                }
            }
        }

        public bool ObjectSatisfiesRefillable(Item item)
        {
            return item != null && (item.Name == Name) && item.Stack >= AmountNeeded;
        }
    }
}