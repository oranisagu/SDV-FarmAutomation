using FarmAutomation.Common;
using StardewModdingAPI;
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
                    Log.Info("Refilled your {0} with a {1} of {2} quality. The machine now takes {3} minutes to process", machine.Name, refillable.Name, (ItemQuality)refillable.quality, machine.minutesUntilReady );
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
                Log.Info("Collected a {0} from your {1} - the next item will be ready in {2}", machine.heldObject.Name, machine.Name, machine.minutesUntilReady);
            }
            else if (connectedChest.addItem(machine.heldObject) == null)
            {
                Log.Info("Collected a {0} from your {1}", machine.heldObject.Name, machine.Name);
                SetMachineIdle(machine);
            }
        }

        public static bool MachineIsReadyForHarvest(Object machine)
        {
            return machine.heldObject != null && machine.minutesUntilReady == 0;
        }


        public static bool MachineIsReadyForProcessing(Object machine)
        {
            return !(machine is Chest) && machine.minutesUntilReady == 0 && machine.heldObject == null;
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