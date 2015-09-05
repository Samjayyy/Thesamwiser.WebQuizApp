using System.Collections.Generic;
using System.Configuration;
using Newtonsoft.Json;

namespace QuizWebApp
{
    public static class JsonAppSettings
    {
        public static Dictionary<string, string> AsDictionary(string key)
        {
            var appSetting = ConfigurationManager.AppSettings[key];
            if (appSetting == null) return null;
            return JsonConvert.DeserializeObject<Dictionary<string, string>>(appSetting);
        }
    }
}