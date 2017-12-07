
namespace Ash.FileLoader
{

    public enum LoadFileStatus
    {
        /// <summary>
        /// 加载文件完成。
        /// </summary>
        Ok = 0,

        /// <summary>
        /// 文件尚未准备完毕。
        /// </summary>
        NotReady,

        /// <summary>
        /// 文件不存在于磁盘上。
        /// </summary>
        NotExist,

        /// <summary>
        /// 文件类型错误。
        /// </summary>
        TypeError,
    }
    
}
