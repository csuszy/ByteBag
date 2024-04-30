using ByteBagWPF.Backend.AfterLogin;
using ByteBagWPF.Backend.baseURL;
using ByteBagWPF.Frontend.Views.MessageWindow.Edit.EndPointURL;
using ByteBagWPF.Frontend.Views.MessageWindow.Error;
using ByteBagWPF.Frontend.Views.MessageWindow.Progress;
using ByteBagWPF.Views.AdminWindow.MainAdminView;
using NetworkHelper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;



namespace ByteBagWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string url = baseURL.Instance.GlobalURLString;//szerver oldali végpont dekrálása.

        public MainWindow()
        {
            InitializeComponent();//Komponennsek betöltése futáskor.
            PreviewKeyDown += MainWindow_PreviewKeyDown;
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

        private void MainWindow_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btnLogin_Click(sender, e);
            }
        }

        private void editEndpointUrl_Click(object sender, RoutedEventArgs e)
        {
            EditEndpointWindow editEndpointWindow = new EditEndpointWindow();
            editEndpointWindow.Show();
            this.Close();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)//az ablak mozgatását ellátó metódus.
        {
            if (e.LeftButton == MouseButtonState.Pressed)//ha bal egérgombot letartva mozdítjuk egerünket.
                DragMove();//akkor engedélyezi az ablak mozgatását.
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)//ablak kicsinyítéséért felelős metódus.
        {
            WindowState = WindowState.Minimized;//ablak lekicsinyítése azaz a tálcára helyezés végzi el.
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)//ablak bezárásáért felelős metódus.
        {
            Application.Current.Shutdown();//ablak bezárása.
        }

        private void webGyors_Click(object sender, RoutedEventArgs e)//a kattintható link kezelését végző metődus.
        {
            string url = baseURL.Instance.GlobalURLString;//a "MainAdmin.xaml"-ből kiszedjük a "NavigateUri" webhely elérési útját, majd "url" változóban tároljuk azt.
            Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });//Alapértelmezett böngészőben megnyitja a változóba megadott értékén lévő weblapot.
        }

        private async void btnLogin_Click(object sender, RoutedEventArgs e)//bejelentkezést kezelő metódus.
        {
            var progressWindow = new ProgressWindow();
            progressWindow.Show();

            try
            {
                bool successResponse = await LoginMethod();
                if (successResponse == true)
                {
                    await Task.Delay(500);
                    progressWindow.Close();
                }
                else
                {
                    await Task.Delay(800);
                    progressWindow.Close();
                }
            }
            catch (Exception ex)
            {
                await Task.Delay(5000);
                progressWindow.Close();
                ErrorMessageWindow errorMessageWindow = new ErrorMessageWindow();
                if (ex != null)
                {
                    bool hasInternet = Backend.Internet.InternetConnectionCheck.IsInternetAvailable();
                    if (hasInternet)
                    {
                        errorMessageWindow.errorTextBlock.Text = $"Hiba történt!\n\nEllenőrizd a végpont formátumát illetve helyességét!\n\nhttps://pelda.hu";
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
        }

        private async Task<bool> LoginMethod()
        {
            string username = usernameTB.Text;//az usernameTB Textbox-ból kiszedi az értéket és tárolja változóban.
            string password = passwordPB.Password.ToString();//az passwordPB PasswordBox-ból kiszedi az értéket és tárolja változóban.

            List<LoginResponseClass> logindatas = new List<LoginResponseClass>();//Lista dekralárása a LoginResponse osztályhoz.

            try
            {
                if (usernameTB.Text != "" && passwordPB.Password.ToString() != "")//elágazás az üres mezők elkerülésére.
                {
                    Response response = await Task.Run(() =>
                    {
                        return NetworkHelper.Backend.POST(url + "/wpflogin").Body(new
                        {
                            loginusername = username,
                            loginpassword = password
                        }).Send();//Könyvtárban megírt POST metódussal elküldjük a mezőkbe beírt adatokat.
                    });

                    string responseData = response.Message.ToString();//a beérkező message tárolása változóban.

                    //Próba megoldás (működik.)
                    string[] tmp = responseData.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);//beérkező válasz split-elése a pontosvesszők mentén.
                    LoginResponseClass loginResponse = new LoginResponseClass(responseData);//a feldarabolt adatok elküldése a LoginResponseClass számára.
                    logindatas.Add(loginResponse);//A splitelt adatok listába küldése.
                    //MessageBox.Show(responseData);
                    if (tmp[0] == "hitelesitve")//Ha a válasz első eleme tartalmazza a "hitelesitett" szót, azaz a szerver oldalon jóváhagyott felhasználókat engedi be csak.
                    {
                        string enteredPassword = password;
                        MainAdmin mainWindow = new MainAdmin(logindatas);//létrehozzuk a Fő ablakot a programnak.
                                                                         //Application.Current.MainWindow.Close();
                        this.Close();//A Loginpanelt bezárjuk.
                        mainWindow.Show();//A főablakot megnyitjuk.
                        return true;
                    }
                    else//Ha a felhasználó nem hitelesített.
                    {
                        ErrorMessageWindow errorMessageWindow = new ErrorMessageWindow();//Akkor létrehozzuk a hibaüzenet ablakát.
                        errorMessageWindow.LabelContent = $"Hiba történt! \n\n{response.Message.ToString()}";//A szervertől kapott választ a Label Content tulajdonságába felvisszük String formájában.
                        errorMessageWindow.Show();//Megjelenítjük az ablakot.
                        return false;
                    }
                }
                else//üres mezők esetén.
                {
                    ErrorMessageWindow errorMessageWindow = new ErrorMessageWindow();//létrehozzuk számára az ablakot.
                    errorMessageWindow.LabelContent = "Hiba történt! \n\nMinden mezőt ki kell tölteni!";//A hibaüzenet ablakjában található Label Content megváltoztatása a kívánt felirattal.
                    errorMessageWindow.Show();//megjelenítjük az ablakot.
                    return false;
                }
            }
            catch (Exception ex)
            {
                await Task.Delay(5000);
                ErrorMessageWindow errorMessageWindow = new ErrorMessageWindow();
                if (ex != null)
                {
                    bool hasInternet = Backend.Internet.InternetConnectionCheck.IsInternetAvailable();
                    if (hasInternet)
                    {
                        errorMessageWindow.errorTextBlock.Text = $"Hiba történt!\n\nEllenőrizd a végpont formátumát illetve helyességét!\n\nhttps://pelda.hu";
                        errorMessageWindow.Show();
                        return false;
                    }
                    else
                    {
                        errorMessageWindow.errorTextBlock.Text = $"Hiba történt!\n\nEllenőrizd az internetkapcsolatodat!";
                        errorMessageWindow.Show();
                        return false;
                    }
                }
                else
                {
                    errorMessageWindow.errorTextBlock.Text = $"Hiba történt!\n\nVáratlan hiba történt!";
                    errorMessageWindow.Show();
                    return false;
                }
            }
        }
    }
}
