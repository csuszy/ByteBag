using System.Windows;
using System.Windows.Input;

namespace ByteBagWPF.Frontend.Views.MessageWindow.Error
{
    /// <summary>
    /// Interaction logic for ErrorMessageWindow.xaml
    /// </summary>
    public partial class ErrorMessageWindow : Window
    {
        public string LabelContent//Label contentjének átalakítására tulajdonság beállítása.
        {
            get { return errorMessageLaB.Content.ToString(); }
            set { errorMessageLaB.Content = value; }
        }

        public ErrorMessageWindow()
        {
            InitializeComponent();
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
            this.Close();
        }

        private void okBT_Click(object sender, RoutedEventArgs e)//Ok gomb kattintásának hatása.
        {
            this.Close();
        }
    }
}
