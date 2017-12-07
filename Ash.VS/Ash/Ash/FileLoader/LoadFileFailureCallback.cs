namespace Ash.FileLoader
{
    public delegate void LoadFileFailureCallback(string fileUrl, LoadFileStatus loadFileStatus, string errorMessage, object userData);
}