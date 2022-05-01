using System.Globalization;
using System.Resources;
using System.Threading;

namespace UniHacker
{
    public class Language
    {
        public const string English = "English";
        public const string Chinese = "Chinese";
        public const string DefaultLanguage = English;

        static ResourceManager s_ResourceManager;

        public static void Init()
        {
            var language = Thread.CurrentThread.CurrentCulture.Name;
            var languageFileName = string.Empty;
            if (language == "zh-CN")
                languageFileName = Language.Chinese;
            else
                languageFileName = Language.English;

            CultureInfo culture = null;
            switch (languageFileName)
            {
                case Chinese:
                    culture = new CultureInfo("zh-CN");
                    break;
                case English:
                    culture = new CultureInfo("en-US");
                    break;
            }

            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;

            s_ResourceManager = new ResourceManager($"{nameof(UniHacker)}.Properties.Language_{languageFileName}", typeof(Language).Assembly);
        }

        public static string GetString(string key, params string[] args)
        {
            return string.Format(s_ResourceManager.GetString(key, Thread.CurrentThread.CurrentCulture) ?? string.Empty, args);
        }
    }
}
