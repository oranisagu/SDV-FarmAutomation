using System.Collections.Generic;
using System.Linq;
using FarmAutomation.ItemCollector.Interfaces;
using StardewValley.Objects;
namespace FarmAutomation.ItemCollector.Processors
{
    public class MaterialHelper : IMaterialHelper
    {
        private readonly IMaterialHelperConfiguration _items;

        public MaterialHelper(IMaterialHelperConfiguration items)
        {
            _items = items;
        }

        public IEnumerable<Refillable> FindMaterialForMachine(string machineName, Chest chest)
        {
            if (chest == null)
            {
                return null;
            }

            if (_items.MachineRefillables.ContainsKey(machineName))
            {
                return _items.MachineRefillables[machineName].Select(mr => mr.GetMatchingRefillables(chest)).FirstOrDefault();
            }
            return null;
        }
    }
}
