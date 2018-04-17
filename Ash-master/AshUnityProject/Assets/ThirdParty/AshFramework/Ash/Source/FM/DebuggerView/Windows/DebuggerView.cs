using System.Collections.Generic;
using UnityEngine;

namespace Ash
{
    /// <summary>
    /// 调试组件。
    /// </summary>
    [DisallowMultipleComponent]
    //[AddComponentMenu("Ash Framework/DebuggerView")]
    public sealed partial class DebuggerView : MonoBehaviour
    {
        internal static DebuggerView _instance = null;

        /// <summary>
        /// Get the singleton
        /// </summary>
        /// <returns></returns>
        public static DebuggerView Instance
        {
            get
            {
                if (_instance == null)
                {
                    //_instance = new MyGameData(new UnityDataLocator());

                    string gameObjectName = "DebuggerView";
                    GameObject gameObjectToAttach = new GameObject(gameObjectName);

                    gameObjectToAttach.hideFlags = HideFlags.HideInHierarchy;
                    _instance = gameObjectToAttach.AddComponent<DebuggerView>();
                }

                return _instance;
            }
        }

        /// <summary>
        /// 默认调试器漂浮框大小。
        /// </summary>
        internal static readonly Rect DefaultIconRect = new Rect(10f, 10f, 60f, 60f);

        /// <summary>
        /// 默认调试器窗口大小。
        /// </summary>
        internal static readonly Rect DefaultWindowRect = new Rect(10f, 10f, 640f, 480f);

        /// <summary>
        /// 默认调试器窗口缩放比例。
        /// </summary>
        internal static readonly float DefaultWindowScale = 1f;

        private IDebuggerManager m_DebuggerManager = null;
        private Rect m_DragRect = new Rect(0f, 0f, float.MaxValue, 25f);
        private Rect m_IconRect = DefaultIconRect;
        private Rect m_WindowRect = DefaultWindowRect;
        private float m_WindowScale = DefaultWindowScale;

        [SerializeField]
        private GUISkin m_Skin = null;

        [SerializeField]
        private DebuggerActiveWindowType m_ActiveWindow = DebuggerActiveWindowType.Auto;

        [SerializeField]
        private bool m_ShowFullWindow = false;

        [SerializeField]
        private ConsoleWindow m_ConsoleWindow = new ConsoleWindow();

        //private SystemInformationWindow m_SystemInformationWindow = new SystemInformationWindow();
        //private EnvironmentInformationWindow m_EnvironmentInformationWindow = new EnvironmentInformationWindow();
        //private ScreenInformationWindow m_ScreenInformationWindow = new ScreenInformationWindow();
        //private GraphicsInformationWindow m_GraphicsInformationWindow = new GraphicsInformationWindow();
        //private InputSummaryInformationWindow m_InputSummaryInformationWindow = new InputSummaryInformationWindow();
        //private InputTouchInformationWindow m_InputTouchInformationWindow = new InputTouchInformationWindow();
        //private InputLocationInformationWindow m_InputLocationInformationWindow = new InputLocationInformationWindow();
        //private InputAccelerationInformationWindow m_InputAccelerationInformationWindow = new InputAccelerationInformationWindow();
        //private InputGyroscopeInformationWindow m_InputGyroscopeInformationWindow = new InputGyroscopeInformationWindow();
        //private InputCompassInformationWindow m_InputCompassInformationWindow = new InputCompassInformationWindow();
        //private PathInformationWindow m_PathInformationWindow = new PathInformationWindow();
        //private SceneInformationWindow m_SceneInformationWindow = new SceneInformationWindow();
        //private TimeInformationWindow m_TimeInformationWindow = new TimeInformationWindow();
        //private QualityInformationWindow m_QualityInformationWindow = new QualityInformationWindow();
        //private ProfilerInformationWindow m_ProfilerInformationWindow = new ProfilerInformationWindow();
        //private WebPlayerInformationWindow m_WebPlayerInformationWindow = new WebPlayerInformationWindow();
        //private RuntimeMemoryInformationWindow<Object> m_RuntimeMemoryAllInformationWindow = new RuntimeMemoryInformationWindow<Object>();
        //private RuntimeMemoryInformationWindow<Texture> m_RuntimeMemoryTextureInformationWindow = new RuntimeMemoryInformationWindow<Texture>();
        //private RuntimeMemoryInformationWindow<Mesh> m_RuntimeMemoryMeshInformationWindow = new RuntimeMemoryInformationWindow<Mesh>();
        //private RuntimeMemoryInformationWindow<Material> m_RuntimeMemoryMaterialInformationWindow = new RuntimeMemoryInformationWindow<Material>();
        //private RuntimeMemoryInformationWindow<AnimationClip> m_RuntimeMemoryAnimationClipInformationWindow = new RuntimeMemoryInformationWindow<AnimationClip>();
        //private RuntimeMemoryInformationWindow<AudioClip> m_RuntimeMemoryAudioClipInformationWindow = new RuntimeMemoryInformationWindow<AudioClip>();
        //private RuntimeMemoryInformationWindow<Font> m_RuntimeMemoryFontInformationWindow = new RuntimeMemoryInformationWindow<Font>();
        //private RuntimeMemoryInformationWindow<GameObject> m_RuntimeMemoryGameObjectInformationWindow = new RuntimeMemoryInformationWindow<GameObject>();
        //private RuntimeMemoryInformationWindow<Component> m_RuntimeMemoryComponentInformationWindow = new RuntimeMemoryInformationWindow<Component>();
        //private ObjectPoolInformationWindow m_ObjectPoolInformationWindow = new ObjectPoolInformationWindow();

