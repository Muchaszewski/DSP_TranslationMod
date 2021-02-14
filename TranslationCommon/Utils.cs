using System.IO;
using System.Reflection;

namespace TranslationCommon
{
    public static class Utils
    {
        private static readonly string AssemblyLocation = Assembly.GetCallingAssembly().Location;

        public static string PluginFolderName = Path.GetDirectoryName(AssemblyLocation);
        /// <summary>
        ///  Plugin path
        /// </summary>
        public static string PluginPath = Path.GetDirectoryName(AssemblyLocation);
    }
}