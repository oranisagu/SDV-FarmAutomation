using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using StardewValley;

namespace FarmAutomation.Common
{
    public class SoundHelper
    {
        public static void MuteTemporary(int milliseconds)
        {
            var worker = new BackgroundWorker();
            var originalVolume = Game1.options.soundVolumeLevel;
            worker.DoWork += (s, a) =>
            {
                Game1.options.soundVolumeLevel = 0;
                Game1.soundCategory.SetVolume(0);
                Thread.Sleep((int) a.Argument);
                Game1.options.soundVolumeLevel = originalVolume;
                Game1.soundCategory.SetVolume(originalVolume);
            };
            worker.RunWorkerAsync(milliseconds);
        }
    }
}
