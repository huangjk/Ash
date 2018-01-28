






using UnityEngine;

namespace Ash.Runtime
{
    public partial class DebuggerComponent
    {
        private sealed class OperationsWindow : ScrollableDebuggerWindowBase
        {
            protected override void OnDrawScrollableWindow()
            {
                GUILayout.Label("<b>Operations</b>");
                GUILayout.BeginVertical("box");
                {
                    ObjectPoolComponent objectPoolComponent = AshUnityEntry.Instance.GetComponent<ObjectPoolComponent>();
                    if (objectPoolComponent != null)
                    {
                        if (GUILayout.Button("Object Pool Release", GUILayout.Height(30f)))
                        {
                            objectPoolComponent.Release();
                        }

                        if (GUILayout.Button("Object Pool Release All Unused", GUILayout.Height(30f)))
                        {
                            objectPoolComponent.ReleaseAllUnused();
                        }
                    }

                    ResourceComponent resourceCompoent = AshUnityEntry.Instance.GetComponent<ResourceComponent>();
                    if (resourceCompoent != null)
                    {
                        if (GUILayout.Button("Unload Unused Assets", GUILayout.Height(30f)))
                        {
                            resourceCompoent.ForceUnloadUnusedAssets(false);
                        }

                        if (GUILayout.Button("Unload Unused Assets and Garbage Collect", GUILayout.Height(30f)))
                        {
                            resourceCompoent.ForceUnloadUnusedAssets(true);
                        }
                    }

                    //if (GUILayout.Button("Shutdown Game Framework (None)", GUILayout.Height(30f)))
                    //{
                    //    AshUnityEntry.Instance.Shutdown(ShutdownType.None);
                    //}
                    //if (GUILayout.Button("Shutdown Game Framework (Restart)", GUILayout.Height(30f)))
                    //{
                    //    AshUnityEntry.Instance.Shutdown(ShutdownType.Restart);
                    //}
                    //if (GUILayout.Button("Shutdown Game Framework (Quit)", GUILayout.Height(30f)))
                    //{
                    //    AshUnityEntry.Instance.Shutdown(ShutdownType.Quit);
                    //}
                }
                GUILayout.EndVertical();
            }
        }
    }
}
