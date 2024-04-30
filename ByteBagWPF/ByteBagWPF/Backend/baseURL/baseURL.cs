using ByteBagWPF.Backend.baseURL.ConfigWriter;

namespace ByteBagWPF.Backend.baseURL
{
    public class baseURL
    {
        private static baseURL instance;

        // Globális változók deklarálása
        public string GlobalURLString { get; set; }

        // Privát konstruktor
        private baseURL()
        {
            // Globális változók inicializálása
            GlobalURLString = ConfigManager.GetConfigValue("baseURL");
        }

        // Singleton példány létrehozása
        public static baseURL Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new baseURL();
                }
                return instance;
            }
        }
    }
}
