using FarmAutomation.Common;
using FarmAutomation.Common.Interfaces;
using FarmAutomation.ItemCollector.Interfaces;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Objects;

namespace FarmAutomation.ItemCollector.Processors
{
    public class MachineHelper : IMachineHelper
    {
        private readonly IMaterialHelper _materialHelper;
        private readonly ILog _logger;
        private readonly IFarmerFactory _factory;
        private readonly IItemHelper _itemHelper;
        private GhostFarmer _who;

        private GhostFarmer Who => _who ?? (_who = _factory.CreateFarmer());

        public MachineHelper(IMaterialHelper materialHelper, ILog logger, IFarmerFactory factory, IItemHelper itemHelper)
        {
            _materialHelper = materialHelper;
            _logger = logger;
            _factory = factory;
            _itemHelper = itemHelper;
        }

        public void ProcessMachine(Object machine, Chest connectedChest)
        {
            if (MachineIsReadyForHarvest(machine))
            {
                HandleFinishedObjectInMachine(machine, connectedChest);
            }
            if (MachineIsReadyForProcessing(machine))
            {
                var refillable = _materialHelper.FindMaterialForMachine(machine.Name, connectedChest);
                Object coal = null;
                if (machine.Name == "Furnace")
                {
                    coal = _materialHelper.FindMaterialForMachine("Coal", connectedChest);
                    if (coal == null)
                    {
                        //no coal to power the furnace
                        return;
                    }
                }
                if (refillable != null)
                {
                    // furnace needs an additional coal
                    if (machine.Name == "Furnace")
                    {
                        var coalAmount = _materialHelper.GetMaterialAmountForMachine(machine.Name, coal);
                        MoveItemToFarmer(coal, connectedChest, Who, coalAmount);
                    }

                    var materialAmount = _materialHelper.GetMaterialAmountForMachine(machine.Name, refillable);
                    if (materialAmount > refillable.Stack)
                    {
                        return;
                    }
                    var tempRefillable = MoveItemToFarmer(refillable, connectedChest, Who, materialAmount);

                    if (!PutItemInMachine(machine, tempRefillable, Who))
                    {
                        // item was not accepted by the machine, transfer it back to the chest
                        Who.items.ForEach(i => connectedChest.addItem(i));
                    }
                    Who.ClearInventory();
                    _logger.Info($"Refilled your {machine.Name} with a {refillable.Name} of {(ItemQuality)refillable.quality} quality. The machine now takes {machine.minutesUntilReady} minutes to process. You have {refillable.Stack} {refillable.Name} left");
                }
            }
        }

        private Object MoveItemToFarmer(Object itemToMove, Chest sourceChest, Farmer target, int amount)
        {
            var temporaryItem = (Object)itemToMove.getOne();
            temporaryItem.Stack = amount;
            var freeIndex = target.items.IndexOf(null);
            target.items[freeIndex] = temporaryItem;
            _itemHelper.RemoveItemFromChest(itemToMove, sourceChest, amount);
            return temporaryItem;
        }

        public void HandleFinishedObjectInMachine(Object machine, Chest connectedChest)
        {
            var logMessage = $"Collecting a {machine.heldObject?.Name} from your {machine.Name}.";
            machine.checkForAction(Who);
            Who.items.ForEach(i =>
            {
                if (i != null)
                {
                    connectedChest.addItem(i);
                }
            });

            if (machine.heldObject != null && machine.minutesUntilReady > 0)
            {
                logMessage += $" The next {machine.heldObject.Name} will be ready in {machine.minutesUntilReady}";
            }
            Who.ClearInventory();
            Log.Info(logMessage);
        }

        public bool MachineIsReadyForHarvest(Object machine)
        {
            return machine.readyForHarvest;
        }


        public bool MachineIsReadyForProcessing(Object machine)
        {
            return !(machine is Chest) && machine.minutesUntilReady == 0 && machine.heldObject == null;
        }


        public bool PutItemInMachine(Object machine, Object refillable, Farmer who)
        {
            return machine.performObjectDropInAction(refillable, false, who);
        }
    }
}