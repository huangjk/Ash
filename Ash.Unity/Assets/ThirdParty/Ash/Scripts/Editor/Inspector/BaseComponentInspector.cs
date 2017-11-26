﻿using Ash;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using AshUnity;

namespace AshUnityEditor
{
    [CustomEditor(typeof(AshBase))]
    internal sealed class AshBaseInspector : AshInspector
    {
        private const string NoneOptionName = "<None>";

        private readonly float[] GameSpeed = new float[] { 0f, 0.25f, 0.5f, 1f, 1.5f, 2f, 4f, 8f };
        private readonly string[] GameSpeedTexts = new string[] { "0x", "0.25x", "0.5x", "1x", "1.5x", "2x", "4x", "8x" };

        private SerializedProperty m_EditorResourceMode = null;
        private SerializedProperty m_EditorLanguage = null;
        private SerializedProperty m_ZipHelperTypeName = null;
        private SerializedProperty m_JsonHelperTypeName = null;
        private SerializedProperty m_ProfilerHelperTypeName = null;
        private SerializedProperty m_FrameRate = null;
        private SerializedProperty m_GameSpeed = null;
        private SerializedProperty m_RunInBackground = null;
        private SerializedProperty m_NeverSleep = null;
        private SerializedProperty m_LogLevel = null;
        private SerializedProperty m_IsLogFile = null;

        private string[] m_ZipHelperTypeNames = null;
        private int m_ZipHelperTypeNameIndex = 0;
        private string[] m_JsonHelperTypeNames = null;
        private int m_JsonHelperTypeNameIndex = 0;
        private string[] m_ProfilerHelperTypeNames = null;
        private int m_ProfilerHelperTypeNameIndex = 0;

        //private int m_LogLevelIndex = 0;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            AshBase t = (AshBase)target;

            EditorGUI.BeginDisabledGroup(EditorApplication.isPlayingOrWillChangePlaymode);
            {
                m_EditorResourceMode.boolValue = EditorGUILayout.BeginToggleGroup("Editor Resource Mode", m_EditorResourceMode.boolValue);
               {
                    EditorGUILayout.HelpBox("Editor resource mode option is only for editor mode. Game Framework will use editor resource files, which you should validate first.", MessageType.Warning);
                    EditorGUILayout.PropertyField(m_EditorLanguage);
                    EditorGUILayout.HelpBox("Editor language option is only use for localization test in editor mode.", MessageType.Info);
                }
                EditorGUILayout.EndToggleGroup();

                EditorGUILayout.BeginVertical("box");
                {
                    EditorGUILayout.LabelField("Global Helpers");

                    int zipHelperSelectedIndex = EditorGUILayout.Popup("Zip Helper", m_ZipHelperTypeNameIndex, m_ZipHelperTypeNames);
                    if (zipHelperSelectedIndex != m_ZipHelperTypeNameIndex)
                    {
                        m_ZipHelperTypeNameIndex = zipHelperSelectedIndex;
                        m_ZipHelperTypeName.stringValue = (zipHelperSelectedIndex <= 0 ? null : m_ZipHelperTypeNames[zipHelperSelectedIndex]);
                    }

                    int jsonHelperSelectedIndex = EditorGUILayout.Popup("JSON Helper", m_JsonHelperTypeNameIndex, m_JsonHelperTypeNames);
                    if (jsonHelperSelectedIndex != m_JsonHelperTypeNameIndex)
                    {
                        m_JsonHelperTypeNameIndex = jsonHelperSelectedIndex;
                        m_JsonHelperTypeName.stringValue = (jsonHelperSelectedIndex <= 0 ? null : m_JsonHelperTypeNames[jsonHelperSelectedIndex]);
                    }

                    int profilerHelperSelectedIndex = EditorGUILayout.Popup("Profiler Helper", m_ProfilerHelperTypeNameIndex, m_ProfilerHelperTypeNames);
                    if (profilerHelperSelectedIndex != m_ProfilerHelperTypeNameIndex)
                    {
                        m_ProfilerHelperTypeNameIndex = profilerHelperSelectedIndex;
                        m_ProfilerHelperTypeName.stringValue = (profilerHelperSelectedIndex <= 0 ? null : m_ProfilerHelperTypeNames[profilerHelperSelectedIndex]);
                    }
                }
                EditorGUILayout.EndVertical();
            }
            EditorGUI.EndDisabledGroup();

