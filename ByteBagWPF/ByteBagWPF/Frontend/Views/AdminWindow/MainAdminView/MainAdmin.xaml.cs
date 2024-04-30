using ByteBagWPF.Backend.AfterLogin;
using ByteBagWPF.Backend.baseURL;
using ByteBagWPF.Frontend.Views.AdminWindow.AdminsView;
using ByteBagWPF.Frontend.Views.AdminWindow.MainPageView;
using ByteBagWPF.Frontend.Views.AdminWindow.MarketPostControlView;
using ByteBagWPF.Frontend.Views.AdminWindow.ModerationView;
using ByteBagWPF.Frontend.Views.AdminWindow.PostControlView;
using ByteBagWPF.Frontend.Views.AdminWindow.UserControlView;
using ByteBagWPF.Frontend.Views.MessageWindow.Error;
using ByteBagWPF.Frontend.Views.MessageWindow.Progress;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;


namespace ByteBagWPF.Views.AdminWindow.MainAdminView
{
    /// <summary>
    /// Interaction logic for MainAdmin.xaml
    /// </summary>
    public partial class MainAdmin : Window
    {
        private List<LoginResponseClass> logindatas;
        private const double ScaleFactor = 1.1; // A méretváltoztatás alapértelmezett faktora a logo kép animációhoz.
        private ScaleTransform scaleTransform; //Privát ScaleTransform változó a kép animációhoz.

        public MainAdmin(List<LoginResponseClass> logindatas)
        {
            this.logindatas = logindatas;
            InitializeComponent();//komponensek beállítása.
            buttonColors("");//gombszínek osztály indítása a program futással.
            scaleTransform = new ScaleTransform();//Metódus kezelése.
            logoIMG.RenderTransform = scaleTransform;//Kezelő hozzácsatolása a ScaleTransform-hoz.
            logoIMG.MouseEnter += Image_MouseEnter;//Eseménykezelő dekrelálás.
            logoIMG.MouseLeave += Image_MouseLeave;//Eseménykezelő dekrelálás.
            try
            {
                Version version = Assembly.GetEntryAssembly().GetName().Version;
                string currentVersion = version.ToString();
                appVersion.Content = $"Alkalmazás verzió: v{currentVersion}";
            }
            catch
            {
                appVersion.Content = $"Alkalmazás verzió: Nem olvasható.";
            }

        }


        //Ablak bezárására és a login felület megnyitásárért felelős metódus.
        private void MainAdmin_Closed(object sender, EventArgs e)
        {
            logindatas.Clear();//kiürítjük a bejelentkezésnél felvett adatok listáját az összeakadás vagy telítettség elkerülése végett.
            MainWindow loginWindow = new MainWindow();//Dekralálom az új ablakot.
            loginWindow.Show();//Megjelenítjük az új ablakot.
            this.Close();//Bezárjuk a jelenlegit.
        }

