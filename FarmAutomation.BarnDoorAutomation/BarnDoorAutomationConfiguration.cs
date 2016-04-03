using FarmAutomation.Common.Configuration;

namespace FarmAutomation.BarnDoorAutomation
{
    /// <summary>
    /// the json serializable configuration for the barn door automation mod
    /// </summary>
    public class BarnDoorAutomationConfiguration : IConfigurationBase
    {

        public bool EnableMod { get; set; }
        public int OpenDoorsAfter { get; set; }
        public int CloseDoorsAfter { get; set; }
        public int FirstDayInSpringToOpen { get; set; }

        public void InitializeDefaults()
        {
            EnableMod = true;
            FirstDayInSpringToOpen = 1;
            OpenDoorsAfter = 600;
            CloseDoorsAfter = 1930;
        }
    }
}
