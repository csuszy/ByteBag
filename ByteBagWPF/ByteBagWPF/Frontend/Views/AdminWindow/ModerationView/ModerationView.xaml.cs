using System.Windows.Controls;

namespace ByteBagWPF.Frontend.Views.AdminWindow.ModerationView
{
    /// <summary>
    /// Interaction logic for ModerationView.xaml
    /// </summary>
    public partial class ModerationView : UserControl
    {
        //baseURL.Instance.GlobalURLString; -> szerver oldali végpont globális változója.
        //private string modarationEndpoint = "";
        public ModerationView()
        {
            InitializeComponent();
        }

        private void moderationListLB_SelectionChanged(object sender, SelectionChangedEventArgs e)//Listában való item kiválasztása esetén a gombok elérhetőségének módosítása.
        {
            criticismDeleteBT.IsEnabled = true;
            criticismBT.IsEnabled = true;
        }
    }
}