        //nézzet vezérlés metódusa switch : case szerkezettel.
        private async void View_Switch(object sender, RoutedEventArgs e)
        {
            switch (((Button)sender).Content.ToString())//switch elágazás az aktuális nézzet betöltésének ellenörzésére
            {
                case "Főoldal":
                    MainViewControl.Content = new MainPageView(logindatas);//ha a "főoldal" gomb van kiválasztva, akkor a MainPageView "nézzet" - "kontrol" töltődik be.
                    break;
                case "Felhasználók":
                    UserControlView uv = new UserControlView();
                    var progressWindowU = new ProgressWindow();
                    progressWindowU.Show();
                    try
                    {
                        bool successLoad = await uv.LoadData();
                        if (successLoad == true)
                        {
                            MainViewControl.Content = uv;
                            await Task.Delay(200);
                            progressWindowU.Close();
                        }
                        else
                        {
                            await uv.LoadData();
                        }
                    }
                    catch (Exception ex)
                    {
                        await Task.Delay(5000);
                        progressWindowU.Close();
                        ErrorMessageWindow errorMessageWindow = new ErrorMessageWindow();
                        if (ex != null)
                        {
                            bool hasInternet = Backend.Internet.InternetConnectionCheck.IsInternetAvailable();
                            if (hasInternet)
                            {
                                errorMessageWindow.errorTextBlock.Text = $"Hiba történt a nézzet váltás közben!";
                                errorMessageWindow.Show();
                            }
                            else
                            {
                                errorMessageWindow.errorTextBlock.Text = $"Hiba történt!\n\nEllenőrizd az internetkapcsolatodat!";
                                errorMessageWindow.Show();
                            }
                        }
                        else
                        {
                            errorMessageWindow.errorTextBlock.Text = $"Hiba történt!\n\nVáratlan hiba történt!";
                            errorMessageWindow.Show();
                        }
                    }
                    break;
                case "Admin Lista":
                    AdminsView av = new AdminsView(logindatas, this);
                    var progressWindowA = new ProgressWindow();
                    progressWindowA.Show();
                    try
                    {
                        bool successLoad = await av.LoadData();
                        if (successLoad == true)
                        {
                            MainViewControl.Content = av;
                            await Task.Delay(200);
                            progressWindowA.Close();
                        }
                        else
                        {
                            await av.LoadData();
                        }
                    }
                    catch (Exception ex)
                    {
                        await Task.Delay(5000);
                        progressWindowA.Close();
                        ErrorMessageWindow errorMessageWindow = new ErrorMessageWindow();
                        if (ex != null)
                        {
                            bool hasInternet = Backend.Internet.InternetConnectionCheck.IsInternetAvailable();
                            if (hasInternet)
                            {
                                errorMessageWindow.errorTextBlock.Text = $"Hiba történt a nézzet váltás közben!";
                                errorMessageWindow.Show();
                            }
                            else
                            {
                                errorMessageWindow.errorTextBlock.Text = $"Hiba történt!\n\nEllenőrizd az internetkapcsolatodat!";
                                errorMessageWindow.Show();
                            }
                        }
                        else
                        {
                            errorMessageWindow.errorTextBlock.Text = $"Hiba történt!\n\nVáratlan hiba történt!";
                            errorMessageWindow.Show();
                        }
                    }
                    break;
                case "Posztok":
                    PostControlView pv = new PostControlView();
                    var progressWindowP = new ProgressWindow();
                    progressWindowP.Show();
                    try
                    {
                        bool successLoad = await pv.LoadData();
                        if (successLoad == true)
                        {
                            MainViewControl.Content = pv;
                            await Task.Delay(200);
                            progressWindowP.Close();
                        }
                        else
                        {
                            await pv.LoadData();
                        }
                    }
                    catch (Exception ex)
                    {
                        await Task.Delay(5000);
                        progressWindowP.Close();
                        ErrorMessageWindow errorMessageWindow = new ErrorMessageWindow();
                        if (ex != null)
                        {
                            bool hasInternet = Backend.Internet.InternetConnectionCheck.IsInternetAvailable();
                            if (hasInternet)
                            {
                                errorMessageWindow.errorTextBlock.Text = $"Hiba történt a nézzet váltás közben!";
                                errorMessageWindow.Show();
                            }
                            else
                            {
                                errorMessageWindow.errorTextBlock.Text = $"Hiba történt!\n\nEllenőrizd az internetkapcsolatodat!";
                                errorMessageWindow.Show();
                            }
                        }
                        else
                        {
                            errorMessageWindow.errorTextBlock.Text = $"Hiba történt!\n\nVáratlan hiba történt!";
                            errorMessageWindow.Show();
                        }
                    }
                    break;
                case "Market Posztok":
                    MarketPostControlView mv = new MarketPostControlView();
                    var progressWindowMP = new ProgressWindow();
                    progressWindowMP.Show();
                    try
                    {
                        bool successLoad = await mv.LoadData();
                        if (successLoad == true)
                        {
                            MainViewControl.Content = mv;
                            await Task.Delay(200);
                            progressWindowMP.Close();
                        }
                        else
                        {
                            await mv.LoadData();
                        }
                    }
                    catch (Exception ex)
                    {
                        await Task.Delay(5000);
                        progressWindowMP.Close();
                        ErrorMessageWindow errorMessageWindow = new ErrorMessageWindow();
                        if (ex != null)
                        {
                            bool hasInternet = Backend.Internet.InternetConnectionCheck.IsInternetAvailable();
                            if (hasInternet)
                            {
                                errorMessageWindow.errorTextBlock.Text = $"Hiba történt a nézzet váltás közben!";
                                errorMessageWindow.Show();
                            }
                            else
                            {
                                errorMessageWindow.errorTextBlock.Text = $"Hiba történt!\n\nEllenőrizd az internetkapcsolatodat!";
                                errorMessageWindow.Show();
                            }
                        }
                        else
                        {
                            errorMessageWindow.errorTextBlock.Text = $"Hiba történt!\n\nVáratlan hiba történt!";
                            errorMessageWindow.Show();
                        }
                    }
                    break;
                case "Moderáció":
                    MainViewControl.Content = new ModerationView();//ha a "Moderáció" gomb van kiválasztva, akkor a ModerationView "nézzet" - "kontrol" töltődik be.
                    break;
                default:
                    break;
            }
            buttonColors(((Button)sender).Content.ToString());//gomb háttér megváltoztatása az éppen aktuálisra.
        }

