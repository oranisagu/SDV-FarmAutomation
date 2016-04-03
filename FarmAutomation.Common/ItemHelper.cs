using FarmAutomation.Common.Interfaces;
using StardewValley.Objects;
using Object = StardewValley.Object;

namespace FarmAutomation.Common
{
    public class ItemHelper :IItemHelper
    {
        public void RemoveItemFromChest(Object refillable, Chest chest, int amount = 1)
        {
            refillable.Stack -= amount;
            if (refillable.Stack <= 0)
            {
                //used last item of stack - delete from chest
                chest.items[chest.items.IndexOf(refillable)] = null;
                chest.items.RemoveAll(i => i == null);
            }
        }
    }
}
