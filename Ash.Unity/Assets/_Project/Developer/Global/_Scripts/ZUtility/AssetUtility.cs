namespace Framework
{
    public static class AssetUtility
    {
        public static string GetDataTableAsset(string assetName)
        {
            return string.Format("Assets/_Project/Developer/Global/DataTables/{0}.txt", assetName);
        }

        public static string GetDictionaryAsset(string assetName)
        {
            return string.Format("Assets/_Project/Developer/Global/Localization/{0}/Dictionaries/{1}.xml", Entry.Localization.Language.ToString(), assetName);
        }

        public static string GetFontAsset(string assetName)
        {
            return string.Format("Assets/_Project/Developer/Global/Localization/{0}/Fonts/{1}.ttf", Entry.Localization.Language.ToString(), assetName);
        }

        public static string GetSceneAsset(string assetName)
        {
            return string.Format("Assets/_Project/Developer/Global/_Scenes/{0}.unity", assetName);
        }

        public static string GetMusicAsset(string assetName)
        {
            return string.Format("Assets/_Project/Developer/Global/Audio/Music/{0}.mp3", assetName);
        }

        public static string GetSoundAsset(string assetName)
        {
            return string.Format("Assets/_Project/Developer/Global/Audio/Sounds/{0}.wav", assetName);
        }

        public static string GetEntityAsset(string assetName)
        {
            return string.Format("Assets/_Project/Developer/Global/Entities/{0}.prefab", assetName);
        }

        public static string GetUIFormAsset(string assetName)
        {
            return string.Format("Assets/_Project/Developer/Global/UI/UIForms/{0}.prefab", assetName);
        }

        public static string GetUISpriteAsset(string assetName)
        {
            return string.Format("Assets/_Project/Developer/Global/UI/UISprites/{0}.png", assetName);
        }

        public static string GetUISoundAsset(string assetName)
        {
            return string.Format("Assets/_Project/Developer/Global/Audio/UISounds/{0}.wav", assetName);
        }
    }
}
