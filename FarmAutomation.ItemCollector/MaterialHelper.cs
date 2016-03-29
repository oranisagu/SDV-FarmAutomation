using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewValley;
using StardewValley.Objects;
using Object = StardewValley.Object;

namespace FarmAutomation.ItemCollector
{
    class MaterialHelper
    {
        public Object FindMaterialForMachine(string machineName, Chest chest)
        {
            if (chest == null)
            {
                return null;
            }

            switch (machineName)
            {
                case "Keg":
                    return (Object)chest.items.FirstOrDefault(i => i is Object && IsKegMaterial(i));
                case "Preserves Jar":
                    return (Object)chest.items.FirstOrDefault(i => i is Object && IsPreservesJarMaterial(i));
                case "Cheese Press":
                    return (Object) chest.items.FirstOrDefault(i => i is Object && IsCheesePressMaterial(i));
                case "Mayonnaise Machine":
                    return (Object) chest.items.FirstOrDefault(i => i is Object && IsMayonnaiseMachineMaterial(i));
                case "Loom":
                    return (Object) chest.items.FirstOrDefault(i => i is Object && IsLoomMaterial(i));
                case "Oil Maker":
                    return (Object) chest.items.FirstOrDefault(i => i is Object && IsOilMakerMaterial(i));
                case "Recycling Machine":
                    return (Object) chest.items.FirstOrDefault(i => i is Object && IsRecyclingMachineMaterial(i));
                default:
                    return null;
            }
        }

        public bool IsRecyclingMachineMaterial(Item item)
        {
            return
                item.parentSheetIndex == 168
                || item.parentSheetIndex == 169
                || item.parentSheetIndex == 170
                || item.parentSheetIndex == 171
                || item.parentSheetIndex == 172
                ;
        }

        public bool IsOilMakerMaterial(Item item)
        {
            return item.parentSheetIndex == 270
                   || item.parentSheetIndex == 421
                   || item.parentSheetIndex == 430
                   || item.parentSheetIndex == 431;
        }

        public bool IsMayonnaiseMachineMaterial(Item item)
        {
            return item.parentSheetIndex == 174
                   || item.parentSheetIndex == 107
                   || item.parentSheetIndex == 176
                   || item.parentSheetIndex == 180
                   || item.parentSheetIndex == 182
                   || item.parentSheetIndex == 442;
        }

        public bool IsCheesePressMaterial(Item item)
        {
            return item.parentSheetIndex == 184
                   || item.parentSheetIndex == 186
                   || item.parentSheetIndex == 436
                   || item.parentSheetIndex == 438;
        }

        public bool IsPreservesJarMaterial(Item item)
        {
            return item.category == -79 //Jelly
                   || item.category == -75 //Pickled
                ;
        }

        public bool IsKegMaterial(Item i)
        {
            return i.category == -79 //Wine
                   || i.category == -75 // Juice
                   || i.Name == "Wheat"
                   || i.Name == "Hops";
        }

        public bool IsLoomMaterial(Item i)
        {
            return i.parentSheetIndex == 440;
        }
    }
}
