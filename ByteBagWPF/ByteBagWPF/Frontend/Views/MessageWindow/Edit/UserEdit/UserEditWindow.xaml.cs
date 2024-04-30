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

namespace ByteBagWPF.Frontend.Views.MessageWindow.Edit.UserEdit
{
    /// <summary>
    /// Interaction logic for UserEditWindow.xaml
    /// </summary>
    public partial class UserEditWindow : Window
    {
        private UserDatas selectedUserData;
        private UserControlView userControlView;

        public UserEditWindow(UserDatas selectedUserData, AdminWindow.UserControlView.UserControlView userControlView)
        {
            InitializeComponent();
            this.selectedUserData = selectedUserData;
            Loaded += UserDatasOnload;
            adminPassCB.ItemsSource = new int[] { 0, 1 };
            adminPassCB.SelectedIndex = 0;
            this.userControlView = userControlView;
            // akárhol le tudod ilyen módon hivatkozni
            int d = (int)adminPassCB.SelectedItem;
        }

        private void UserDatasOnload(object sender, RoutedEventArgs e)
        {
            UserNameTB.Text = "";
            UserEMAILTB.Text = "";
            UserOldpasswordPB.Password = "******";
            UserPasswordChangePB.Password = "";

            UserNameTB.Text = selectedUserData.Name;
            UserEMAILTB.Text = selectedUserData.Email;
            UserOldpasswordPB.Password = selectedUserData.OldPassword;

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

        private async Task<bool> userEditTask()
        {
            string newPassword = UserPasswordChangePB.Password.ToString();
            string newUsername = UserNameTB.Text;
            int newAdminRole = adminPassCB.SelectedIndex;
            string url = baseURL.Instance.GlobalURLString + "/user/" + selectedUserData.Id;
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
                    ErrorMessageWindow errorMessageWindow = new ErrorMessageWindow();//létrehozzuk számára az ablakot.
                    errorMessageWindow.LabelContent = $"\n\nHiba történt!";//A hibaüzenet ablakjában található Label Content megváltoztatása a kívánt felirattal.
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

        private async void UserEditokBT_Click(object sender, RoutedEventArgs e)
        {
            var progressWindow = new ProgressWindow();
            progressWindow.Show();
            try
            {
                bool successLoad = await userControlView.LoadData();
                bool successDataUP = await userEditTask();
                if (successLoad == true && successDataUP == true)
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

        private void UserEditUndoBT_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
