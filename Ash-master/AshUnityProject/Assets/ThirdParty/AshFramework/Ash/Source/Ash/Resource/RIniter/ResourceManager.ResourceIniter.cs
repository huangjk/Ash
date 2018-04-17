using System;
using System.Collections.Generic;
using System.IO;

namespace Ash.Core.Resource
{
    internal partial class ResourceManager
    {
        /// <summary>
        /// 资源初始化器。
        /// </summary>
        private sealed class ResourceIniter
        {
            private readonly ResourceManager m_ResourceManager;
            private string m_CurrentVariant;

            public AshAction ResourceInitComplete;

            /// <summary>
            /// 初始化资源初始化器的新实例。
            /// </summary>
            /// <param name="resourceManager">资源管理器。</param>
            public ResourceIniter(ResourceManager resourceManager)
            {
                m_ResourceManager = resourceManager;
                m_CurrentVariant = null;

                ResourceInitComplete = null;
            }

            /// <summary>
            /// 关闭并清理资源初始化器。
            /// </summary>
            public void Shutdown()
            {

            }

            /// <summary>
            /// 初始化资源。
            /// </summary>
            public void InitResources(string currentVariant)
            {
                m_CurrentVariant = currentVariant;

                if (m_ResourceManager.m_ResourceHelper == null)
                {
                    throw new AshException("Resource helper is invalid.");
                }

                m_ResourceManager.m_ResourceHelper.LoadBytes(Utility.Path.GetRemotePath(m_ResourceManager.m_ReadOnlyPath, Utility.Path.GetResourceNameWithSuffix(VersionListFileName)/*.dat*/), ParsePackageList);
            }

            /// <summary>
            /// 解析资源包资源列表。
            /// </summary>
            /// <param name="fileUri">版本资源列表文件路径。</param>
            /// <param name="bytes">要解析的数据。</param>
            /// <param name="errorMessage">错误信息。</param>
            private void ParsePackageList(string fileUri, byte[] bytes, string errorMessage)
            {
                if (bytes == null || bytes.Length <= 0)
                {
                    throw new AshException(string.Format("Package list '{0}' is invalid, error message is '{1}'.", fileUri, string.IsNullOrEmpty(errorMessage) ? "<Empty>" : errorMessage));
                }

                MemoryStream memoryStream = null;
                try
                {
                    memoryStream = new MemoryStream(bytes);
                    using (BinaryReader binaryReader = new BinaryReader(memoryStream))
                    {
                        memoryStream = null;
                        char[] header = binaryReader.ReadChars(3);
                        if (header[0] != PackageListHeader[0] || header[1] != PackageListHeader[1] || header[2] != PackageListHeader[2])
                        {
                            throw new AshException("Package list header is invalid.");
                        }

                        byte listVersion = binaryReader.ReadByte();

                        if (listVersion == 0)
                        {
                            //用于解密的Bytes
                            byte[] encryptBytes = binaryReader.ReadBytes(4);

                            //获取当前资源适用的游戏版本号。
                            m_ResourceManager.m_ApplicableGameVersion = Utility.Converter.GetString(Utility.Encryption.GetXorBytes(binaryReader.ReadBytes(binaryReader.ReadByte()), encryptBytes));
                            //获取当前资源内部版本号。
                            m_ResourceManager.m_InternalResourceVersion = binaryReader.ReadInt32();

                            int resourceCount = binaryReader.ReadInt32();
                            string[] names = new string[resourceCount];
                            string[] variants = new string[resourceCount];
                            int[] lengths = new int[resourceCount];
                            Dictionary<string, string[]> dependencyAssetNamesCollection = new Dictionary<string, string[]>();

                            for (int i = 0; i < resourceCount; i++)
                            {
                                //名字
                                names[i] = Utility.Converter.GetString(Utility.Encryption.GetXorBytes(binaryReader.ReadBytes(binaryReader.ReadByte()), encryptBytes));

                                //变体
                                variants[i] = null;
                                byte variantLength = binaryReader.ReadByte();
                                if (variantLength > 0)
                                {
                                    variants[i] = Utility.Converter.GetString(Utility.Encryption.GetXorBytes(binaryReader.ReadBytes(variantLength), encryptBytes));
                                }

                                //资源加载方式类型
                                LoadType loadType = (LoadType)binaryReader.ReadByte();

                                //长度？？
                                lengths[i] = binaryReader.ReadInt32();

                                //hashCode
                                int hashCode = binaryReader.ReadInt32();

                                //resource（资源）里面的asset(资产)数量
                                int assetNamesCount = binaryReader.ReadInt32();
                                string[] assetNames = new string[assetNamesCount];
                                //遍历asset
                                for (int j = 0; j < assetNamesCount; j++)
                                {
                                    //资产名字
                                    assetNames[j] = Utility.Converter.GetString(Utility.Encryption.GetXorBytes(binaryReader.ReadBytes(binaryReader.ReadByte()), Utility.Converter.GetBytes(hashCode)));

                                    //关联资产的数量
                                    int dependencyAssetNamesCount = binaryReader.ReadInt32();
                                    string[] dependencyAssetNames = new string[dependencyAssetNamesCount];

                                    //遍历关联的资产
                                    for (int k = 0; k < dependencyAssetNamesCount; k++)
                                    {
                                        //关联的资产的名字
                                        dependencyAssetNames[k] = Utility.Converter.GetString(Utility.Encryption.GetXorBytes(binaryReader.ReadBytes(binaryReader.ReadByte()), Utility.Converter.GetBytes(hashCode)));
                                    }

                                    //缓存当前变体的关联资产
                                    if (variants[i] == null || variants[i] == m_CurrentVariant)
                                    {
                                        dependencyAssetNamesCollection.Add(assetNames[j], dependencyAssetNames);
                                    }
                                }

                                //缓存当前变体的资产信息和资源信息
                                if (variants[i] == null || variants[i] == m_CurrentVariant)
                                {
                                    ResourceName resourceName = new ResourceName(names[i], variants[i]);
                                    ProcessAssetInfo(resourceName, assetNames);
                                    ProcessResourceInfo(resourceName, loadType, lengths[i], hashCode);
                                }
                            }

                            //处理完所有资源信息和资产信息后，移除互相引用的关联资产，统计离散的关联资产。
                            ProcessAssetDependencyInfo(dependencyAssetNamesCollection);

                            //建个ResourceGroup，加入所有资源
                            ResourceGroup resourceGroupAll = m_ResourceManager.GetResourceGroup(string.Empty);
                            for (int i = 0; i < resourceCount; i++)
                            {
                                resourceGroupAll.AddResource(names[i], variants[i], lengths[i]);
                            }

                            //资源组的数量??
                            int resourceGroupCount = binaryReader.ReadInt32();
                            for (int i = 0; i < resourceGroupCount; i++)
                            {
                                //资源组名字
                                string groupName = Utility.Converter.GetString(Utility.Encryption.GetXorBytes(binaryReader.ReadBytes(binaryReader.ReadByte()), encryptBytes));

                                //找到资源组，没有就新建
                                ResourceGroup resourceGroup = m_ResourceManager.GetResourceGroup(groupName);

                                //资源组里面资源的数量
                                int groupResourceCount = binaryReader.ReadInt32();
                                for (int j = 0; j < groupResourceCount; j++)
                                {
                                    //版本Index？
                                    ushort versionIndex = binaryReader.ReadUInt16();
                                    if (versionIndex >= resourceCount)
                                    {
                                        throw new AshException(string.Format("Package index '{0}' is invalid, resource count is '{1}'.", versionIndex, resourceCount));
                                    }

                                    resourceGroup.AddResource(names[versionIndex], variants[versionIndex], lengths[versionIndex]);
                                }
                            }
                        }
                        else
                        {
                            throw new AshException("Package list version is invalid.");
                        }
                    }

                    ResourceInitComplete();
                }
                catch (Exception exception)
                {
                    if (exception is AshException)
                    {
                        throw;
                    }

                    throw new AshException(string.Format("Parse package list exception '{0}'.", exception.Message), exception);
                }
                finally
                {
                    if (memoryStream != null)
                    {
                        memoryStream.Dispose();
                        memoryStream = null;
                    }
                }
            }

