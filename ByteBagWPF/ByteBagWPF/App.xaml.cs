using System.Windows;

namespace ByteBagWPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // UpdateChecker példány létrehozása
            var updateChecker = new Backend.UpdateChecker.checkUpdate();

            await updateChecker.CheckForUpdatesAsync();
            // Frissítés ellenőrzése induláskor

        }
    }
}
