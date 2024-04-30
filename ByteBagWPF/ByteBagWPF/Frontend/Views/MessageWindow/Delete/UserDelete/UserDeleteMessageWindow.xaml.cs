using ByteBagWPF.Backend.baseURL;
using ByteBagWPF.Backend.GetUserClass.SendUserDatas;
using ByteBagWPF.Frontend.Views.AdminWindow.UserControlView;
using ByteBagWPF.Frontend.Views.MessageWindow.Error;
using ByteBagWPF.Frontend.Views.MessageWindow.Ok;
using ByteBagWPF.Frontend.Views.MessageWindow.Progress;
using NetworkHelper;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ByteBagWPF.Frontend.Views.MessageWindow.Delete
{
    /// <summary>
    /// Interaction logic for DeleteMessageWindow.xaml
    /// </summary>
    public partial class DeleteMessageWindow : Window
    {
        private UserControlView userControlView;
        private UserDatas selectedUserData;
        public string LabelContent //publikus változó, hogy bármelyik ablakban módosítható legyen a label-je a törlés ablaknak.
        {
            get { return deleteWindowLabel.Content.ToString(); }//Egy stringé konvertált content-el tér vissza.
            set { deleteWindowLabel.Content = value; }//beállítjuk a label contentjének értékét.
        }
        public DeleteMessageWindow(UserDatas selectedUserData, AdminWindow.UserControlView.UserControlView userControlView)
        {
            InitializeComponent();//Komponensek inícializálása.
            this.selectedUserData = selectedUserData;
            LabelContent = $"Biztosan törlöd\n{selectedUserData.Name} felhasználót?";
            this.userControlView = userControlView;
        }

        public DeleteMessageWindow(Backend.GetUserClass.SendAdminDatas.AdminDatas selectedAdminData)
        {
        }

        public DeleteMessageWindow()
        {
        }

        private void Image_MouseUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)//Ablak vonszolása
        {
            if (e.LeftButton == MouseButtonState.Pressed)//Ha a bal egérgombot nyomva tartjuk,
                DragMove();//Mozdíthatóvá válik az ablak.
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)//Kicsinyítés.
        {
            WindowState = WindowState.Minimized;//Ablak kicsinyítése, értsd tálcára helyezése.
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)//Ablak bezárása.
        {
            this.Close();//Bezárás.
        }

        private async void okBT_Click(object sender, RoutedEventArgs e)//Ok gomb esetén bezárja az ablakot.
        {
            string deleteEndpointURL = baseURL.Instance.GlobalURLString + "/user/" + selectedUserData.Id;//végpont
            try
            {
                Response response = await Task.Run(() =>
                {
                    return NetworkHelper.Backend.DELETE(deleteEndpointURL).Send();
                });
                string responseData = response.StatusCode.ToString();
                if (response.StatusCode.ToString() == "OK")
                {
                    OkayMessageWindow okayMessageWindow = new OkayMessageWindow();
                    okayMessageWindow.Show();
                    this.Close();
                    var progressWindow = new ProgressWindow();
                    progressWindow.Show();
                    try
                    {
                        bool successLoad = await userControlView.LoadData();
                        if (successLoad == true)
                        {
                            await Task.Delay(200);
                            progressWindow.Close();
                        }
                        else
                        {
                            await userControlView.LoadData();
                        }
                    }
                    catch (Exception ex)
                    {
                        await Task.Delay(5000);
                        progressWindow.Close();
                        ErrorMessageWindow errorMessageWindow = new ErrorMessageWindow();
                        if (ex != null)
                        {
                            errorMessageWindow.LabelContent = $"\n\nHiba történt!";//A hibaüzenet ablakjában található Label Content megváltoztatása a kívánt felirattal.
                            errorMessageWindow.Show();//megjelenítjük az ablakot.
                        }
                        else
                        {
                            errorMessageWindow.errorTextBlock.Text = $"Hiba történt!\n\nVáratlan hiba történt!";
                            errorMessageWindow.Show();
                        }
                    }
                }
                else
                {
                    ErrorMessageWindow errorMessageWindow = new ErrorMessageWindow();//létrehozzuk számára az ablakot.
                    errorMessageWindow.LabelContent = $"\n\nHiba történt!";//A hibaüzenet ablakjában található Label Content megváltoztatása a kívánt felirattal.
                    errorMessageWindow.Show();//megjelenítjük az ablakot.
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

        private void undoBT_Click(object sender, RoutedEventArgs e)//Visszavonás
        {
            this.Close();//Ablak bezárása.
        }
    }
}