            /// <summary>
            /// 处理资产信息(缓存到字典m_ResourceManager.m_AssetInfos)
            /// </summary>
            /// <param name="resourceName"></param>
            /// <param name="assetNames"></param>
            private void ProcessAssetInfo(ResourceName resourceName, string[] assetNames)
            {
                foreach (string assetName in assetNames)
                {
                    m_ResourceManager.m_AssetInfos.Add(assetName, new AssetInfo(assetName, resourceName));
                }
            }

            /// <summary>
            /// 移除互相引用的关联资产，统计离散的关联资产。
            /// </summary>
            /// <param name="dependencyAssetNamesCollection"></param>
            private void ProcessAssetDependencyInfo(Dictionary<string, string[]> dependencyAssetNamesCollection)
            {
                foreach (KeyValuePair<string, string[]> dependencyAssetNamesCollectionItem in dependencyAssetNamesCollection)
                {
                    List<string> dependencyAssetNames = new List<string>();
                    List<string> scatteredDependencyAssetNames = new List<string>();
                    foreach (string dependencyAssetName in dependencyAssetNamesCollectionItem.Value)
                    {
                        AssetInfo? assetInfo = m_ResourceManager.GetAssetInfo(dependencyAssetName);
                        if (assetInfo.HasValue)
                        {
                            dependencyAssetNames.Add(dependencyAssetName);
                        }
                        else
                        {
                            scatteredDependencyAssetNames.Add(dependencyAssetName);
                        }
                    }

                    m_ResourceManager.m_AssetDependencyInfos.Add(dependencyAssetNamesCollectionItem.Key, new AssetDependencyInfo(dependencyAssetNamesCollectionItem.Key, dependencyAssetNames.ToArray(), scatteredDependencyAssetNames.ToArray()));
                }
            }

            /// <summary>
            /// 处理资源信息(缓存到字典m_ResourceManager.m_ResourceInfos)
            /// </summary>
            /// <param name="resourceName"></param>
            /// <param name="loadType"></param>
            /// <param name="length">位置</param>
            /// <param name="hashCode"></param>
            private void ProcessResourceInfo(ResourceName resourceName, LoadType loadType, int length, int hashCode)
            {
                if (m_ResourceManager.m_ResourceInfos.ContainsKey(resourceName))
                {
                    throw new AshException(string.Format("Resource info '{0}' is already exist.", resourceName.FullName));
                }

                m_ResourceManager.m_ResourceInfos.Add(resourceName, new ResourceInfo(resourceName, loadType, length, hashCode, true));
            }
        }
    }
}
