using StardewValley;
using StardewValley.Objects;

namespace FarmAutomation.ItemCollector.Interfaces
{
    internal interface IMachineHelper
    {
        void ProcessMachine(Object o, Chest connectedChest);

        void DailyReset();
        void SetLocation(GameLocation gameLocation);
    }
}