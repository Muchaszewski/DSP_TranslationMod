using System.IO;

namespace TranslationCommon
{
    public static class Utils
    {
        public const string PluginName = "DSPTranslationPlugin";
        public static string PluginPath = Path.Combine(BepInEx.Paths.PluginPath, PluginName); /*BepInEx.Paths.PluginPath*/
    }
}