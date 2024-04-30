using ByteBagWPF.Frontend.Views.MessageWindow.Error;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;


namespace ByteBagWPF.Backend.baseURL.ConfigWriter
{
    public static class ConfigManager
    {
        private static readonly string appDataRoamingPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        private static readonly string byteBagDirectoryPath = Path.Combine(appDataRoamingPath, "ByteBag");
        private static readonly string filePath = Path.Combine(byteBagDirectoryPath, "ByteBagConfig.json");


        public static void SetConfigValue(string key, string value)
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    CreateDefaultConfig();
                }
                string json = File.ReadAllText(filePath);
                var config = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                config[key] = value;
                string updatedJson = JsonConvert.SerializeObject(config);
                File.WriteAllText(filePath, updatedJson);
            }
            catch (Exception)
            {
                ErrorMessageWindow errorMessageWindow = new ErrorMessageWindow();//létrehozzuk számára az ablakot.
                errorMessageWindow.errorTextBlock.Text = $"Hiba történt!\n\nEllenőrizd a végpont formátumát illetve helyességét!\n\nhttps://pelda.hu";
                errorMessageWindow.Show();//megjelenítjük az ablakot.
            }
        }

        public static void CreateDefaultConfig()
        {
            try
            {
                Directory.CreateDirectory(byteBagDirectoryPath);
                var defaultConfig = new Dictionary<string, string>
            {
               {"defaultURL", "https://bytebag.hu"},
               {"baseURL", "https://bytebag.hu"}
            };
                string defaultJson = JsonConvert.SerializeObject(defaultConfig);
                File.WriteAllText(filePath, defaultJson);
            }
            catch (Exception)
            {
                ErrorMessageWindow errorMessageWindow = new ErrorMessageWindow();//létrehozzuk számára az ablakot.
                errorMessageWindow.errorTextBlock.Text = $"Hiba történt az config fájl létrehozása közben!";
                errorMessageWindow.Show();
            }

        }

        public static string GetConfigValue(string key)
        {

            if (!File.Exists(filePath))
            {
                CreateDefaultConfig();
            }

            string json = File.ReadAllText(filePath);
            var config = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            try
            {
                if (config.ContainsKey(key))
                {
                    return config[key];
                }
                else
                {
                    ErrorMessageWindow errorMessageWindow = new ErrorMessageWindow();//létrehozzuk számára az ablakot.
                    errorMessageWindow.errorTextBlock.Text = $"Hiba történt!\n\nA kulcs nem található!\n-->ByteBagConfig.json";
                    errorMessageWindow.Show();
                    throw new KeyNotFoundException("A megadott kulcs nem található a konfigurációban!");
                }
            }
            catch
            {
                ErrorMessageWindow errorMessageWindow = new ErrorMessageWindow();//létrehozzuk számára az ablakot.
                errorMessageWindow.errorTextBlock.Text = $"Hiba történt!\n\nA kulcs nem található!\n-->ByteBagConfig.json";
                errorMessageWindow.Show();
                throw new KeyNotFoundException("A megadott kulcs nem található a konfigurációban!");
            }

        }
    }
}
