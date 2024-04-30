using ByteBagWPF.Backend.baseURL;
using ByteBagWPF.Backend.GetUserClass;
using ByteBagWPF.Backend.GetUserClass.SendUserDatas;
using ByteBagWPF.Frontend.Views.MessageWindow.Delete;
using ByteBagWPF.Frontend.Views.MessageWindow.Edit.UserEdit;
using ByteBagWPF.Frontend.Views.MessageWindow.Error;
using ByteBagWPF.Frontend.Views.MessageWindow.Progress;
using NetworkHelper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ByteBagWPF.Frontend.Views.AdminWindow.UserControlView
{
    /// <summary>
    /// Interaction logic for UserControlView.xaml
    /// </summary>
    public partial class UserControlView : UserControl
    {
        private string userEndpoint = baseURL.Instance.GlobalURLString;//szerver oldali végpont dekrálása.
        public List<GetUser> users;
        UserDatas selectedUserData;

        public UserControlView()//Nézzet betöltésekor.
        {
            InitializeComponent();//Komponensek betöltése.
            Task.Run(async () =>
            {
                await LoadData();
            });

        }

        private async void userRefreshBT_Click(object sender, System.Windows.RoutedEventArgs e)//Lista frissítésére szolgáló eljárás.
        {

            var progressWindow = new ProgressWindow();
            progressWindow.Show();

            try
            {
                bool successResponse = await ReFreshButton();
                bool successLoad = await LoadData();
                if (successResponse == true)
                {
                    await Task.Delay(500);
                    progressWindow.Close();
                }
                else
                {
                    await ReFreshButton();
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

        public async Task<bool> ReFreshButton()
        {

            userListLB.Items.Clear();
            usereditBT.IsEnabled = false;
            userDeleteBT.IsEnabled = false;

            try
            {
                Response response = await Task.Run(() =>
                {
                    return NetworkHelper.Backend.GET(userEndpoint + "/onlyuser").Send();
                });
                string responseData = response.Message.ToString();//Válasz feldolgozása.
                List<GetUser> users = JsonConvert.DeserializeObject<List<GetUser>>(responseData);//Json konvertálás után listába töltés.

                if (response != null)//Ha nem üres a válasz.
                {
                    foreach (GetUser user in users)//Foreach-el adatok változóba töltése tulajdonság alapján.
                    {
                        int userID = user.userID;
                        string username = user.username;
                        string userEmail = user.email;
                        DateTime regDate = user.registerDATE;
                        int adminE = user.admin;
                    }

                    if (users.Count > 0)//Ha a lista nem üres, vagy a végéig nem értünk.
                    {
                        foreach (var user in users)//Foreach-el adatok feltöltése a listboxba itemként.
                        {
                            userListLB.Items.Add($"Azonosító: {user.userID} - Felhasználónév: {user.username}\n\nE-mail: {user.email}" +
                                $"\nRegisztráció dátuma: {user.registerDATE}");

                        }
                        return true;
                    }
                    else//Üres lista esetén.
                    {
                        ErrorMessageWindow errorMessageWindow = new ErrorMessageWindow();//létrehozzuk számára az ablakot.
                        errorMessageWindow.LabelContent = "Hiba történt! \n\nNem található felhasználó a listában!";//A hibaüzenet ablakjában található Label Content megváltoztatása a kívánt felirattal.
                        errorMessageWindow.Show();//megjelenítjük az ablakot.
                        return false;
                    }
                }
                else//Üres válasz esetén.
                {
                    ErrorMessageWindow errorMessageWindow = new ErrorMessageWindow();//létrehozzuk számára az ablakot.
                    errorMessageWindow.LabelContent = "Hiba történt! \n\nÜres válasz érkezett!";//A hibaüzenet ablakjában található Label Content megváltoztatása a kívánt felirattal.
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

        public async Task<bool> LoadData()
        {
            userListLB.Items.Clear();//Listbox itemjeinek törlése fentmaradók esetén.

            try
            {
                Response response = await Task.Run(() =>
                {
                    return NetworkHelper.Backend.GET(userEndpoint + "/onlyuser").Send();
                });

                string responseData = response.Message.ToString();//Válasz konvertálása.
                List<GetUser> users = JsonConvert.DeserializeObject<List<GetUser>>(responseData);//Válasz json konvertálása, listába töltése.

                if (response != null)//Üres válasz esetén.
                {
                    foreach (GetUser user in users)//foreach a lista adatainak változóba töltésére, osztály tulajdonságai szerint.
                    {
                        int userID = user.userID;
                        string username = user.username;
                        string userEmail = user.email;
                        DateTime regDate = user.registerDATE;
                        int adminE = user.admin;
                    }

                    if (users.Count > 0)//Ha a lista nem üres és nem értünk a lista végére.
                    {
                        foreach (var user in users)//Feltöltjük a listboxot itemekkel.
                        {
                            userListLB.Items.Add($"Azonosító: {user.userID} - Felhasználónév: {user.username}\n\nE-mail: {user.email}" +
                                $"\nRegisztráció dátuma: {user.registerDATE}");
                        }
                        return true;
                    }
                    else//Üres lista esetén.
                    {
                        ErrorMessageWindow errorMessageWindow = new ErrorMessageWindow();//létrehozzuk számára az ablakot.
                        errorMessageWindow.LabelContent = "Hiba történt! \n\nNem található felhasználó a listában!";//A hibaüzenet ablakjában található Label Content megváltoztatása a kívánt felirattal.
                        errorMessageWindow.Show();//megjelenítjük az ablakot.
                        return false;
                    }
                }
                else//Üres válasz esetén.
                {
                    ErrorMessageWindow errorMessageWindow = new ErrorMessageWindow();//létrehozzuk számára az ablakot.
                    errorMessageWindow.LabelContent = "Hiba történt! \n\nÜres válasz érkezett!";//A hibaüzenet ablakjában található Label Content megváltoztatása a kívánt felirattal.
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

        private void userDeleteBT_Click(object sender, RoutedEventArgs e)//Törlés gomb
        {
            try
            {
                if (userListLB.SelectedItem != null)//Kiválasztott item esetében.
                {
                    DeleteMessageWindow deleteUser = new DeleteMessageWindow(selectedUserData, this);//létrehozzuk számára az ablakot.
                    deleteUser.Show();//megjelenítjük az ablakot.
                }
                else//Ha nincs kiválasztva item.
                {
                    ErrorMessageWindow errorMessageWindow = new ErrorMessageWindow();//létrehozzuk számára az ablakot.
                    errorMessageWindow.LabelContent = $"Hiba történt!\nNincs kiválasztva felhasználó!";//A hibaüzenet ablakjában található Label Content megváltoztatása a kívánt felirattal.
                    errorMessageWindow.Show();//megjelenítjük az ablakot.
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

        private void userListLB_SelectionChanged(object sender, SelectionChangedEventArgs e)//Amennyiben megváltozik a kiválasztott item értéke, tehát ki van jelölve item, akkor--->
        {
            //Gombok elérhetősége megváltozik.
            usereditBT.IsEnabled = true;
            userDeleteBT.IsEnabled = true;
            try
            {
                //Ha van kijelölve item.
                if (userListLB.SelectedItem != null)
                {
                    //A listbox itemének konvertálása string-é.
                    string selectedUserLine = userListLB.SelectedItem.ToString();
                    //Sor splitelése.
                    string[] userProperties = selectedUserLine.ToString().Split('\n', ' ');

                    //sor üres karaktereinek kiszedése Trimmeléssel.
                    for (int i = 0; i < userProperties.Length; i++)
                    {
                        userProperties[i] = userProperties[i].Trim();
                    }

                    //tömb elemeinek globális, osztály típusú objektum lesz, amelynek értékei változóba lesznek töltve.
                    selectedUserData = new UserDatas
                    {
                        Id = int.Parse(userProperties[1]),
                        Name = userProperties[4],
                        Email = userProperties[7],
                        OldPassword = "******"
                    };
                }
                else
                {//Ha nincs kijelölve, hiba nélkül kilép.
                    return;
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

        private void usereditBT_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (userListLB.SelectedItem != null)//Kiválasztott item esetében.
                {
                    if (selectedUserData != null)
                    {
                        UserEditWindow userEditWindow = new UserEditWindow(selectedUserData, this);
                        userEditWindow.Show();

                    }
                    else
                    {
                        MessageBox.Show("Nem választottál ki semmit!");
                    }

                }
                else//Ha nincs kiválasztva item.
                {
                    ErrorMessageWindow errorMessageWindow = new ErrorMessageWindow();//létrehozzuk számára az ablakot.
                    errorMessageWindow.LabelContent = $"Hiba történt!\nNincs kiválasztva felhasználó!";//A hibaüzenet ablakjában található Label Content megváltoztatása a kívánt felirattal.
                    errorMessageWindow.Show();//megjelenítjük az ablakot.
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
    }
}
