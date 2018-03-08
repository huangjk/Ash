






namespace Ash.Core.Download
{
    internal partial class DownloadManager
    {
        private partial class DownloadCounter
        {
            private sealed class DownloadCounterNode
            {
                private readonly int m_DownloadedLength;
                private float m_ElapseSeconds;

                public DownloadCounterNode(int downloadedLength)
                {
                    m_DownloadedLength = downloadedLength;
                    m_ElapseSeconds = 0f;
                }

                public int DownloadedLength
                {
                    get
                    {
                        return m_DownloadedLength;
                    }
                }

                public float ElapseSeconds
                {
                    get
                    {
                        return m_ElapseSeconds;
                    }
                }

                public void Update(float elapseSeconds, float realElapseSeconds)
                {
                    m_ElapseSeconds += realElapseSeconds;
                }
            }
        }
    }
}
