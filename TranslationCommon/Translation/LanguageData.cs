using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace TranslationCommon
{
    /// <summary>
    ///     Container for language translation data
    /// </summary>
    [Serializable]
    public class LanguageData
    {
        /// <summary>
        ///     File name of legacy format
        /// </summary>
        [NonSerialized]
        public const string TranslationFileName = "translation.json";
        /// <summary>
        ///     File name of plain text format
        /// </summary>
        [NonSerialized]
        public const string TranslationDumpFileName = "translation.dump.txt";
        /// <summary>
        ///     File name of crowdin format
        /// </summary>
        [NonSerialized] 
        public const string TranslationCrowdinFileName = "translation_DysonSphereProgram.json";
        
        /// <summary>
        ///     Translation table - used for loading
        /// </summary>
        [SerializeField]
        public List<TranslationProto> TranslationTable;
        
        /// <summary>
        ///     Default empty constructor
        /// </summary>
        public LanguageData()
        {
        }
        
        /// <summary>
        ///     Copy constructor to generate new Language data from in Game Data
        /// </summary>
        /// <param name="settings">Settings</param>
        /// <param name="stringProto">In game translations</param>
        public LanguageData(LanguageSettings settings, ProtoSet<StringProto> stringProto)
        {
            TranslationTable = new List<TranslationProto>(stringProto.Length);
            var translationDelegate = GetOriginalTextDelegate<StringProto>(settings);
            for (var i = 0; i < stringProto.dataArray.Length; i++)
            {
                var proto = stringProto.dataArray[i];
                TranslationProto translationProto = new TranslationProto();
                translationProto.IsValid = true;
                translationProto.Original = translationDelegate(proto);
                translationProto.Translation = translationDelegate(proto);
                translationProto.Name = proto.Name;
                TranslationTable.Add(translationProto);
            }
        }

        /// <summary>
        ///     Delegate constructor for getting field value from original translation
        /// </summary>
        /// <param name="settings">Language settings</param>
        /// <typeparam name="T">Type of data</typeparam>
        /// <returns>Resulting delegate to get string from</returns>
        /// <exception cref="ArgumentException">Throws exception if given field info was not found in assmebly</exception>
        public static Func<T, string> GetOriginalTextDelegate<T>(LanguageSettings settings)
        {
            var fieldInfo = typeof(T).GetField(settings.OriginalLanguage, BindingFlags.Public | BindingFlags.Instance);
            if (fieldInfo == null)
            {
                throw new ArgumentException($"LanguageSettings has incorrect original translation value -- used {settings.OriginalLanguage} but it do not exists");
            }

            var d = new Func<T, string>(arg => (string)fieldInfo.GetValue(arg));
            return param => d(param);
        }
    }
}