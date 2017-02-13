using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;

namespace GroceryCheckOutSystem.Console
{
    public static class ValidationConsole
    {
        //Check whether file required by the application is actually present or not
        public static bool PerformHealthCheck()
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            KeyValueConfigurationCollection settings = config.AppSettings.Settings;
            Dictionary<string, string> dictionary = settings.AllKeys.ToDictionary(key => key, key => settings[key].Value);
            bool result = true;
            foreach (KeyValuePair<string, string> dic in dictionary)
            {
                if (!dic.Key.ToLower().Contains("filename"))
                    continue;
                if (File.Exists(dic.Value))
                    continue;
                System.Console.WriteLine("Configutration for {0} is invalid. File {1} does not exists.Please set it correctly", dic.Key, dic.Value);
                result = false;
            }
            return result;
        }
    }
}