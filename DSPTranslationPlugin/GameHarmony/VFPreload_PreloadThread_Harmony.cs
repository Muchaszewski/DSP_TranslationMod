using System.IO;
using System.Reflection;
using HarmonyLib;
using TranslationCommon;
using TranslationCommon.SimpleJSON;
using TranslationCommon.Translation;

namespace DSPTranslationPlugin.GameHarmony
{
    [HarmonyPatch(typeof(VFPreload), "PreloadThread")]
    public class VFPreload_PreloadThread
    {
        [HarmonyPostfix]
        public static void Postfix(VFPreload __instance)
        {
            var stringProtoSet = LDB.strings;
            foreach (var languageContainer in TranslationManager.Langauges)
            {
                var templateLanguageData = new LanguageData(languageContainer.Settings, stringProtoSet);
                languageContainer.LoadTranslation(templateLanguageData);
            }
        }

        private static ProtoSet<T> GetProtoSet<T>() 
            where T : Proto
        {
            var properties = typeof(LDB).GetProperties(BindingFlags.Static | BindingFlags.Public);
            foreach (var property in properties)
            {
                var value = property.GetValue(null, null);
                var type = value.GetType();
                var dataArray = (Proto[])type.GetField("dataArray").GetValue(value);
                var arrayType = dataArray.GetType().GetElementType();

                if (arrayType != typeof(T))
                {
                    continue;
                }

                return (ProtoSet<T>)value;
            }

            return null;
        }
        
        private static void ProtoJsonDump() 
        {
            var properties = typeof(LDB).GetProperties(BindingFlags.Static | BindingFlags.Public);
            foreach (var property in properties)
            {
                var value = property.GetValue(null, null);
                var type = value.GetType();
                var result = JSON.ToJson(value, true);
                var instanceGenericName = type.FullName;
                TextWriter writer = new StreamWriter($"{Utils.PluginPath}/JsonDump/{instanceGenericName}.json");
                writer.Write(result);
                writer.Flush();
                writer.Close();
                ConsoleLogger.LogInfo("JsonProtoDumpSet: " + value + " of " + instanceGenericName);
            }
        }
    }
}