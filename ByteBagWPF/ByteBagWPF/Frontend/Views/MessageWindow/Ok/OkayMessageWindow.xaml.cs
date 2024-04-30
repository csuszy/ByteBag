using System.Windows;
using System.Windows.Input;

namespace ByteBagWPF.Frontend.Views.MessageWindow.Ok
{
    /// <summary>
    /// Interaction logic for OkayMessageWindow.xaml
    /// </summary>
    public partial class OkayMessageWindow : Window
    {
        public string LabelContent//Label contentjéhez szükséges string felépítése.
        {
            get { return okayMessageLaB.Content.ToString(); }
            set { okayMessageLaB.Content = value; }
        }

        public OkayMessageWindow()
        {
            InitializeComponent();
        }

        private void Image_MouseUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)//ablak mozgatása
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)//ablak tálcára helyezése.
        {
            WindowState = WindowState.Minimized;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)//ablak bezárása.
        {
            this.Close();
        }

        private void okBT_Click(object sender, RoutedEventArgs e)//oké gomb kattintási eseménye.
        {
            this.Close();
        }
    }
}
