using System.ComponentModel;
using System.Threading;
using FarmAutomation.Common.Interfaces;
using StardewValley;

namespace FarmAutomation.Common
{
    public class SoundHelper : ISoundHelper
    {
        private readonly object _lock = new object();
        public void MuteTemporary(int milliseconds)
        {
            var worker = new BackgroundWorker();
            worker.DoWork += (s, a) =>
            {
                lock (_lock)
                {
                    var originalVolume = Game1.options.soundVolumeLevel;
                    if (originalVolume.CompareTo(0) == 0)
                    {
                        return;
                    }
                    Game1.soundCategory.SetVolume(0);
                    Thread.Sleep((int)a.Argument);
                    Game1.soundCategory.SetVolume(originalVolume);
                }
            };
            worker.RunWorkerAsync(milliseconds);

        }
    }
}