        private SettingsWindow m_SettingsWindow = new SettingsWindow();
        //private OperationsWindow m_OperationsWindow = new OperationsWindow();

        private FpsCounter m_FpsCounter = null;

        /// <summary>
        /// 获取或设置调试窗口是否激活。
        /// </summary>
        public bool ActiveWindow
        {
            get
            {
                return m_DebuggerManager.ActiveWindow;
            }
            set
            {
                m_DebuggerManager.ActiveWindow = value;
                enabled = value;
            }
        }

        /// <summary>
        /// 获取或设置是否显示完整调试器界面。
        /// </summary>
        public bool ShowFullWindow
        {
            get
            {
                return m_ShowFullWindow;
            }
            set
            {
                m_ShowFullWindow = value;
            }
        }

        /// <summary>
        /// 获取或设置调试器漂浮框大小。
        /// </summary>
        public Rect IconRect
        {
            get
            {
                return m_IconRect;
            }
            set
            {
                m_IconRect = value;
            }
        }

        /// <summary>
        /// 获取或设置调试器窗口大小。
        /// </summary>
        public Rect WindowRect
        {
            get
            {
                return m_WindowRect;
            }
            set
            {
                m_WindowRect = value;
            }
        }

        /// <summary>
        /// 获取或设置调试器窗口缩放比例。
        /// </summary>
        public float WindowScale
        {
            get
            {
                return m_WindowScale;
            }
            set
            {
                m_WindowScale = value;
            }
        }

        /// <summary>
        /// 游戏框架组件初始化。
        /// </summary>
        private void Awake()
        {
            if (_instance == null) _instance = this;

            if (_instance != null && _instance != this)
            {
                Destroy(this.gameObject);
                return;
            }

            m_DebuggerManager = new DebuggerManager();

            if (m_ActiveWindow == DebuggerActiveWindowType.Auto)
            {
                ActiveWindow = Debug.isDebugBuild;
            }
            else
            {
                ActiveWindow = (m_ActiveWindow == DebuggerActiveWindowType.Open);
            }

            m_FpsCounter = new FpsCounter(0.5f);
        }

