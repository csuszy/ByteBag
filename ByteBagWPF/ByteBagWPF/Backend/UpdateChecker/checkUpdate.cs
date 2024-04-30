using ByteBagWPF.Frontend.Views.MessageWindow.Error;
using ByteBagWPF.Frontend.Views.MessageWindow.Update;
using NetworkHelper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;

namespace ByteBagWPF.Backend.UpdateChecker
{
    public class checkUpdate
    {
        private string url = ByteBagWPF.Backend.baseURL.baseURL.Instance.GlobalURLString;
        private string currentVersion;
        private static readonly HttpClient httpClient = new HttpClient();

        public async Task CheckForUpdatesAsync()
        {
            try
            {
                // API endpoint a frissítési információhoz
                string updateInfoUrl = url + "/update/latest";
                // Frissítési információk lekérése az API-tól
                Response response = NetworkHelper.Backend.GET(updateInfoUrl).Send();
                string responseData = response.Message.ToString();
                if (response != null)
                {
                    // Az aktuális verziószám
                    Version version = Assembly.GetEntryAssembly().GetName().Version;
                    currentVersion = version.ToString();
                    var updateInfo = JsonConvert.DeserializeObject<List<UpdateInfo>>(responseData);
                    if (updateInfo != null)
                    {
                        foreach (UpdateInfo info in updateInfo)
                        {
                            string serverVersion = info.version.ToString();
                            string downloadUrl = info.downloadUrl.ToString();
                            string releaseNotes = info.releaseNotes.ToString();
                            if (serverVersion != currentVersion)
                            {

                                UpdateAvaiable updateAvaiableWindow = new UpdateAvaiable();
                                updateAvaiableWindow.errorTextBlock.Text = $"Új frissítés elérhető: {serverVersion}\nLeírás: {releaseNotes}\n\nFrissítés 5 másodpercen belül...";
                                updateAvaiableWindow.Show();
                                await Task.Delay(8000);
                                await DownloadAndUpdate(info.downloadUrl);

                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                ErrorMessageWindow errorMessageWindow = new ErrorMessageWindow();
                errorMessageWindow.errorTextBlock.Text = $"Hiba történt!\n\n{ex.Message}";
                errorMessageWindow.Show();
            }
        }

        public async Task DownloadAndUpdate(string downloadUrl)
        {
            try
            {
                using (var httpClient = new WebClient())
                {
                    // Ideiglenes mappa
                    string tempFolderPath = Path.GetTempPath();

                    // Letöltött fájl teljes elérési útja
                    string downloadedFilePath = Path.Combine(tempFolderPath, Path.GetFileName(downloadUrl));

                    // Letöltés a megadott URL-ről
                    await httpClient.DownloadFileTaskAsync(new Uri(url + "/update/" + downloadUrl), downloadedFilePath);

                    if (System.IO.File.Exists(downloadedFilePath))
                    {
                        // Fájl megnyitása
                        System.Diagnostics.Process.Start(downloadedFilePath);
                        Application.Current.Shutdown();
                    }
                    else
                    {
                        await Task.Delay(70000);
                        return;
                    }

                }
            }
            catch (Exception ex)
            {
                // Hibakezelés (pl. hibaüzenet megjelenítése)
                MessageBox.Show($"Hiba történt a letöltés közben: {ex.Message}", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}

