using ByteBagWPF.Backend.baseURL.ConfigWriter;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace ByteBagUnitTests
{



    [TestClass]
    public class ConfigManagerUnitest
    {
        [TestMethod]
        public void ConfigManager_GetConfigValue_If_FileExists()
        {
            var result = ConfigManager.GetConfigValue("defaultURL");
            Assert.AreEqual("http://www.bytebag.hu:9703", result, "Error");
        }

        [TestMethod]
        public void ConfigManager_SetConfigValue_If_FileExists()
        {
            string key = "baseURL";
            string value = "almafa.hu";
            ConfigManager.SetConfigValue(key, value);
            string h = ConfigManager.GetConfigValue(key);
            Assert.AreEqual("almafa.hu", h, "Error");
        }
    }
}
