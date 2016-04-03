using StardewValley;
using StardewValley.Objects;

namespace FarmAutomation.ItemCollector.Interfaces
{
    public interface IMaterialHelper
    {
        Object FindMaterialForMachine(string name, Chest connectedChest);
        int GetMaterialAmountForMachine(string name, Object coal);
    }
}