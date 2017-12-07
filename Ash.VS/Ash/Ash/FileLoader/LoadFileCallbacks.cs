namespace Ash.FileLoader
{
    public sealed class LoadFileCallbacks
    {
        private readonly LoadFileSuccessCallback m_LoadFileSuccessCallback;
        private readonly LoadFileFailureCallback m_LoadFileFailureCallback;
        private readonly LoadFileUpdateCallback m_LoadFileUpdateCallback;

        
        public LoadFileCallbacks(LoadFileSuccessCallback loadFileSuccessCallback,  LoadFileUpdateCallback loadFileUpdateCallback) : this(loadFileSuccessCallback,null, loadFileUpdateCallback)
        {
        }

        public LoadFileCallbacks(LoadFileSuccessCallback loadFileSuccessCallback) : this(loadFileSuccessCallback, null, null)
        {
        }
       
        public LoadFileCallbacks(LoadFileSuccessCallback loadFileSuccessCallback, LoadFileFailureCallback loadFileFailureCallback) : this(loadFileSuccessCallback, loadFileFailureCallback, null)
        {
        }

        public LoadFileCallbacks(LoadFileSuccessCallback loadFileSuccessCallback, LoadFileFailureCallback loadFileFailureCallback, LoadFileUpdateCallback loadFileUpdateCallback)
        {
            if (loadFileSuccessCallback == null)
            {
                throw new AshException("Load File success callback is invalid.");
            }

            m_LoadFileSuccessCallback = loadFileSuccessCallback;
            m_LoadFileFailureCallback = loadFileFailureCallback;
            m_LoadFileUpdateCallback = loadFileUpdateCallback;
        }

        public LoadFileSuccessCallback LoadFileSuccessCallback
        {
            get
            {
                return m_LoadFileSuccessCallback;
            }
        }

        public LoadFileFailureCallback LoadFileFailureCallback
        {
            get
            {
                return m_LoadFileFailureCallback;
            }
        }

        public LoadFileUpdateCallback LoadFileUpdateCallback
        {
            get
            {
                return m_LoadFileUpdateCallback;
            }
        }
    }
}