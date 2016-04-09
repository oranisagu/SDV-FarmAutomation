using System.Collections.Generic;
using StardewValley.Objects;

namespace FarmAutomation.ItemCollector.Interfaces
{
    public interface IMaterialHelper
    {
        IEnumerable<Refillable> FindMaterialForMachine(string machineName, Chest chest);
    }
}