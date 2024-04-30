using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace ByteBagWPF.Frontend.Views.MessageWindow.Update
{
    /// <summary>
    /// Interaction logic for UpdateAvaiable.xaml
    /// </summary>
    public partial class UpdateAvaiable : Window
    {
        private DispatcherTimer timer;
        private double progressValue = 0;
        public UpdateAvaiable()
        {
            InitializeComponent();
            InitializeTimer();
            progressValue = 0;
            progressBar.Value = progressValue;
            timer.Start();
        }


        private void InitializeTimer()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(0.05); // 50ms-es időközönként növeli az értéket
            timer.Tick += Timer_Tick;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            progressValue += 100.0 / (5 * (1000 / 50)); // Az 5 másodperc alatt növekvő érték kiszámítása
            progressBar.Value = progressValue;

            if (progressBar.Value >= progressBar.Maximum)
            {
                timer.Stop();
            }
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
    }
}
