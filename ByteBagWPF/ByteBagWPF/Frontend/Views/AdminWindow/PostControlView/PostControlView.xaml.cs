using ByteBagWPF.Backend.baseURL;
using ByteBagWPF.Backend.GetPost;
using ByteBagWPF.Backend.GetPostClass.SendPostDatas;
using ByteBagWPF.Frontend.Views.MessageWindow.Delete.PostDelete;
using ByteBagWPF.Frontend.Views.MessageWindow.Edit.PostEdit.ForumPost;
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

namespace ByteBagWPF.Frontend.Views.AdminWindow.PostControlView
{
    /// <summary>
    /// Interaction logic for PostControlView.xaml
    /// </summary>
    public partial class PostControlView : UserControl
    {
        private string postEndpoint = baseURL.Instance.GlobalURLString;//szerver oldali végpont dekrálása.
        public List<GetPosts> posts;
        PostDatas selectedPostData;

        public PostControlView()//Nézzet betöltésekor.
        {
            InitializeComponent();//Komponensek betöltése.
            Task.Run(async () =>
            {
                await LoadData();
            });
        }

        public async Task<bool> LoadData()
        {
            postListLB.Items.Clear();//esetlegesen fentmaradó listbox itemek törlése.

            try
            {
                Response response = await Task.Run(() =>
                {
                    return NetworkHelper.Backend.GET(postEndpoint + "/getpost").Send();
                });

                string responseData = response.Message.ToString();//Válasz konvertálása.
                List<GetPosts> posts = JsonConvert.DeserializeObject<List<GetPosts>>(responseData);//Json konvertálása, majd listába küldése.
                if (response != null)//Ha nem üres a válasz.
                {
                    foreach (GetPosts post in posts)//Osztály tulajdonságok segítségével a konvertált json fájl adatainak változóba töltése típus szerint.
                    {
                        int marketID = post.posztID;
                        int userID = post.userID;
                        string username = post.userName;
                        string header = post.title;
                        DateTime marketDate = post.postDate;
                        string pharagraph = post.post;

                    }


                    if (posts.Count > 0)//Ha nem üres a lista.
                    {
                        foreach (var post in posts)//Foreach-el végig a listán, listbox feltötése.
                        {
                            postListLB.Items.Add($"Azonosító: {post.posztID}\nPosztoló neve: {post.userName} ({post.userID})\n\nCím: \n{post.title}" +
                                $"\n\nPoszt szövege: \n{post.post}\n\nPosztolás dátuma: {post.postDate}");

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

        public async Task<bool> ReFreshButton()
        {
            postListLB.Items.Clear();//esetlegesen fentmaradó listbox itemek törlése.


            try
            {
                Response response = await Task.Run(() =>
                {
                    return NetworkHelper.Backend.GET(postEndpoint + "/getpost").Send();
                });

                string responseData = response.Message.ToString();//Válasz konvertálása.
                List<GetPosts> posts = JsonConvert.DeserializeObject<List<GetPosts>>(responseData);//Json konvertálása, majd listába küldése.

                if (response != null)//Ha nem üres a válasz.
                {
                    foreach (GetPosts post in posts)//Osztály tulajdonságok segítségével a konvertált json fájl adatainak változóba töltése típus szerint.
                    {
                        int marketID = post.posztID;
                        int userID = post.userID;
                        string username = post.userName;
                        string header = post.title;
                        DateTime marketDate = post.postDate;
                        string pharagraph = post.post;

                    }


                    if (posts.Count > 0)//Ha nem üres a lista.
                    {
                        foreach (var post in posts)//Foreach-el végig a listán, listbox feltötése.
                        {
                            postListLB.Items.Add($"Azonosító: {post.posztID}\nPosztoló neve: {post.userName} ({post.userID})\n\nCím: \n{post.title}" +
                                $"\n\nPoszt szövege: \n{post.post}\n\nPosztolás dátuma: {post.postDate}");

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

        private void DeleteBT_Click(object sender, RoutedEventArgs e)//Törlés gomb metódusa
        {
            try
            {
                if (postListLB.SelectedItems != null)//ha van kiválasztva item.
                {

                    PostDelete deletePost = new PostDelete(selectedPostData, this);//létrehozzuk számára az ablakot.
                    deletePost.Show();//megjelenítjük az ablakot.
                }
                else//Ha nincs kiválasztva item
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

        private void postListLB_SelectionChanged(object sender, SelectionChangedEventArgs e)//Kiválasztás esetén gombok elérhetőségének módosítása.
        {
            editBT.IsEnabled = true;
            DeleteBT.IsEnabled = true;
            try
            {
                if (postListLB.SelectedItem != null)
                {
                    string selectedPostLine = postListLB.SelectedItem.ToString();
                    string[] PostProperties = selectedPostLine.ToString().Split('\n');
                    PostProperties = PostProperties.Select(s => s.Replace("\r", "").Trim()).Where(s => !string.IsNullOrEmpty(s)).ToArray();
                    int startIndex = 5;
                    int utolsoKettokihagy = PostProperties.Length - startIndex - 1;
                    string concatenatedString = string.Join(" ", PostProperties.Skip(startIndex).Take(utolsoKettokihagy));
                    string[] newArray = { PostProperties[0], PostProperties[1], PostProperties[3], concatenatedString };
                    newArray = newArray.Concat(PostProperties.Skip(PostProperties.Length - 1)).ToArray();

                    string[] marketpostPropertiesPostID = newArray[0].Split(' ');
                    string[] marketpostPropertiesUSERID = newArray[1].Split('(', ')', ' ');


                    selectedPostData = new PostDatas
                    {
                        posztID = int.Parse(marketpostPropertiesPostID[1]),
                        userID = int.Parse(marketpostPropertiesUSERID[4]),
                        userName = marketpostPropertiesUSERID[2],
                        title = newArray[2],
                        post = newArray[3],
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

        private void editBT_Click(object sender, RoutedEventArgs e)//Módosítás gomb eljárása.
        {
            try
            {
                if (postListLB.SelectedItem != null)//Ha van kiválasztva item.
                {
                    ForumPostEdit postEditWindow = new ForumPostEdit(selectedPostData, this);//létrehozzuk számára az ablakot.
                    postEditWindow.Show();//megjelenítjük az ablakot.
                }
                else//Ha nincs kiválasztva item.
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

        private async void refreshBT_Click(object sender, RoutedEventArgs e)
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
