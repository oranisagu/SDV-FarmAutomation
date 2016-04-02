using System.Collections.Generic;
using StardewValley;

namespace FarmAutomation.Common
{
    /// <summary>
    /// helper class for actions where a reference to a farmer is necessary
    /// </summary>
    public class GhostFarmer : Farmer
    {
        public new bool IsMainPlayer => true;

        private GhostFarmer()
        {
            ClearInventory();
            sprite = Game1.player.sprite;
            maxItems = 24;
        }

        public void ClearInventory()
        {
            items = new List<Item>(new Item[maxItems]);
        }

        /// <summary>
        /// need to override the constructor as for some reason the base sets the sprites on the main player which leads to a crash.
        /// </summary>
        /// <returns></returns>
        public static GhostFarmer CreateFarmer()
        {
            var prevSprite = Game1.player.sprite;
            var who = new GhostFarmer();
            Game1.player.sprite = prevSprite;
            return who;
        }
    }
}