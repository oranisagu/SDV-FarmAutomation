using System.ComponentModel;
using System.Threading;
using FarmAutomation.Common.Interfaces;
using StardewValley;

namespace FarmAutomation.Common
{
    public class SoundHelper : ISoundHelper
    {
        public void MuteTemporary(int milliseconds)
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