        private void Start()
        {
            //注册消息窗口
            RegisterDebuggerWindow("Console", m_ConsoleWindow);
            //注册环境-系统信息窗口
            var m_SystemInformationWindow = new SystemInformationWindow();
            RegisterDebuggerWindow("Information/System", m_SystemInformationWindow);
            //注册环境-环境信息窗口
            var m_EnvironmentInformationWindow = new EnvironmentInformationWindow();
            RegisterDebuggerWindow("Information/Environment", m_EnvironmentInformationWindow);

            var m_ScreenInformationWindow = new ScreenInformationWindow();
            RegisterDebuggerWindow("Information/Screen", m_ScreenInformationWindow);

            var m_GraphicsInformationWindow = new GraphicsInformationWindow();
            RegisterDebuggerWindow("Information/Graphics", m_GraphicsInformationWindow);

            var m_InputSummaryInformationWindow = new InputSummaryInformationWindow();
            RegisterDebuggerWindow("Information/Input/Summary", m_InputSummaryInformationWindow);

            var m_InputTouchInformationWindow = new InputTouchInformationWindow();
            RegisterDebuggerWindow("Information/Input/Touch", m_InputTouchInformationWindow);

            //RegisterDebuggerWindow("Information/Input/Location", m_InputLocationInformationWindow);
            //RegisterDebuggerWindow("Information/Input/Acceleration", m_InputAccelerationInformationWindow);
            //RegisterDebuggerWindow("Information/Input/Gyroscope", m_InputGyroscopeInformationWindow);
            //RegisterDebuggerWindow("Information/Input/Compass", m_InputCompassInformationWindow);
            //RegisterDebuggerWindow("Information/Other/Scene", m_SceneInformationWindow);

            var m_PathInformationWindow = new PathInformationWindow();
            RegisterDebuggerWindow("Information/Other/Path", m_PathInformationWindow);
            var m_TimeInformationWindow = new TimeInformationWindow();
            RegisterDebuggerWindow("Information/Other/Time", m_TimeInformationWindow);
            var m_QualityInformationWindow = new QualityInformationWindow();
            RegisterDebuggerWindow("Information/Other/Quality", m_QualityInformationWindow);
            //RegisterDebuggerWindow("Information/Other/Web Player", m_WebPlayerInformationWindow);
            var m_ProfilerInformationWindow = new ProfilerInformationWindow();
            RegisterDebuggerWindow("Profiler/Summary", m_ProfilerInformationWindow);
            var m_RuntimeMemoryAllInformationWindow = new RuntimeMemoryInformationWindow<Object>();
            RegisterDebuggerWindow("Profiler/Memory/All", m_RuntimeMemoryAllInformationWindow);
            var m_RuntimeMemoryTextureInformationWindow = new RuntimeMemoryInformationWindow<Texture>();
            RegisterDebuggerWindow("Profiler/Memory/Texture", m_RuntimeMemoryTextureInformationWindow);
            var m_RuntimeMemoryMeshInformationWindow = new RuntimeMemoryInformationWindow<Mesh>();
            RegisterDebuggerWindow("Profiler/Memory/Mesh", m_RuntimeMemoryMeshInformationWindow);
            var m_RuntimeMemoryMaterialInformationWindow = new RuntimeMemoryInformationWindow<Material>();
            RegisterDebuggerWindow("Profiler/Memory/Material", m_RuntimeMemoryMaterialInformationWindow);
            var m_RuntimeMemoryAnimationClipInformationWindow = new RuntimeMemoryInformationWindow<AnimationClip>();
            RegisterDebuggerWindow("Profiler/Memory/AnimationClip", m_RuntimeMemoryAnimationClipInformationWindow);
            var m_RuntimeMemoryAudioClipInformationWindow = new RuntimeMemoryInformationWindow<AudioClip>();
            RegisterDebuggerWindow("Profiler/Memory/AudioClip", m_RuntimeMemoryAudioClipInformationWindow);
            var m_RuntimeMemoryFontInformationWindow = new RuntimeMemoryInformationWindow<Font>();
            RegisterDebuggerWindow("Profiler/Memory/Font", m_RuntimeMemoryFontInformationWindow);
            var m_RuntimeMemoryGameObjectInformationWindow = new RuntimeMemoryInformationWindow<GameObject>();
            RegisterDebuggerWindow("Profiler/Memory/GameObject", m_RuntimeMemoryGameObjectInformationWindow);
            var m_RuntimeMemoryComponentInformationWindow = new RuntimeMemoryInformationWindow<Component>();
            RegisterDebuggerWindow("Profiler/Memory/Component", m_RuntimeMemoryComponentInformationWindow);
            //if (AshUnityEntry.Instance.GetComponent<ObjectPoolComponent>() != null)
            //{
            //    RegisterDebuggerWindow("Profiler/Object Pool", m_ObjectPoolInformationWindow);
            //}
            RegisterDebuggerWindow("Other/Settings", m_SettingsWindow);
            //RegisterDebuggerWindow("Other/Operations", m_OperationsWindow);
        }

        private void Update()
        {
            m_FpsCounter.Update(Time.deltaTime, Time.unscaledDeltaTime);
        }

