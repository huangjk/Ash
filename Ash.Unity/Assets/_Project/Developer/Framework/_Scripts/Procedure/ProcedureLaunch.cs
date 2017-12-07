using System;
using UnityEngine;
using ProcedureOwner = Ash.Fsm.IFsm<Ash.Procedure.IProcedureManager>;
using Ash.Localization;
using Ash;
using AshUnity;

namespace Framework
{
    public class ProcedureLaunch : ProcedureBase
    {
        protected override void OnInit(ProcedureOwner procedureOwner)
        {
            base.OnInit(procedureOwner);
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);

            // 构建信息：发布版本时，把一些数据以 Json 的格式写入 Assets/GameMain/Configs/BuildInfo.txt，供游戏逻辑读取。
            // 获得版本号信息
            Entry.Config.InitAppBuildInfo();

            #region Setting
            // 语言配置：设置当前使用的语言，如果不设置，则默认使用操作系统语言。
            //InitLanguageSettings();
            // 变体配置：根据使用的语言，通知底层加载对应的资源变体。
            //InitCurrentVariant();
            // 画质配置：根据检测到的硬件信息 Assets/Main/Configs/DeviceModelConfig 和用户配置数据，设置即将使用的画质选项。
            //InitQualitySettings();
            // 声音配置：根据用户配置数据，设置即将使用的声音选项。
            //InitSoundSettings();
            #endregion

            // 默认字典：加载默认字典文件 Assets/GameMain/Configs/DefaultDictionary.xml。
            // 此字典文件记录了资源更新前使用的各种语言的字符串，会随 App 一起发布，故不可更新。
            Entry.Config.InitDefaultDictionary();
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            ChangeState<ProcedureSplash>(procedureOwner);
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
        }

        protected override void OnDestroy(ProcedureOwner procedureOwner)
        {
            base.OnDestroy(procedureOwner);
        }

        private void InitLanguageSettings()
        {
            if (Entry.Base.EditorResourceMode && Entry.Base.EditorLanguage != Language.Unspecified)
            {
                // 编辑器资源模式直接使用 Inspector 上设置的语言
                return;
            }

            Language language = Entry.Localization.Language;
            string languageString = Entry.Setting.GetString(Constant.Setting.Language);
            if (!string.IsNullOrEmpty(languageString))
            {
                try
                {
                    language = (Language)Enum.Parse(typeof(Language), languageString);
                }
                catch
                {

                }
            }

            if (language != Language.English
                && language != Language.ChineseSimplified)
            {
                // 若是暂不支持的语言，则使用英语
                language = Language.English;

                Entry.Setting.SetString(Constant.Setting.Language, language.ToString());
                Entry.Setting.Save();
            }

            Entry.Localization.Language = language;

            Log.Info("Init language settings complete, current language is '{0}'.", language.ToString());
        }

        private void InitCurrentVariant()
        {
            if (Entry.Base.EditorResourceMode)
            {
                // 编辑器资源模式不使用 AssetBundle，也就没有变体了
                return;
            }

            string currentVariant = null;
            switch (Entry.Localization.Language)
            {
                //根据语言类型设置变态，AssetBundle变体命名协议
                case Language.English:
                    currentVariant = "en-us";
                    break;
                case Language.ChineseSimplified:
                    currentVariant = "zh-cn";
                    break;
            }

            Entry.Resource.SetCurrentVariant(currentVariant);

            Log.Info("Init current variant complete.");
        }

        private void InitQualitySettings()
        {
            int qualityLevel = Entry.Setting.GetInt(Constant.Setting.QualityLevel,-1);
            if (qualityLevel == -1)
            {
                qualityLevel = (int)QualityLevelType.Fantastic;
            }

            Entry.Setting.SetInt(Constant.Setting.QualityLevel, qualityLevel);

            QualitySettings.SetQualityLevel(qualityLevel, true);

            //Screen.fullScreen = true;
            //Screen.SetResolution(1920, 1080, true);
            //QualitySettings.antiAliasing = 4;
            //Application.targetFrameRate = 60;
            //RenderSettings.ambientLight = Color.white;
            //Input.multiTouchEnabled = false;
            //Screen.sleepTimeout = SleepTimeout.NeverSleep;

            Log.Info("Init quality settings complete.");
        }

        private void InitSoundSettings()
        {
            Entry.Sound.Mute("Music", Entry.Setting.GetBool(Constant.Setting.MusicMuted, false));
            Entry.Sound.SetVolume("Music", Entry.Setting.GetFloat(Constant.Setting.MusicVolume, 0.3f));
            Entry.Sound.Mute("Sound", Entry.Setting.GetBool(Constant.Setting.SoundMuted, false));
            Entry.Sound.SetVolume("Sound", Entry.Setting.GetFloat(Constant.Setting.SoundVolume, 1f));
            Entry.Sound.Mute("UISound", Entry.Setting.GetBool(Constant.Setting.UISoundMuted, false));
            Entry.Sound.SetVolume("UISound", Entry.Setting.GetFloat(Constant.Setting.UISoundVolume, 1f));

            Log.Info("Init sound settings complete.");
        }
    }
}
