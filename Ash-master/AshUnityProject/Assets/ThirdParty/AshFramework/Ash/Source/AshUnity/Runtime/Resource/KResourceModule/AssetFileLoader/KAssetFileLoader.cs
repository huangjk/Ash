using System;
using System.Collections;
using System.IO;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Ash.Runtime
{
    /// <summary>
    /// 根據不同模式，從AssetBundle中獲取Asset或從Resources中獲取,两种加载方式同时实现的桥接类
    /// 读取一个文件的对象，不做拷贝和引用
    /// </summary>
    public class AssetFileLoader : AbstractResourceLoader
    {
        public delegate void AssetFileBridgeDelegate(bool isOk, Object resultObj);

        public Object Asset
        {
            get { return ResultObject as Object; }
        }

        public static bool IsEditorLoadAsset
        {
            get
            {
                return /*AppEngine.GetConfig("KEngine", "IsEditorLoadAsset").ToInt32() != 0 &&*/ Application.isEditor;
            }
        }

        public override float Progress
        {
            get
            {
                return 0;
            }
        }

        public static AssetFileLoader Load(string path, AssetFileBridgeDelegate assetFileLoadedCallback = null)
        {
            LoaderDelgate realcallback = null;
            if (assetFileLoadedCallback != null)
            {
                realcallback = (isOk, obj) => assetFileLoadedCallback(isOk, obj as Object);
            }

            return AutoNew<AssetFileLoader>(path, realcallback, false);
        }

        protected override void Init(string url, params object[] args)
        {
            var loaderMode = (LoaderMode)args[0];

            base.Init(url, args);

            Object getAsset = null;
            string path = url;

            if (IsEditorLoadAsset)
            {
#if UNITY_EDITOR
                if (path.EndsWith(".unity"))
                {
                    // scene
                    getAsset = KResourceLoader.Instance;
                    Log.Warning("Load scene from Build Settings: {0}", path);
                }
                else
                {
                    getAsset = UnityEditor.AssetDatabase.LoadAssetAtPath("Assets/" + path, typeof(UnityEngine.Object));
                    if (getAsset == null)
                    {
                        Log.Error("Asset is NULL(from {0} Folder): {1}", "ResourcesBuildDir", path);
                    }
                }
#else
				Log.Error("`IsEditorLoadAsset` is Unity Editor only");

#endif
                OnFinish(getAsset);

            }
            else
            {
                string extension = Path.GetExtension(path);
                path = path.Substring(0, path.Length - extension.Length); // remove extensions

                getAsset = Resources.Load<Object>(path);
                if (getAsset == null)
                {
                    Log.Error("Asset is NULL(from Resources Folder): {0}", path);
                }
                OnFinish(getAsset);
            }

            if (Application.isEditor)
            {
                if (getAsset != null)
                    KResoourceLoadedAssetDebugger.Create(getAsset.GetType().Name, Url, getAsset as Object);

                // 编辑器环境下，如果遇到GameObject，对Shader进行Fix
                if (getAsset is GameObject)
                {
                    var go = getAsset as GameObject;
                    foreach (var r in go.GetComponentsInChildren<Renderer>(true))
                    {
                        RefreshMaterialsShaders(r);
                    }
                }
            }

            if (getAsset != null)
            {
                // 更名~ 注明来源asset bundle 带有类型
                getAsset.name = String.Format("{0}~{1}", getAsset, Url);
            }
            OnFinish(getAsset);
        }

        /// <summary>
        /// 编辑器模式下，对指定GameObject刷新一下Material
        /// </summary>
        public static void RefreshMaterialsShaders(Renderer renderer)
        {
            if (renderer.sharedMaterials != null)
            {
                foreach (var mat in renderer.sharedMaterials)
                {
                    if (mat != null && mat.shader != null)
                    {
                        mat.shader = Shader.Find(mat.shader.name);
                    }
                }
            }
        }

        protected override void DoDispose()
        {
            base.DoDispose();
            {
                Resources.UnloadAsset(ResultObject as Object);
                //Object.DestroyObject(ResultObject as UnityEngine.Object);

                // Destroying GameObjects immediately is not permitted during physics trigger/contact, animation event callbacks or OnValidate. You must use Destroy instead.
                //                    Object.DestroyImmediate(ResultObject as Object, true);


                //var bRemove = Caches.Remove(Url);
                //if (!bRemove)
                //{
                //    Log.Warning("[DisposeTheCache]Remove Fail(可能有两个未完成的，同时来到这) : {0}", Url);
                //}

                //else
                //{
                //    // 交给加载后，进行检查并卸载资源
                //    // 可能情况TIPS：两个未完成的！会触发上面两次！
                //}
            }
        }
    }

}
