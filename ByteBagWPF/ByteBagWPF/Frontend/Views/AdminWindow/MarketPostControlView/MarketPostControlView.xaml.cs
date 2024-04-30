using ByteBagWPF.Backend.baseURL;
using ByteBagWPF.Backend.GetMArketPostClass;
using ByteBagWPF.Backend.GetMArketPostClass.SendMarketDatas;
using ByteBagWPF.Frontend.Views.MessageWindow.Delete.MarketPostDelete;
using ByteBagWPF.Frontend.Views.MessageWindow.Edit.PostEdit.MarketPost;
using ByteBagWPF.Frontend.Views.MessageWindow.Error;
using ByteBagWPF.Frontend.Views.MessageWindow.Progress;
using NetworkHelper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ByteBagWPF.Frontend.Views.AdminWindow.MarketPostControlView
{
    /// <summary>
    /// Interaction logic for MarketPostControlView.xaml
    /// </summary>
    public partial class MarketPostControlView : UserControl
    {
        private string postEndpoint = baseURL.Instance.GlobalURLString;//szerver oldali végpont dekrálása.
        MarketDatas selectedMarketPostData;

        public MarketPostControlView()//Nézzet hivatkozása.
        {
            InitializeComponent();
            Task.Run(async () =>
            {
                await LoadData();
            });
        }

        public async Task<bool> LoadData()
        {
            postListLB.Items.Clear();//Esetlegesen fennmaradó listbox itemek törlése.

            try
            {
                Response response = await Task.Run(() =>
                {
                    return NetworkHelper.Backend.GET(postEndpoint + "/get-market-ad").Send();
                });

                string responseData = response.Message.ToString();//Válasz kezelése.
                List<GetMarketPost> marketposts = JsonConvert.DeserializeObject<List<GetMarketPost>>(responseData);//Json konvertálás, majd betöltés listába.
                if (response != null)//Ha a válasz nem üres.
                {
                    foreach (GetMarketPost marketpostpost in marketposts)//Lista osztály segítségével való változóba töltés.
                    {
                        int marketID = marketpostpost.marketID;
                        int userID = marketpostpost.userID;
                        string username = marketpostpost.username;
                        string header = marketpostpost.title;
                        DateTime marketDate = marketpostpost.marketDATE;
                        double postPrice = marketpostpost.price;
                        string marketParagraph = marketpostpost.marketpost;
                    }


                    if (marketposts.Count > 0)//Ha a lista nem üres.
                    {
                        foreach (var posts in marketposts)//Lista adatainak kiírása Foreach-el.
                        {
                            postListLB.Items.Add($"Azonosító: {posts.marketID}\nPosztoló neve: {posts.username} ({posts.userID})\n\nCím:\n {posts.title} " +
                                $"\n\nPoszt szövege: \n{posts.marketpost}\n\nPosztolás dátuma: {posts.marketDATE}\nTermék ára: {posts.price} Ft,-");
                        }
                        return true;
                    }
                    else//Ha üres a lista.
                    {
                        ErrorMessageWindow errorMessageWindow = new ErrorMessageWindow();//létrehozzuk számára az ablakot.
                        errorMessageWindow.LabelContent = "Hiba történt! \n\nNem található poszt a listában!";//A hibaüzenet ablakjában található Label Content megváltoztatása a kívánt felirattal.
                        errorMessageWindow.Show();//megjelenítjük az ablakot.
                        return false;
                    }
                }
                else//Ha a válasz amit kaptunk üres.
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

        public async Task<bool> ReFreshButton()
        {
            postListLB.Items.Clear();//Előző itemek törlése az elavult illetve ismétlődő itemek elkerülése végett.
            editBT.IsEnabled = false;//Gombok letiltása.
            DeleteBT.IsEnabled = false;

            try
            {
                Response response = await Task.Run(() =>
                {
                    return NetworkHelper.Backend.GET(postEndpoint + "/get-market-ad").Send();
                });

                string responseData = response.Message.ToString();//Válasz feldolgozása.
                List<GetMarketPost> marketposts = JsonConvert.DeserializeObject<List<GetMarketPost>>(responseData);//Válasz jsnon konvertálása és listábak üldése.
                if (response != null)//Ha a válasz nem üres.
                {
                    foreach (GetMarketPost marketpostpost in marketposts)//Osztály típusú listával adatok változóba töltése.
                    {
                        int marketID = marketpostpost.marketID;
                        string username = marketpostpost.username;
                        string header = marketpostpost.title;
                        DateTime marketDate = marketpostpost.marketDATE;
                        double postPrice = marketpostpost.price;
                        string marketParagraph = marketpostpost.marketpost;
                    }


                    if (marketposts.Count > 0)//Ha nem üres a lista.
                    {
                        foreach (var posts in marketposts)
                        {
                            postListLB.Items.Add($"Azonosító: {posts.marketID}\nPosztoló neve: {posts.username} ({posts.userID})\n\nCím:\n {posts.title} " +
                                $"\n\nPoszt szövege: \n{posts.marketpost}\n\nPosztolás dátuma: {posts.marketDATE}\nTermék ára: {posts.price} Ft,-");
                        }
                        return true;
                    }
                    else//Ha üres a lista.
                    {
                        ErrorMessageWindow errorMessageWindow = new ErrorMessageWindow();//létrehozzuk számára az ablakot.
                        errorMessageWindow.LabelContent = "Hiba történt! \n\nNem található poszt a listában!";//A hibaüzenet ablakjában található Label Content megváltoztatása a kívánt felirattal.
                        errorMessageWindow.Show();//megjelenítjük az ablakot.
                        return false;
                    }
                }
                else//Ha üres a válasz.
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

        private void DeleteBT_Click(object sender, RoutedEventArgs e)//Törlés gomb eseménye.
        {
            try
            {
                if (postListLB.SelectedItems != null)//Hibakezelés.
                {

                    MarketDelete marketDelete = new MarketDelete(selectedMarketPostData, this);//létrehozzuk számára az ablakot.
                    marketDelete.Show();//megjelenítjük az ablakot.
                }
                else
                {
                    ErrorMessageWindow errorMessageWindow = new ErrorMessageWindow();//létrehozzuk számára az ablakot.
                    errorMessageWindow.LabelContent = $"Hiba történt!\nNincs kiválasztva egy poszt sem!";//A hibaüzenet ablakjában található Label Content megváltoztatása a kívánt felirattal.
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

        private void postListLB_SelectionChanged(object sender, SelectionChangedEventArgs e)//Ha a listbox itemjei közül ki van választva egy, akkor a gombok egdeélyezve lesznek.
        {
            editBT.IsEnabled = true;
            DeleteBT.IsEnabled = true;

            try
            {
                if (postListLB.SelectedItem != null)
                {
                    string selectedPostLine = postListLB.SelectedItem.ToString();
                    string[] marketPostProperties = selectedPostLine.ToString().Split('\n');
                    marketPostProperties = marketPostProperties.Select(s => s.Replace("\r", "").Trim()).Where(s => !string.IsNullOrEmpty(s)).ToArray();
                    int startIndex = 5;
                    int utolsoKettokihagy = marketPostProperties.Length - startIndex - 2;
                    string concatenatedString = string.Join(" ", marketPostProperties.Skip(startIndex).Take(utolsoKettokihagy));
                    string[] newArray = { marketPostProperties[0], marketPostProperties[1], marketPostProperties[3], concatenatedString };
                    newArray = newArray.Concat(marketPostProperties.Skip(marketPostProperties.Length - 2)).ToArray();




                    string[] marketpostPropertiesPostID = newArray[0].Split(' ');
                    string[] marketpostPropertiesUSERID = newArray[1].Split('(', ')', ' ');
                    string[] marketpostPropertiesPrice = newArray[5].Split(' ');


                    selectedMarketPostData = new MarketDatas
                    {
                        posztID = int.Parse(marketpostPropertiesPostID[1]),
                        userID = int.Parse(marketpostPropertiesUSERID[4]),
                        userName = marketpostPropertiesUSERID[2],
                        title = newArray[2],
                        post = newArray[3],
                        price = double.Parse(marketpostPropertiesPrice[2].ToString())
                    };
                }
                else
                {
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

        private void editBT_Click(object sender, RoutedEventArgs e)//Módosítás gomb eseménye.
        {
            try
            {
                if (postListLB.SelectedItem != null)//Hibakezelés.
                {
                    MarketPostEdit marketPostEdit = new MarketPostEdit(selectedMarketPostData, this);//létrehozzuk számára az ablakot.
                    marketPostEdit.Show();//megjelenítjük az ablakot.
                }
                else
                {
                    ErrorMessageWindow errorMessageWindow = new ErrorMessageWindow();//létrehozzuk számára az ablakot.
                    errorMessageWindow.LabelContent = $"Hiba történt!\nNincs kiválasztva egy poszt sem!";//A hibaüzenet ablakjában található Label Content megváltoztatása a kívánt felirattal.
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

        private async void refreshBT_Click(object sender, RoutedEventArgs e)//Lista frissítésére szolgáló gomb.
        {
            var progressWindow = new ProgressWindow();
            progressWindow.Show();

            try
            {
                bool successResponse = await ReFreshButton();
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
    }
}
