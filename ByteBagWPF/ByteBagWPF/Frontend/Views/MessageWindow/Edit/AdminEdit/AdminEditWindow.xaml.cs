using ByteBagWPF.Backend.baseURL;
using ByteBagWPF.Backend.GetUserClass.SendAdminDatas;
using ByteBagWPF.Frontend.Views.AdminWindow.AdminsView;
using ByteBagWPF.Frontend.Views.MessageWindow.Error;
using ByteBagWPF.Frontend.Views.MessageWindow.Ok;
using ByteBagWPF.Frontend.Views.MessageWindow.Progress;
using NetworkHelper;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ByteBagWPF.Frontend.Views.MessageWindow.Edit.AdminEdit
{
    /// <summary>
    /// Interaction logic for AdminEditWindow.xaml
    /// </summary>
    public partial class AdminEditWindow : Window
    {
        private AdminDatas selectedAdminData;
        private AdminsView adminsView;
        public AdminEditWindow(Backend.GetUserClass.SendAdminDatas.AdminDatas selectedAdminData, AdminWindow.AdminsView.AdminsView adminsView)
        {
            InitializeComponent();
            this.selectedAdminData = selectedAdminData;
            Loaded += AdminsDatasOnload;
            adminPassCB.ItemsSource = new int[] { 0, 1 };
            adminPassCB.SelectedIndex = 1;
            this.adminsView = adminsView;
            // akárhol le tudod ilyen módon hivatkozni
            int d = (int)adminPassCB.SelectedItem;

        }

        private void AdminsDatasOnload(object sender, RoutedEventArgs e)
        {
            AdminNameTB.Text = "";
            adminEMAIL.Text = "";
            AdminoldpasswordPB.Password = "******";
            AdminpasswordChangePB.Password = "";

            AdminNameTB.Text = selectedAdminData.Name;
            adminEMAIL.Text = selectedAdminData.Email;
            AdminoldpasswordPB.Password = selectedAdminData.OldPassword;
        }

        private void Image_MouseUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private async Task<bool> adminEditTask()
        {
            string newPassword = AdminpasswordChangePB.Password.ToString();
            string newUsername = AdminNameTB.Text;
            int newAdminRole = adminPassCB.SelectedIndex;
            string url = baseURL.Instance.GlobalURLString + "/user/" + selectedAdminData.Id;
            try
            {
                Response response = await Task.Run(() =>
                {
                    return NetworkHelper.Backend.PUT(url).Body(new
                    {
                        admin = newAdminRole,
                        username = newUsername,
                        newpassword = newPassword
                    }).Send();
                });

                if (response.StatusCode.ToString() == "OK")
                {
                    OkayMessageWindow okayMessageWindow = new OkayMessageWindow();
                    okayMessageWindow.Show();
                    this.Close();
                    return true;
                }
                else
                {
                    ErrorMessageWindow errorMessageWindow = new ErrorMessageWindow();//Akkor létrehozzuk a hibaüzenet ablakát.
                    errorMessageWindow.LabelContent = $"Hiba történt! \n\n{response.Message.ToString()}";//A szervertől kapott választ a Label Content tulajdonságába felvisszük String formájában.
                    errorMessageWindow.Show();//Megjelenítjük az ablakot.
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

        private async void okBT_Click(object sender, RoutedEventArgs e)
        {
            var progressWindow = new ProgressWindow();
            progressWindow.Show();
            try
            {
                bool successLoad = await adminsView.LoadData();
                bool successDataUP = await adminEditTask();
                if (successLoad == true && successDataUP == true)
                {
                    await Task.Delay(200);
                    progressWindow.Close();
                }
                else
                {
                    await adminsView.LoadData();
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

        private void undoBT_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
