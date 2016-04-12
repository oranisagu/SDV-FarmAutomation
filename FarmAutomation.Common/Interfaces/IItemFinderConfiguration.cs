using System.Collections.Generic;

namespace FarmAutomation.Common.Interfaces
{
    public interface IItemFinderConfiguration
    {
        List<string> GetConnectorItems();
        List<int> FlooringsToConsiderConnectors { get; set; }
        bool AllowDiagonalConnectionsForAllItems { get; set; }
    }
}