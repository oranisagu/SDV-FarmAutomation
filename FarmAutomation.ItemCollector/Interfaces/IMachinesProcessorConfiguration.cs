using System.Collections.Generic;

namespace FarmAutomation.ItemCollector.Interfaces
{
    internal interface IMachinesProcessorConfiguration
    {
        bool AddBuildingsToLocations { get; set; }
        List<string> GetMachineNamesToProcess();
        List<string> GetLocationsToSearch();
    }
}