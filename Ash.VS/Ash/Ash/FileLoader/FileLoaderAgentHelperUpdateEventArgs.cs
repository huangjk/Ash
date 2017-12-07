namespace Ash.FileLoader
{

    public sealed class FileLoaderAgentHelperUpdateEventArgs : BaseEventArgs
    {
        public FileLoaderAgentHelperUpdateEventArgs(float progress)
        {
            Progress = progress;
        }

        public float Progress
        {
            get;
            private set;
        }
    }
}