        //nézzetváltás hatására gombok felülírása, háttere, szövegszíne
        private void buttonColors(string buttonContent)
        {
            string buttonDefBackGroundHex = "#3A0061";//alapértelmezett hátérszín a gombokon
            string buttonActiveHex = "#000823";//kattintás/váltás hatására az új háttérszíne a gombnak
            string buttonDefForegroundHex = "#FFFFFF";//kattintás/váltás hatására az új szövegszín a gombnak
            string buttonActiveForegroundHex = "#DA34AE";//kattintás/váltás hatására az új szövegszín a gombnak
            Color defaultBrushHex = (Color)ColorConverter.ConvertFromString(buttonDefBackGroundHex);//színkód átalakítása, majd változóba rakása
            Color activeBrushHex = (Color)ColorConverter.ConvertFromString(buttonActiveHex);//színkód átalakítása, új változóba töltése
            Color defaultforegroundHex = (Color)ColorConverter.ConvertFromString(buttonDefForegroundHex);//színkód átalakítása, új változóba töltése
            Color activeforegroundHex = (Color)ColorConverter.ConvertFromString(buttonActiveForegroundHex);//színkód átalakítása, új változóba töltése
            SolidColorBrush defBTBrush = new SolidColorBrush(defaultBrushHex);//új szín dekralárása a programnak
            SolidColorBrush activeBTBrush = new SolidColorBrush(activeBrushHex);//új szín dekralárása a programnak
            SolidColorBrush defBTFore = new SolidColorBrush(defaultforegroundHex);//új szín dekralárása a programnak
            SolidColorBrush activeBTFore = new SolidColorBrush(activeforegroundHex);//új szín dekralárása a programnak


            foreach (var item in ViewSwitchSP.Children)//ciklus a gomb színének kiválasztására, származtatja a gomb címét
            {
                if (item is Button)//ha a kattintott item gomb típusú
                {
                    (item as Button).Background = ((item as Button).Content.ToString() != buttonContent) ? defBTBrush : activeBTBrush;//megváltoztatja a gomb háttérszínét az aktív gombján, az előzőt pedig mindig az alapértelmezettre állítja vissza.
                    (item as Button).Foreground = ((item as Button).Content.ToString() != buttonContent) ? defBTFore : activeBTFore;//megváltoztatja a gomb szövegszínét az aktív gombján, az előzőt pedig mindig az alapértelmezettre állítja vissza.
                }
            }
        }

        private void Image_MouseUp(object sender, MouseButtonEventArgs e)//A kép kattinthatóságáért felelős metódus.
        {
            string url = baseURL.Instance.GlobalURLString;//string típusban dekraláljuk a weboldal címét változóban.
            Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });//Folyamatot indítunk a weboldal megnyitására az alapértelmezett böngészőben, betöltjük a változóban megadott címet.
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)//az ablak mozgatását ellátó metódus.
        {
            if (e.LeftButton == MouseButtonState.Pressed)//ha bal egérgombot letartva mozdítjuk egerünket.
                DragMove();//akkor engedélyezi az ablak mozgatását.
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)//ablak kicsinyítéséért felelős metódus.
        {
            WindowState = WindowState.Minimized;//ablak lekicsinyítése azaz a tálcára helyezést végzi el.
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)//ablak bezárásáért felelős metódus.
        {
            logindatas.Clear();
            Application.Current.Shutdown();//alkalmazás bezárása.
        }

        private void Image_MouseEnter(object sender, MouseEventArgs e)//a képhez közeledő kurzor átalakításáért felelős eljárás.
        {
            Mouse.OverrideCursor = Cursors.Hand;//kurzor átalakítása a megadott értékre, mely az operációs rendszerben meghatározott "Hand" értékhez igazodik.
            DoubleAnimation animation = new DoubleAnimation(ScaleFactor, TimeSpan.FromMilliseconds(200));
            scaleTransform.BeginAnimation(ScaleTransform.ScaleXProperty, animation);
            scaleTransform.BeginAnimation(ScaleTransform.ScaleYProperty, animation);

        }

        private void Image_MouseLeave(object sender, MouseEventArgs e)//a képtől eltávolodott kurzor átalakításáért felelős eljárás.
        {
            Mouse.OverrideCursor = null;//kurzor átalakítása alapértelmezett értékre, mely az operációs rendszer alapértelmezését állítja vissza.
            DoubleAnimation animation = new DoubleAnimation(1.0, TimeSpan.FromMilliseconds(200));
            scaleTransform.BeginAnimation(ScaleTransform.ScaleXProperty, animation);
            scaleTransform.BeginAnimation(ScaleTransform.ScaleYProperty, animation);
        }

        private void logOUT_MouseEnter(object sender, MouseEventArgs e)//Eljárás az egér belépésre a képre
        {
            Mouse.OverrideCursor = Cursors.Hand;//kurzor átalakítása a megadott értékre, mely az operációs rendszerben meghatározott "Hand" értékhez igazodik.
        }

        private void logOUT_MouseLeave(object sender, MouseEventArgs e)//Eljárás az egér elhagyására a képről
        {
            Mouse.OverrideCursor = null;//kurzor átalakítása alapértelmezett értékre, mely az operációs rendszer alapértelmezését állítja vissza.
        }
    }
}