        private void OnGUI()
        {
            if (m_DebuggerManager == null || !m_DebuggerManager.ActiveWindow)
            {
                return;
            }

            GUISkin cachedGuiSkin = GUI.skin;
            Matrix4x4 cachedMatrix = GUI.matrix;

            GUI.skin = m_Skin;
            GUI.matrix = Matrix4x4.Scale(new Vector3(m_WindowScale, m_WindowScale, 1f));

            if (m_ShowFullWindow)
            {
                m_WindowRect = GUILayout.Window(0, m_WindowRect, DrawWindow, "<b>GAME FRAMEWORK DEBUGGER</b>");
            }
            else
            {
                m_IconRect = GUILayout.Window(0, m_IconRect, DrawDebuggerWindowIcon, "<b>DEBUGGER</b>");
            }

            GUI.matrix = cachedMatrix;
            GUI.skin = cachedGuiSkin;
        }

        /// <summary>
        /// 注册调试窗口。
        /// </summary>
        /// <param name="path">调试窗口路径。</param>
        /// <param name="debuggerWindow">要注册的调试窗口。</param>
        /// <param name="args">初始化调试窗口参数。</param>
        public void RegisterDebuggerWindow(string path, IDebuggerWindow debuggerWindow, params object[] args)
        {
            m_DebuggerManager.RegisterDebuggerWindow(path, debuggerWindow, args);
        }

        /// <summary>
        /// 获取调试窗口。
        /// </summary>
        /// <param name="path">调试窗口路径。</param>
        /// <returns>要获取的调试窗口。</returns>
        public IDebuggerWindow GetDebuggerWindow(string path)
        {
            return m_DebuggerManager.GetDebuggerWindow(path);
        }

        private void DrawWindow(int windowId)
        {
            GUI.DragWindow(m_DragRect);
            DrawDebuggerWindowGroup(m_DebuggerManager.DebuggerWindowRoot);
        }

        private void DrawDebuggerWindowGroup(IDebuggerWindowGroup debuggerWindowGroup)
        {
            if (debuggerWindowGroup == null)
            {
                return;
            }

            List<string> names = new List<string>();
            string[] debuggerWindowNames = debuggerWindowGroup.GetDebuggerWindowNames();
            for (int i = 0; i < debuggerWindowNames.Length; i++)
            {
                names.Add(string.Format("<b>{0}</b>", debuggerWindowNames[i]));
            }

            if (debuggerWindowGroup == m_DebuggerManager.DebuggerWindowRoot)
            {
                names.Add("<b>Close</b>");
            }

            int toolbarIndex = GUILayout.Toolbar(debuggerWindowGroup.SelectedIndex, names.ToArray(), GUILayout.Height(30f), GUILayout.MaxWidth(Screen.width));
            if (toolbarIndex >= debuggerWindowGroup.DebuggerWindowCount)
            {
                m_ShowFullWindow = false;
                return;
            }

            if (debuggerWindowGroup.SelectedIndex != toolbarIndex)
            {
                debuggerWindowGroup.SelectedWindow.OnLeave();
                debuggerWindowGroup.SelectedIndex = toolbarIndex;
                debuggerWindowGroup.SelectedWindow.OnEnter();
            }

            IDebuggerWindowGroup subDebuggerWindowGroup = debuggerWindowGroup.SelectedWindow as IDebuggerWindowGroup;
            if (subDebuggerWindowGroup != null)
            {
                DrawDebuggerWindowGroup(subDebuggerWindowGroup);
            }

            if (debuggerWindowGroup.SelectedWindow != null)
            {
                debuggerWindowGroup.SelectedWindow.OnDraw();
            }
        }

        private void DrawDebuggerWindowIcon(int windowId)
        {
            GUI.DragWindow(m_DragRect);
            GUILayout.Space(5);
            Color32 color = Color.white;
            m_ConsoleWindow.RefreshCount();
            if (m_ConsoleWindow.FatalCount > 0)
            {
                color = m_ConsoleWindow.GetLogStringColor(LogType.Exception);
            }
            else if (m_ConsoleWindow.ErrorCount > 0)
            {
                color = m_ConsoleWindow.GetLogStringColor(LogType.Error);
            }
            else if (m_ConsoleWindow.WarningCount > 0)
            {
                color = m_ConsoleWindow.GetLogStringColor(LogType.Warning);
            }
            else
            {
                color = m_ConsoleWindow.GetLogStringColor(LogType.Log);
            }

            string title = string.Format("<color=#{0}{1}{2}{3}><b>FPS: {4}</b></color>", color.r.ToString("x2"), color.g.ToString("x2"), color.b.ToString("x2"), color.a.ToString("x2"), m_FpsCounter.CurrentFps.ToString("F2"));
            if (GUILayout.Button(title, GUILayout.Width(100f), GUILayout.Height(40f)))
            {
                m_ShowFullWindow = true;
            }
        }
    }
}