            int frameRate = EditorGUILayout.IntSlider("Frame Rate", m_FrameRate.intValue, 1, 120);
            if (frameRate != m_FrameRate.intValue)
            {
                if (EditorApplication.isPlaying)
                {
                    t.FrameRate = frameRate;
                }
                else
                {
                    m_FrameRate.intValue = frameRate;
                }
            }

            EditorGUILayout.BeginVertical("box");
            {
                float gameSpeed = EditorGUILayout.Slider("Game Speed", m_GameSpeed.floatValue, 0f, 8f);
                int selectedGameSpeed = GUILayout.SelectionGrid(GetSelectedGameSpeed(gameSpeed), GameSpeedTexts, 4);
                if (selectedGameSpeed >= 0)
                {
                    gameSpeed = GetGameSpeed(selectedGameSpeed);
                }

                if (gameSpeed != m_GameSpeed.floatValue)
                {
                    if (EditorApplication.isPlaying)
                    {
                        t.GameSpeed = gameSpeed;
                    }
                    else
                    {
                        m_GameSpeed.floatValue = gameSpeed;
                    }
                }
            }
            EditorGUILayout.EndVertical();

            //EditorGUILayout.Space();
            //EditorGUILayout.LabelField("Log");

            EditorGUILayout.PropertyField(m_LogLevel);
            EditorGUILayout.PropertyField(m_IsLogFile);

            //LogLevel logLevel = (LogLevel)EditorGUILayout.EnumPopup("Log Level",(LogLevel)m_LogLevel.enumValueIndex);
            //if (logLevel != (LogLevel)m_LogLevel.enumValueIndex)
            //{
            //    m_LogLevel.enumValueIndex = (int)logLevel;
            //    t.LogLevel = logLevel;
            //}

            //bool isLogFile = EditorGUILayout.Toggle("Is Log File", m_IsLogFile.boolValue);
            //if (isLogFile != m_IsLogFile.boolValue)
            //{
            //    if (EditorApplication.isPlaying)
            //    {
            //        t.IsLogFile = isLogFile;
            //    }
            //    else
            //    {
            //        m_IsLogFile.boolValue = isLogFile;
            //    }
            //}
            EditorGUILayout.Space();
            //EditorGUILayout.LabelField("Other");

            bool runInBackground = EditorGUILayout.Toggle("Run in Background", m_RunInBackground.boolValue);
            if (runInBackground != m_RunInBackground.boolValue)
            {
                if (EditorApplication.isPlaying)
                {
                    t.RunInBackground = runInBackground;
                }
                else
                {
                    m_RunInBackground.boolValue = runInBackground;
                }
            }

            bool neverSleep = EditorGUILayout.Toggle("Never Sleep", m_NeverSleep.boolValue);
            if (neverSleep != m_NeverSleep.boolValue)
            {
                if (EditorApplication.isPlaying)
                {
                    t.NeverSleep = neverSleep;
                }
                else
                {
                    m_NeverSleep.boolValue = neverSleep;
                }
            }

            serializedObject.ApplyModifiedProperties();
        }

        protected override void OnCompileComplete()
        {
            base.OnCompileComplete();

            RefreshTypeNames();
        }

