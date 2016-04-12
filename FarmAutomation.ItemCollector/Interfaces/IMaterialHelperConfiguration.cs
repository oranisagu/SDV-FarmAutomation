using System.Collections.Generic;

namespace FarmAutomation.ItemCollector.Interfaces
{
    public interface IMaterialHelperConfiguration
    {
        Dictionary<string,List<Refillable>> MachineRefillables { get; set; }
    }
}