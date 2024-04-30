using ByteBagWPF.Backend.baseURL;
using ByteBagWPF.Backend.GetPostClass.SendPostDatas;
using ByteBagWPF.Frontend.Views.AdminWindow.PostControlView;
using ByteBagWPF.Frontend.Views.MessageWindow.Error;
using ByteBagWPF.Frontend.Views.MessageWindow.Ok;
using ByteBagWPF.Frontend.Views.MessageWindow.Progress;
using NetworkHelper;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ByteBagWPF.Frontend.Views.MessageWindow.Edit.PostEdit.ForumPost
{
    /// <summary>
    /// Interaction logic for ForumPostEdit.xaml
    /// </summary>
    public partial class ForumPostEdit : Window
    {
        private PostDatas selectedPostData;
        private PostControlView postControlView;
        public ForumPostEdit(Backend.GetPostClass.SendPostDatas.PostDatas selectedPostData, AdminWindow.PostControlView.PostControlView postControlView)
        {
            this.selectedPostData = selectedPostData;
            Loaded += PostDatasOnload;
            this.postControlView = postControlView;
            InitializeComponent();
        }

        private void PostDatasOnload(object sender, RoutedEventArgs e)
        {
            UserNameTB.Text = "";
            PostHeaderTB.Text = "";
            editedPostParagraph.Text = "";

            UserNameTB.Text = selectedPostData.userName;
            PostHeaderTB.Text = selectedPostData.title;
            editedPostParagraph.Text = selectedPostData.post;
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

        private void postEditUndoBT_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private async void postEditokBT_Click(object sender, RoutedEventArgs e)
        {
            string url = baseURL.Instance.GlobalURLString + "/wpfupdatepost";//szerver oldali végpont dekrálása.
            try
            {
                Response response = NetworkHelper.Backend.POST(url).Body(new
                {
                    editposztID = selectedPostData.posztID,
                    newtitle = PostHeaderTB.Text,
                    newpost = editedPostParagraph.Text
                }).Send();

                if (response.StatusCode == StatusCode.OK)
                {
                    OkayMessageWindow okayMessageWindow = new OkayMessageWindow();
                    okayMessageWindow.Show();
                    this.Close();
                    var progressWindow = new ProgressWindow();
                    progressWindow.Show();
                    try
                    {
                        bool successLoad = await postControlView.LoadData();
                        if (successLoad == true)
                        {
                            await Task.Delay(200);
                            progressWindow.Close();
                        }
                        else
                        {
                            await postControlView.LoadData();
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
                else
                {
                    ErrorMessageWindow errorMessageWindow = new ErrorMessageWindow();//Akkor létrehozzuk a hibaüzenet ablakát.
                    errorMessageWindow.LabelContent = $"Hiba történt! \n\n{response.StatusCode}";//A szervertől kapott választ a Label Content tulajdonságába felvisszük String formájában.
                    errorMessageWindow.Show();//Megjelenítjük az ablakot.
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

        private void editedPostParagraph_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && (Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift)
            {
                var textBox = (TextBox)sender;
                int caretIndex = textBox.CaretIndex;
                textBox.Text = textBox.Text.Insert(caretIndex, Environment.NewLine);
                textBox.CaretIndex = caretIndex + Environment.NewLine.Length;
                e.Handled = true;
            }
        }
    }
}
