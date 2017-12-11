using System.Collections.Generic;

namespace ProjectCatalogDirectory
{

    /// <summary>
    /// 项目文件夹接口
    /// </summary>
    public interface IProjectDirectory
    {
        /// <summary>
        /// 得到当前工程目录
        /// </summary>
        /// <returns>所有文件夹路径列表</returns>
        List<string> GetCurrentProjectCatalog();


        /// <summary>
        /// 读Json获得目录文件夹路径List
        /// </summary>
        /// <param name="txtFullPath">Json文件路径</param>
        /// <returns></returns>
        List<string> LoadCatalogDirectoryJsonData(string txtFullPath);

        /// <summary>
        /// 将json数据解析成文件夹路径信息
        /// </summary>
        /// <returns></returns>
        List<string> ParseJsonDatToCatalogData(string jsonData);

        /// <summary>
        /// 存Unity工程文件夹目录为JSon文件
        /// </summary>
        /// <param name="txtTargetPath"></param>
        /// <param name="allDirectoryPaths"></param>
        void SaveCatalogDirectoryJsonData(string txtTargetPath, List<string> allDirectoryPaths);

        /// <summary>
        /// 得到不包含指定路径的所有路径
        /// </summary>
        /// <param name="excludePath">除去的路径</param>
        /// <returns></returns>
        List<string> GetAllDirectoryListExclude(List<string> allDirectorys, string excludeStr);

        /// <summary>
        /// 得到包含的指定路径的所有路径
        /// </summary>
        /// <param name="excludePath">除去的路径</param>
        /// <returns></returns>
        List<string> GetAllDirectoryListInclude(List<string> allDirectorys, string includeStr);
    }
}
