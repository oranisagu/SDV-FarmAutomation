using FarmAutomation.Common;
using StardewValley;
using StardewValley.Objects;

namespace FarmAutomation.ItemCollector.Processors
{
    public class MachineHelper
    {
        public static void ProcessMachine(Object machine, Chest connectedChest, MaterialHelper materialHelper)
        {
            if (MachineIsReadyForHarvest(machine))
            {
                HandleFinishedObjectInMachine(machine, connectedChest);
            }
            if (MachineIsReadyForProcessing(machine))
            {
                var refillable = materialHelper.FindMaterialForMachine(machine.Name, connectedChest);
                if (refillable != null)
                {
                    PutItemInMachine(machine, refillable);
                    ItemHelper.RemoveItemFromChest(refillable, connectedChest);
                }
            }
        }

        public static void HandleFinishedObjectInMachine(Object machine, Chest connectedChest)
        {
            // special case as we don't remove the held item
            if (machine.Name == "Crystalarium")
            {
                var item = machine.heldObject.getOne() as Object;
                if (item != null && connectedChest.addItem(item) == null)
                {
                    machine.minutesUntilReady = ItemHelper.GetMinutesForCrystalarium(item.ParentSheetIndex);
                    machine.readyForHarvest = false;
                    machine.showNextIndex = false;
                }
            }
            else if (connectedChest.addItem(machine.heldObject) == null)
            {
                SetMachineIdle(machine);
            }
        }

        public static bool MachineIsReadyForHarvest(Object machine)
        {
            return machine.heldObject != null && machine.minutesUntilReady == 0;
        }


        public static bool MachineIsReadyForProcessing(Object machine)
        {
            return machine.minutesUntilReady == 0 && machine.heldObject == null;
        }

        public static void SetMachineIdle(Object machine)
        {
            machine.heldObject = null;
            machine.readyForHarvest = false;
            machine.showNextIndex = false;
        }

        public static void PutItemInMachine(Object machine, Object refillable)
        {
            machine.performObjectDropInAction(refillable, false, Game1.player);
        }
    }
}