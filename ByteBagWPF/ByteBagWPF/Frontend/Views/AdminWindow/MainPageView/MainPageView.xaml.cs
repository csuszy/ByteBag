using ByteBagWPF.Backend.AfterLogin;
using ByteBagWPF.Backend.baseURL;
using ByteBagWPF.Backend.GetUserClass;
using ByteBagWPF.Frontend.Views.MessageWindow.Error;
using ByteBagWPF.Frontend.Views.MessageWindow.Ok;
using NetworkHelper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ByteBagWPF.Frontend.Views.AdminWindow.MainPageView
{
    /// <summary>
    /// Interaction logic for MainPageView.xaml
    /// </summary>
    public partial class MainPageView : UserControl
    {
        private string url = baseURL.Instance.GlobalURLString + "/user/";//szerver oldali végpont dekrálása.
        private int userID;
        private List<LoginResponseClass> logindatas;//Lista hivatkozása
        public List<GetUser> users;

        public MainPageView(List<LoginResponseClass> logindatas)//A nézzet a kapott paraméter átvétellel.
        {
            this.logindatas = logindatas;//paraméter hivatkozása.
            InitializeComponent();//komponensek betöltése.
            Loaded += LoadData;
        }

        private void LoadData(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (LoginResponseClass loginData in logindatas)//Foreach-el végigmegyünk a listán, változóba helyezzük a szükséges adatokat, majd beírjuk a megfelelő mezőbe.
                {
                    userID = loginData.userID;
                    string username = loginData.userName;
                    string email = loginData.Email;
                    usernameTB.Text = username;
                    usernameEMAIL.Text = email;
                    oldpasswordPB.Password = "*******";
                }
            }
            catch (Exception ex)
            {
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

        public async Task ReFreshData()
        {
            try
            {
                Response response = await Task.Run(() =>
                {
                    return NetworkHelper.Backend.GET(url + userID).Send();
                });

                if (response.StatusCode == StatusCode.OK)
                {
                    return;
                }
                else
                {
                    ErrorMessageWindow errorMessageWindow = new ErrorMessageWindow();//Akkor létrehozzuk a hibaüzenet ablakát.
                    errorMessageWindow.LabelContent = $"Hiba történt! \n\n{response.Message.ToString()}";//A szervertől kapott választ a Label Content tulajdonságába felvisszük String formájában.
                    errorMessageWindow.Show();//Megjelenítjük az ablakot.
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


        private async void updateBT_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            string username = usernameTB.Text;
            string password = passwordChangePB.Password;
            string url = baseURL.Instance.GlobalURLString + "/user/" + userID;
            try
            {
                Response response = await Task.Run(() =>
                {
                    return NetworkHelper.Backend.PUT(url).Body(new
                    {
                        admin = 1,
                        username = username,
                        newpassword = password
                    }).Send();
                });

                if (response.StatusCode == StatusCode.OK)
                {
                    OkayMessageWindow okayMessageWindow = new OkayMessageWindow();
                    passwordChangePB.Password = "";
                    okayMessageWindow.LabelContent = "Sikeres művelet! \n\nMódosítottad az adataid!";
                    okayMessageWindow.Show();
                    await Task.Run(async () =>
                    {
                        await ReFreshData();
                    });
                }
                else
                {
                    ErrorMessageWindow errorMessageWindow = new ErrorMessageWindow();//Akkor létrehozzuk a hibaüzenet ablakát.
                    errorMessageWindow.LabelContent = $"Hiba történt! \n\n{response.Message.ToString()}";//A szervertől kapott választ a Label Content tulajdonságába felvisszük String formájában.
                    errorMessageWindow.Show();//Megjelenítjük az ablakot.
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
    }
}
