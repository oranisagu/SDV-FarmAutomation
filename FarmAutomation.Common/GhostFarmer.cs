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
            items = new List<Item>();
        }

        /// <summary>
        /// need to override the constructor as for some reason the base sets the sprites on the main player which leads to a crash.
        /// </summary>
        /// <returns></returns>
        public static Farmer CreateFarmer()
        {
            var prevSprite = Game1.player.sprite;
            var who = new GhostFarmer();
            Game1.player.sprite = prevSprite;
            return who;
        }
    }
}