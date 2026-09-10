using stroevkaI;
using stroevkaI.Forms;
using stroevkaI.Services;

namespace stroevkaI
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            var config = AppConfig.Load();

            switch (config.Mode)
            {
                case AppMode.Standalone:
                    Application.Run(new PivotRowEditor(config.PchId));
                    break;
                case AppMode.Garrison:
                case AppMode.Central:
                default:
                    Application.Run(new Form1());
                    break;
            }
        }
    }
}