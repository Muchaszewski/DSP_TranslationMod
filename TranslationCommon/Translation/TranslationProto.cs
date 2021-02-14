using System;

namespace TranslationCommon.Translation
{
    [Serializable]
    public class TranslationProto
    {
        public bool IsValid;
            
        public string Name;
        
        public string Original;
        public string Translation;
        
        public string GetTranslation()
        {
            return Translation;
        }

        public TranslationProto(TranslationProto proto, string translation)
        {
            Name = proto.Name;
            Original = proto.Original;
            Translation = translation;
        }

        public static TranslationProto FromCrowdin(string name, string translation)
        {
            TranslationProto result = new TranslationProto();
            result.Name = name;
            result.Translation = translation;
            return result;
        }
        
        public TranslationProto(string name, int id, string original, string translation)
        {
            Name = name;
            Original = original;
            Translation = translation;
        }

        public TranslationProto()
        {
            
        }
    }
}