        private void OnEnable()
        {
            m_EditorResourceMode = serializedObject.FindProperty("m_EditorResourceMode");
            m_EditorLanguage = serializedObject.FindProperty("m_EditorLanguage");
            m_ZipHelperTypeName = serializedObject.FindProperty("m_ZipHelperTypeName");
            m_JsonHelperTypeName = serializedObject.FindProperty("m_JsonHelperTypeName");
            m_ProfilerHelperTypeName = serializedObject.FindProperty("m_ProfilerHelperTypeName");
            m_FrameRate = serializedObject.FindProperty("m_FrameRate");
            m_GameSpeed = serializedObject.FindProperty("m_GameSpeed");
            m_RunInBackground = serializedObject.FindProperty("m_RunInBackground");
            m_NeverSleep = serializedObject.FindProperty("m_NeverSleep");

            m_LogLevel = serializedObject.FindProperty("m_LogLevel");
            m_IsLogFile = serializedObject.FindProperty("m_IsLogFile");
            
            RefreshTypeNames();
        }

        private void RefreshTypeNames()
        {
            List<string> zipHelperTypeNames = new List<string>();
            zipHelperTypeNames.Add(NoneOptionName);
            zipHelperTypeNames.AddRange(TypeHelper.GetTypeNames(typeof(Utility.Zip.IZipHelper)));
            m_ZipHelperTypeNames = zipHelperTypeNames.ToArray();
            m_ZipHelperTypeNameIndex = 0;
            if (!string.IsNullOrEmpty(m_ZipHelperTypeName.stringValue))
            {
                m_ZipHelperTypeNameIndex = zipHelperTypeNames.IndexOf(m_ZipHelperTypeName.stringValue);
                if (m_ZipHelperTypeNameIndex <= 0)
                {
                    m_ZipHelperTypeNameIndex = 0;
                    m_ZipHelperTypeName.stringValue = null;
                }
            }

            List<string> jsonHelperTypeNames = new List<string>();
            jsonHelperTypeNames = new List<string>();
            jsonHelperTypeNames.Add(NoneOptionName);
            jsonHelperTypeNames.AddRange(TypeHelper.GetTypeNames(typeof(Utility.Json.IJsonHelper)));
            m_JsonHelperTypeNames = jsonHelperTypeNames.ToArray();
            m_JsonHelperTypeNameIndex = 0;
            if (!string.IsNullOrEmpty(m_JsonHelperTypeName.stringValue))
            {
                m_JsonHelperTypeNameIndex = jsonHelperTypeNames.IndexOf(m_JsonHelperTypeName.stringValue);
                if (m_JsonHelperTypeNameIndex <= 0)
                {
                    m_JsonHelperTypeNameIndex = 0;
                    m_JsonHelperTypeName.stringValue = null;
                }
            }

            List<string> profilerHelperTypeNames = new List<string>();
            profilerHelperTypeNames.Add(NoneOptionName);
            profilerHelperTypeNames.AddRange(TypeHelper.GetTypeNames(typeof(Utility.Profiler.IProfilerHelper)));
            m_ProfilerHelperTypeNames = profilerHelperTypeNames.ToArray();
            m_ProfilerHelperTypeNameIndex = 0;
            if (!string.IsNullOrEmpty(m_ProfilerHelperTypeName.stringValue))
            {
                m_ProfilerHelperTypeNameIndex = profilerHelperTypeNames.IndexOf(m_ProfilerHelperTypeName.stringValue);
                if (m_ProfilerHelperTypeNameIndex <= 0)
                {
                    m_ProfilerHelperTypeNameIndex = 0;
                    m_ProfilerHelperTypeName.stringValue = null;
                }
            }

            serializedObject.ApplyModifiedProperties();
        }

        private float GetGameSpeed(int selectedGameSpeed)
        {
            if (selectedGameSpeed < 0)
            {
                return GameSpeed[0];
            }

            if (selectedGameSpeed >= GameSpeed.Length)
            {
                return GameSpeed[GameSpeed.Length - 1];
            }

            return GameSpeed[selectedGameSpeed];
        }

        private int GetSelectedGameSpeed(float gameSpeed)
        {
            for (int i = 0; i < GameSpeed.Length; i++)
            {
                if (gameSpeed == GameSpeed[i])
                {
                    return i;
                }
            }

            return -1;
        }
    }
}