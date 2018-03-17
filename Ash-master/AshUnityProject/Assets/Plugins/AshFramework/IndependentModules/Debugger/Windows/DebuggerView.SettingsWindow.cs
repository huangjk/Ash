using Ash.Core;
using UnityEngine;

namespace Ash
{
    public partial class DebuggerView
    {
        private sealed class SettingsWindow : ScrollableDebuggerWindowBase
        {
            private DebuggerView m_DebuggerComponent = null;
            private float m_LastIconX = 0f;
            private float m_LastIconY = 0f;
            private float m_LastWindowX = 0f;
            private float m_LastWindowY = 0f;
            private float m_LastWindowWidth = 0f;
            private float m_LastWindowHeight = 0f;
            private float m_LastWindowScale = 0f;

            public override void Initialize(params object[] args)
            {
                m_DebuggerComponent = DebuggerView.GetInstance();
                if (m_DebuggerComponent == null)
                {
                    Log.Fatal("Debugger component is invalid.");
                    return;
                }

                m_LastIconX = Setting.GetInstance().Get("Debugger.Icon.X", DefaultIconRect.x);
                m_LastIconY = Setting.GetInstance().Get("Debugger.Icon.Y", DefaultIconRect.y);
                m_LastWindowX = Setting.GetInstance().Get("Debugger.Window.X", DefaultWindowRect.x);
                m_LastWindowY = Setting.GetInstance().Get("Debugger.Window.Y", DefaultWindowRect.y);
                m_LastWindowWidth = Setting.GetInstance().Get("Debugger.Window.Width", DefaultWindowRect.width);
                m_LastWindowHeight = Setting.GetInstance().Get("Debugger.Window.Height", DefaultWindowRect.height);
                m_DebuggerComponent.WindowScale = m_LastWindowScale = Setting.GetInstance().Get("Debugger.Window.Scale", DefaultWindowScale);
                m_DebuggerComponent.IconRect = new Rect(m_LastIconX, m_LastIconY, DefaultIconRect.width, DefaultIconRect.height);
                m_DebuggerComponent.WindowRect = new Rect(m_LastWindowX, m_LastWindowY, m_LastWindowWidth, m_LastWindowHeight);
            }

            public override void OnUpdate(float elapseSeconds, float realElapseSeconds)
            {
                if (m_LastIconX != m_DebuggerComponent.IconRect.x)
                {
                    m_LastIconX = m_DebuggerComponent.IconRect.x;
                    Setting.GetInstance().Set("Debugger.Icon.X", m_DebuggerComponent.IconRect.x);
                }

                if (m_LastIconY != m_DebuggerComponent.IconRect.y)
                {
                    m_LastIconY = m_DebuggerComponent.IconRect.y;
                    Setting.GetInstance().Set("Debugger.Icon.Y", m_DebuggerComponent.IconRect.y);
                }

                if (m_LastWindowX != m_DebuggerComponent.WindowRect.x)
                {
                    m_LastWindowX = m_DebuggerComponent.WindowRect.x;
                    Setting.GetInstance().Set("Debugger.Window.X", m_DebuggerComponent.WindowRect.x);
                }

                if (m_LastWindowY != m_DebuggerComponent.WindowRect.y)
                {
                    m_LastWindowY = m_DebuggerComponent.WindowRect.y;
                    Setting.GetInstance().Set("Debugger.Window.Y", m_DebuggerComponent.WindowRect.y);
                }

                if (m_LastWindowWidth != m_DebuggerComponent.WindowRect.width)
                {
                    m_LastWindowWidth = m_DebuggerComponent.WindowRect.width;
                    Setting.GetInstance().Set("Debugger.Window.Width", m_DebuggerComponent.WindowRect.width);
                }

                if (m_LastWindowHeight != m_DebuggerComponent.WindowRect.height)
                {
                    m_LastWindowHeight = m_DebuggerComponent.WindowRect.height;
                    Setting.GetInstance().Set("Debugger.Window.Height", m_DebuggerComponent.WindowRect.height);
                }

                if (m_LastWindowScale != m_DebuggerComponent.WindowScale)
                {
                    m_LastWindowScale = m_DebuggerComponent.WindowScale;
                    Setting.GetInstance().Set("Debugger.Window.Scale", m_DebuggerComponent.WindowScale);
                }
            }

            protected override void OnDrawScrollableWindow()
            {
                GUILayout.Label("<b>Window Settings</b>");
                GUILayout.BeginVertical("box");
                {
                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.Label("Position:", GUILayout.Width(60f));
                        GUILayout.Label("Drag window caption to move position.");
                    }
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    {
                        float width = m_DebuggerComponent.WindowRect.width;
                        GUILayout.Label("Width:", GUILayout.Width(60f));
                        if (GUILayout.RepeatButton("-", GUILayout.Width(30f)))
                        {
                            width--;
                        }
                        width = GUILayout.HorizontalSlider(width, 100f, Screen.width - 20f);
                        if (GUILayout.RepeatButton("+", GUILayout.Width(30f)))
                        {
                            width++;
                        }
                        width = Mathf.Clamp(width, 100f, Screen.width - 20f);
                        if (width != m_DebuggerComponent.WindowRect.width)
                        {
                            m_DebuggerComponent.WindowRect = new Rect(m_DebuggerComponent.WindowRect.x, m_DebuggerComponent.WindowRect.y, width, m_DebuggerComponent.WindowRect.height);
                        }
                    }
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    {
                        float height = m_DebuggerComponent.WindowRect.height;
                        GUILayout.Label("Height:", GUILayout.Width(60f));
                        if (GUILayout.RepeatButton("-", GUILayout.Width(30f)))
                        {
                            height--;
                        }
                        height = GUILayout.HorizontalSlider(height, 100f, Screen.height - 20f);
                        if (GUILayout.RepeatButton("+", GUILayout.Width(30f)))
                        {
                            height++;
                        }
                        height = Mathf.Clamp(height, 100f, Screen.height - 20f);
                        if (height != m_DebuggerComponent.WindowRect.height)
                        {
                            m_DebuggerComponent.WindowRect = new Rect(m_DebuggerComponent.WindowRect.x, m_DebuggerComponent.WindowRect.y, m_DebuggerComponent.WindowRect.width, height);
                        }
                    }
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    {
                        float scale = m_DebuggerComponent.WindowScale;
                        GUILayout.Label("Scale:", GUILayout.Width(60f));
                        if (GUILayout.RepeatButton("-", GUILayout.Width(30f)))
                        {
                            scale -= 0.01f;
                        }
                        scale = GUILayout.HorizontalSlider(scale, 0.5f, 4f);
                        if (GUILayout.RepeatButton("+", GUILayout.Width(30f)))
                        {
                            scale += 0.01f;
                        }
                        scale = Mathf.Clamp(scale, 0.5f, 4f);
                        if (scale != m_DebuggerComponent.WindowScale)
                        {
                            m_DebuggerComponent.WindowScale = scale;
                        }
                    }
                    GUILayout.EndHorizontal();

                    if (GUILayout.Button("Reset Window Settings", GUILayout.Height(30f)))
                    {
                        m_DebuggerComponent.IconRect = DefaultIconRect;
                        m_DebuggerComponent.WindowRect = DefaultWindowRect;
                        m_DebuggerComponent.WindowScale = DefaultWindowScale;
                    }
                }
                GUILayout.EndVertical();
            }
        }
    }
}
