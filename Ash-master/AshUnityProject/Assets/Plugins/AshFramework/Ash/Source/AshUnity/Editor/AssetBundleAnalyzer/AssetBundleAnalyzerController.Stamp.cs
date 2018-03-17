






namespace Ash.Editor.AssetBundleTools
{
    internal sealed partial class AssetBundleAnalyzerController
    {
        private struct Stamp
        {
            private readonly string m_AssetName;
            private readonly string m_HostAssetName;

            public Stamp(string assetName, string hostAssetName)
            {
                m_AssetName = assetName;
                m_HostAssetName = hostAssetName;
            }

            public string AssetName
            {
                get
                {
                    return m_AssetName;
                }
            }

            public string HostAssetName
            {
                get
                {
                    return m_HostAssetName;
                }
            }
        }
    }
}
