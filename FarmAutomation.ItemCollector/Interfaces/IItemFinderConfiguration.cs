using System;

namespace FarmAutomation.ItemCollector.Interfaces
{
    public interface IItemFinderConfiguration
    {

        string ItemsToConsiderConnectors { get; set; }
        string FlooringsToConsiderConnectors { get; set; }
        bool AllowDiagonalConnectionsForAllItems { get; set; }
    }
}