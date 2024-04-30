using ByteBagWPF.Backend.baseURL;
using ByteBagWPF.Backend.baseURL.ConfigWriter;
using ByteBagWPF.Frontend.Views.MessageWindow.Error;
using ByteBagWPF.Frontend.Views.MessageWindow.Ok;
using System.Windows;
using System.Windows.Input;

namespace ByteBagWPF.Frontend.Views.MessageWindow.Edit.EndPointURL
{
    /// <summary>
    /// Interaction logic for EditEndpointWindow.xaml
    /// </summary>
    public partial class EditEndpointWindow : Window
    {
        public EditEndpointWindow()
        {
            InitializeComponent();
            warningLabel.Content = $"Ügyelj rá,\n hogy helyesen add meg a végpont URL-jét!\n\nPl: https://bytebag.hu";
            EndpointTbx.Text = baseURL.Instance.GlobalURLString;
        }

        private void Image_MouseUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)//Ablak mozgatása
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)//ablak tálcára küldése.
        {
            WindowState = WindowState.Minimized;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)//ablak bezárása.
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();//Ablak bezárása.
        }

        private void okBT_Click(object sender, RoutedEventArgs e)//Ok gomb kattintásának hatása.
        {
            try
            {
                if (EndpointTbx.Text != "")
                {
                    baseURL.Instance.GlobalURLString = EndpointTbx.Text;
                    ConfigManager.SetConfigValue("baseURL", baseURL.Instance.GlobalURLString);
                    OkayMessageWindow okayMessageWindow = new OkayMessageWindow();
                    okayMessageWindow.Show();
                    this.Close();
                    MainWindow mainWindow = new MainWindow();
                    mainWindow.Show();
                }
                else
                {
                    ErrorMessageWindow errorMessageWindow = new ErrorMessageWindow();
                    errorMessageWindow.LabelContent = "Hiba történt!\n\nNem lehet üres a mező!";
                    errorMessageWindow.Show();
                }
            }
            catch
            {
                ErrorMessageWindow errorMessageWindow = new ErrorMessageWindow();//létrehozzuk számára az ablakot.
                errorMessageWindow.errorTextBlock.Text = $"Hiba történt!\n\nA kulcs módosítása sikertelen!\nByteBagConfig.json";
                errorMessageWindow.Show();//megjelenítjük az ablakot.
            }
        }

        private void undoBT_Click(object sender, RoutedEventArgs e)//Visszavonás
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();//Ablak bezárása.
        }
    }
}
