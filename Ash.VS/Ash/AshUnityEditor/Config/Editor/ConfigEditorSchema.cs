using Ash;
using AshUnity;
using AshUnity.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace AshUnityEditor.Config
{
    public enum WindowType
    {
        INPUT,
        CALLBACK
    }

    public class ConfigEditorSchema<T> : UnityEditor.EditorWindow where T : ConfigModel, new()
    {
        private ConfigEditorBase<T> _configEditorBase;
        private bool _initialized;

        private WindowType _currentWindowType = WindowType.INPUT;

        public WindowType CurrentWindowType
        {
            get
            {
                return _currentWindowType;
            }
        }

        private FileSystem _fileSystem;

        public FileSystem FileSystem
        {
            get
            {
                if (_fileSystem == null) return AshApp.ConfigDefaultFileSystem;
                return _fileSystem;
            }

            set
            {
                _fileSystem = value;
            }
        }

        /// <summary>
        /// 新建一条配置
        /// </summary>
        /// <returns>Data对象</returns>
        public virtual T CreateValue()
        {
            return _configEditorBase.CreateValue();
        }

        public virtual void Initialize(){}

        public virtual void SetConfigType(ConfigBase<T> tp)
        {
            _configEditorBase.SetConfigType(tp);
        }

        public void OnGUI()
        {
            if (Event.current.isKey && Event.current.keyCode == KeyCode.Escape)
            {
                Close();
                return;
            }

            if (!_initialized)
            {
                switch (_currentWindowType)
                {
                    case WindowType.INPUT:
                        {
                            _configEditorBase = new ConfigEditorInputWindowType<T>(this);
                        }
                        break;
                    case WindowType.CALLBACK:
                        {
                            _configEditorBase = new ConfigEditorBase<T>();
                        }
                        break;
                }


                Initialize();

                _initialized = true;
            }

            _configEditorBase.Draw();
        }     
    }
}
