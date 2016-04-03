using StardewValley;
using StardewValley.Objects;

namespace FarmAutomation.Common.Interfaces
{
    public interface IItemHelper
    {
        void RemoveItemFromChest(Object refillable, Chest chest, int amount = 1);
    }
}