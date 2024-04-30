using ByteBagWPF.Backend.AfterLogin;
using ByteBagWPF.Backend.baseURL;
using ByteBagWPF.Backend.GetAdminClass;
using ByteBagWPF.Backend.GetUserClass.SendAdminDatas;
using ByteBagWPF.Frontend.Views.MessageWindow.Delete.AdminDelete;
using ByteBagWPF.Frontend.Views.MessageWindow.Edit.AdminEdit;
using ByteBagWPF.Frontend.Views.MessageWindow.Error;
using ByteBagWPF.Frontend.Views.MessageWindow.Progress;
using ByteBagWPF.Views.AdminWindow.MainAdminView;
using NetworkHelper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ByteBagWPF.Frontend.Views.AdminWindow.AdminsView
{
    /// <summary>
    /// Interaction logic for AdminsView.xaml
    /// </summary>
    public partial class AdminsView : UserControl
    {

        private string adminEndpoint = baseURL.Instance.GlobalURLString;//szerver oldali végpont dekrálása.
        public List<GetAdmin> admins;
        AdminDatas selectedAdminData;
        public object lockObject = new object();
        private List<LoginResponseClass> logindatas;
        MainAdmin mainAdmin;

        public AdminsView(List<Backend.AfterLogin.LoginResponseClass> logindatas, ByteBagWPF.Views.AdminWindow.MainAdminView.MainAdmin mainAdmin)//A nézzet betöltésekor.
        {
            InitializeComponent();
            this.logindatas = logindatas;
            this.mainAdmin = mainAdmin;
            Task.Run(async () =>
            {
                await LoadData();
            });
        }

        private async void adminrefreshBT_Click(object sender, System.Windows.RoutedEventArgs e)//Lista frissítésére szolgáló eljárás.
        {
            var progressWindow = new ProgressWindow();
            progressWindow.Show();

            try
            {
                bool successResponse = await RefreshButton();
                if (successResponse == true)
                {
                    await Task.Delay(500);
                    progressWindow.Close();
                }
                else
                {
                    await RefreshButton();
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

        public async Task<bool> RefreshButton()
        {
            adminListLB.Items.Clear();//Lista itemek törlése az ismétlődések illetve a régi adatok eltűntetésére.
            adminDeleteBT.IsEnabled = false;//Gombok alapértékének megváltoztatása, mivel frissítésnél nincs kijelölve item.
            AdminEditBT.IsEnabled = false;

            try
            {
                Response response = await Task.Run(() =>
                {
                    return NetworkHelper.Backend.GET(adminEndpoint + "/onlyadmin").Send();
                });

                string responseData = response.Message.ToString();//Beérkező válasz konvertálása.
                List<GetAdmin> admins = JsonConvert.DeserializeObject<List<GetAdmin>>(responseData);//Json konvertálása.
                if (response != null)//üres válasz esetén féltétel.
                {
                    foreach (GetAdmin admin in admins)//beérkezett válasz splitelve az osztály konstruktorával illetve változókba mentése.
                    {
                        int userID = admin.userID;
                        string username = admin.userName;
                        string userEmail = admin.email;
                        DateTime regDate = admin.registerDATE;
                        int adminE = admin.adminE;
                    }

                    if (admins.Count > 0)//Ha van admin.
                    {
                        foreach (var admin in admins)//Adatok listboxba adása foreach-al a lista végéig.
                        {
                            adminListLB.Items.Add($"Admin azonosító: {admin.userID} - Felhasználónév: {admin.userName}\n\nE-mail: {admin.email}" +
                                $"\nRegisztráció dátuma: {admin.registerDATE}");
                        }
                        return true;
                    }
                    else//Üres lista esetén.
                    {
                        ErrorMessageWindow errorMessageWindow = new ErrorMessageWindow();//létrehozzuk számára az ablakot.
                        errorMessageWindow.LabelContent = "Hiba történt! \n\nNem található admin a listában!";//A hibaüzenet ablakjában található Label Content megváltoztatása a kívánt felirattal.
                        errorMessageWindow.Show();//megjelenítjük az ablakot.
                        return false;
                    }
                }
                else//Üres válasz esetén.
                {
                    ErrorMessageWindow errorMessageWindow = new ErrorMessageWindow();//létrehozzuk számára az ablakot.
                    errorMessageWindow.LabelContent = "Hiba történt! \n\nNem található admin a listában!";//A hibaüzenet ablakjában található Label Content megváltoztatása a kívánt felirattal.
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
            adminListLB.Items.Clear();//Esetleges előző itemek eltűntetése ismétlődés elkerülése végett.

            try
            {
                Response response = await Task.Run(() =>
                {
                    return NetworkHelper.Backend.GET(adminEndpoint + "/onlyadmin").Send();
                });

                string responseData = response.Message.ToString();//Válasz konvert.
                List<GetAdmin> admins = JsonConvert.DeserializeObject<List<GetAdmin>>(responseData);//Json konvert.

                if (response != null)//Ha nem üres.
                {
                    foreach (GetAdmin admin in admins)//Foreach-al változóba töltés a GetAdmin osztály típusú listával.
                    {
                        int userID = admin.userID;
                        string username = admin.userName;
                        string userEmail = admin.email;
                        DateTime regDate = admin.registerDATE;
                        int adminE = admin.adminE;
                    }


                    if (admins.Count > 0)//Ha nem üres a lista.
                    {
                        foreach (var admin in admins)//Foreach-el listbox feltöltése.
                        {
                            adminListLB.Items.Add($"Admin azonosító: {admin.userID} - Felhasználónév: {admin.userName}\n\nE-mail: {admin.email}" +
                               $"\nRegisztráció dátuma: {admin.registerDATE}");
                        }
                        return true;
                    }
                    else //Ha üres a lista, hibaüzenet egyedi ablakban.
                    {
                        ErrorMessageWindow errorMessageWindow = new ErrorMessageWindow();//létrehozzuk számára az ablakot.
                        errorMessageWindow.LabelContent = "Hiba történt! \n\nNem található admin a listában!";//A hibaüzenet ablakjában található Label Content megváltoztatása a kívánt felirattal.
                        errorMessageWindow.Show();//megjelenítjük az ablakot.
                        return false;
                    }
                }
                else//Üres válasz esetén.
                {
                    ErrorMessageWindow errorMessageWindow = new ErrorMessageWindow();//létrehozzuk számára az ablakot.
                    errorMessageWindow.LabelContent = $"Hiba történt!\nÜres válasz érkezett a szervertől!";//A hibaüzenet ablakjában található Label Content megváltoztatása a kívánt felirattal.
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

        private void AdminEditBT_Click(object sender, RoutedEventArgs e)//Admin adatainak módosítására szolgáló metódus.
        {
            try
            {
                if (adminListLB.SelectedItem != null)
                {
                    AdminEditWindow adminEditWindow = new AdminEditWindow(selectedAdminData, this);//létrehozzuk számára az ablakot.
                    adminEditWindow.Show();//megjelenítjük az ablakot.
                }
                else
                {
                    ErrorMessageWindow errorMessageWindow = new ErrorMessageWindow();//létrehozzuk számára az ablakot.
                    errorMessageWindow.LabelContent = $"Hiba történt!\nNincs kiválasztva Admin felhasználó!";//A hibaüzenet ablakjában található Label Content megváltoztatása a kívánt felirattal.
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

        private void adminListLB_SelectionChanged(object sender, SelectionChangedEventArgs e)//ha megváltozik a listbox itemjeinek kijelölése, akkor a gombok elérhetőségének megváltoztatása.
        {
            adminDeleteBT.IsEnabled = true;
            AdminEditBT.IsEnabled = true;
            try
            {
                //Ha van kijelölve item.
                if (adminListLB.SelectedItem != null)
                {
                    //A listbox itemének konvertálása string-é.
                    string selectedAdminLine = adminListLB.SelectedItem.ToString();
                    //Sor splitelése.
                    string[] adminProperties = selectedAdminLine.ToString().Split('\n', ' ');

                    //sor üres karaktereinek kiszedése Trimmeléssel.
                    for (int i = 0; i < adminProperties.Length; i++)
                    {
                        adminProperties[i] = adminProperties[i].Trim();

                    }

                    //tömb elemeinek globális, osztály típusú objektum lesz, amelynek értékei változóba lesznek töltve.
                    selectedAdminData = new AdminDatas
                    {
                        Id = int.Parse(adminProperties[2]),
                        Name = adminProperties[5],
                        Email = adminProperties[8],
                        OldPassword = "******"
                    };
                }
                else
                {//Ha nincs kijelölve, kilép.
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

        private void adminDeleteBT_Click(object sender, RoutedEventArgs e)//Admin törlése a listából.
        {
            try
            {
                if (adminListLB.SelectedItems != null)//Hibakezelés.
                {

                    AdminDeleteWindow deleteAdmin = new AdminDeleteWindow(selectedAdminData, this, logindatas, mainAdmin);//létrehozzuk számára az ablakot.
                    deleteAdmin.Show();//megjelenítjük az ablakot.
                }
                else
                {
                    ErrorMessageWindow errorMessageWindow = new ErrorMessageWindow();//létrehozzuk számára az ablakot.
                    errorMessageWindow.LabelContent = $"Hiba történt!\nNincs kiválasztva Admin felhasználó!";//A hibaüzenet ablakjában található Label Content megváltoztatása a kívánt felirattal.
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
