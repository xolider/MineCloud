namespace MineCloudApp.Lang
{
    public class LanguageController
    {
        public static ILanguage CurrentLanguage { get; private set; }

        public static void Init()
        {
            CurrentLanguage = new FrenchLang();
        }
    }
}
