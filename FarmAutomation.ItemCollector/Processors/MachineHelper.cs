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
                Object coal = null;
                if (machine.Name == "Furnace")
                {
                    coal = materialHelper.FindMaterialForMachine("Coal", connectedChest);
                    if (coal == null)
                    {
                        //no coal to power the furnace
                        return;
                    }
                }
                if (refillable != null)
                {
                    Farmer who = GhostFarmer.CreateFarmer();

                    // furnace needs an additional coal
                    if (machine.Name == "Furnace")
                    {
                        var coalAmount = materialHelper.GetMaterialAmountForMachine(machine.Name, coal);
                        MoveItemToFarmer(coal, connectedChest, who, coalAmount);
                        ItemHelper.RemoveItemFromChest(coal, connectedChest, coalAmount);
                    }

                    var materialAmount = materialHelper.GetMaterialAmountForMachine(machine.Name, refillable);
                    var tempRefillable = MoveItemToFarmer(refillable, connectedChest, who, materialAmount);

                    if (!PutItemInMachine(machine, tempRefillable, who) && who.items.Count > 0)
                    {
                        // item was not accepted by the machine, transfer it back to the chest
                        who.items.ForEach(i => connectedChest.addItem(i));
                    }
                    Log.Info("Refilled your {0} with a {1} of {2} quality. The machine now takes {3} minutes to process. You have {4} {1} left", machine.Name, refillable.Name, (ItemQuality)refillable.quality, machine.minutesUntilReady, refillable.Stack);
                }
            }
        }

        private static Object MoveItemToFarmer(Object itemToMove, Chest sourceChest, Farmer target, int amount)
        {
            var temporaryItem = (Object)itemToMove.getOne();
            temporaryItem.Stack = amount;
            target.items.Add(temporaryItem);
            ItemHelper.RemoveItemFromChest(itemToMove, sourceChest, amount);
            return temporaryItem;
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

        public static bool PutItemInMachine(Object machine, Object refillable, Farmer who = null)
        {
            if (who == null)
            {
                who = Game1.player;
            }
            return machine.performObjectDropInAction(refillable, false, who);
        }
    }
}