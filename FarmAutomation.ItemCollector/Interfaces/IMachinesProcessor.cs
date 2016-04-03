using StardewValley;

namespace FarmAutomation.ItemCollector.Interfaces
{
    public interface IMachinesProcessor
    {
        void DailyReset();
        void ProcessMachines();
        void InvalidateCacheForLocation(GameLocation currentLocation);
    }
}