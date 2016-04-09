using System.Linq;
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
                var refillable = _materialHelper.FindMaterialForMachine(machine.Name, connectedChest)?.ToList();
                if (refillable != null)
                {
                    var dropIn = refillable.First();
                    var tempItems = refillable.Select(r=>MoveItemsToFarmer(r, connectedChest, Who)).ToList();
                    
                    if (!PutItemInMachine(machine, tempItems.First(), Who))
                    {
                        // item was not accepted by the machine, transfer it back to the chest
                        Who.items.ForEach(i => connectedChest.addItem(i));
                    }
                    Who.ClearInventory();
                    _logger.Info($"Refilled your {machine.Name} with a {dropIn.Name} of {(ItemQuality)tempItems.First().quality} quality. The machine now takes {machine.minutesUntilReady} minutes to process.");
                }
            }
        }

        private Object MoveItemsToFarmer(Refillable refillable, Chest connectedChest, GhostFarmer who)
        {
            var itemToMove = connectedChest.items.OfType<Object>().FirstOrDefault(refillable.ObjectSatisfiesRefillable);
            if (itemToMove == null)
            {
                return null;
            }
            var temporaryItem = (Object)itemToMove.getOne();
            temporaryItem.Stack = refillable.AmountNeeded;
            var freeIndex = who.items.IndexOf(null);
            who.items[freeIndex] = temporaryItem;
            _itemHelper.RemoveItemFromChest(itemToMove, connectedChest, refillable.AmountNeeded);
            return temporaryItem;
        }

        private void HandleFinishedObjectInMachine(Object machine, Chest connectedChest)
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

        private bool MachineIsReadyForHarvest(Object machine)
        {
            return machine.readyForHarvest;
        }


        private bool MachineIsReadyForProcessing(Object machine)
        {
            return !(machine is Chest) && machine.minutesUntilReady == 0 && machine.heldObject == null;
        }


        private bool PutItemInMachine(Object machine, Object refillable, Farmer who)
        {
            return machine.performObjectDropInAction(refillable, false, who);
        }
    }
}