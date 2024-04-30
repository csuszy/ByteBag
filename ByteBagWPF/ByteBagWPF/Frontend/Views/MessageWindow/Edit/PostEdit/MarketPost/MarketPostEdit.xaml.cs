using ByteBagWPF.Backend.baseURL;
using ByteBagWPF.Backend.GetMArketPostClass.SendMarketDatas;
using ByteBagWPF.Frontend.Views.AdminWindow.MarketPostControlView;
using ByteBagWPF.Frontend.Views.MessageWindow.Error;
using ByteBagWPF.Frontend.Views.MessageWindow.Ok;
using ByteBagWPF.Frontend.Views.MessageWindow.Progress;
using NetworkHelper;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ByteBagWPF.Frontend.Views.MessageWindow.Edit.PostEdit.MarketPost
{
    /// <summary>
    /// Interaction logic for MarketPostEdit.xaml
    /// </summary>
    public partial class MarketPostEdit : Window
    {

        private MarketDatas selectedMarketPostData;
        private MarketPostControlView marketPostControlView;
        public MarketPostEdit(Backend.GetMArketPostClass.SendMarketDatas.MarketDatas selectedMarketPostData, AdminWindow.MarketPostControlView.MarketPostControlView marketPostControlView)
        {
            InitializeComponent();
            Loaded += OnLoadMarketData;
            this.marketPostControlView = marketPostControlView;
            this.selectedMarketPostData = selectedMarketPostData;
        }

        private void OnLoadMarketData(object sender, RoutedEventArgs e)
        {
            UserNameTB.Text = "";
            PriceTB.Text = "";
            PostHeaderTB.Text = "";
            editedPostParagraph.Text = "";

            UserNameTB.Text = selectedMarketPostData.userName;
            PriceTB.Text = selectedMarketPostData.price.ToString("F0");
            PostHeaderTB.Text = selectedMarketPostData.title;
            editedPostParagraph.Text = selectedMarketPostData.post;
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
            string url = baseURL.Instance.GlobalURLString + "/wpfupdatemarketpost";//szerver oldali végpont dekrálása.
            try
            {
                int priceToInt = int.Parse(PriceTB.Text);
                Response response = NetworkHelper.Backend.POST(url).Body(new
                {
                    editposztID = selectedMarketPostData.posztID,
                    newtitle = PostHeaderTB.Text,
                    newpost = editedPostParagraph.Text,
                    editprice = priceToInt
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
                        bool successLoad = await marketPostControlView.LoadData();
                        if (successLoad == true)
                        {
                            await Task.Delay(200);
                            progressWindow.Close();
                        }
                        else
                        {
                            await marketPostControlView.LoadData();
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
