﻿using Ash;
using Ash.DataTable;
using Ash.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AshUnity;
using System;

namespace Framework
{
    public static class UIExtension
    {
        public static IEnumerator FadeToAlpha(this CanvasGroup canvasGroup, float alpha, float duration)
        {
            float time = 0f;
            float originalAlpha = canvasGroup.alpha;
            while (time < duration)
            {
                time += Time.deltaTime;
                canvasGroup.alpha = Mathf.Lerp(originalAlpha, alpha, time / duration);
                yield return new WaitForEndOfFrame();
            }

            canvasGroup.alpha = alpha;
        }

        public static IEnumerator SmoothValue(this Slider slider, float value, float duration)
        {
            float time = 0f;
            float originalValue = slider.value;
            while (time < duration)
            {
                time += Time.deltaTime;
                slider.value = Mathf.Lerp(originalValue, value, time / duration);
                yield return new WaitForEndOfFrame();
            }

            slider.value = value;
        }

        public static bool HasUIForm(this UIComponent uiComponent, UIFormId uiFormId, string uiGroupName = null)
        {
            return uiComponent.HasUIForm((int)uiFormId, uiGroupName);
        }

        public static bool HasUIForm(this UIComponent uiComponent, int uiFormId, string uiGroupName = null)
        {
            IDataTable<DRUIForm> dtUIForm = Entry.DataTable.GetDataTable<DRUIForm>();
            DRUIForm drUIForm = dtUIForm.GetDataRow(uiFormId);
            if (drUIForm == null)
            {
                return false;
            }

            string assetName = AssetUtility.GetUIFormAsset(drUIForm.AssetName);
            if (string.IsNullOrEmpty(uiGroupName))
            {
                return uiComponent.HasUIForm(assetName);
            }

            IUIGroup uiGroup = uiComponent.GetUIGroup(uiGroupName);
            if (uiGroup == null)
            {
                return false;
            }

            return uiGroup.HasUIForm(assetName);
        }

        public static UGuiForm GetUIForm(this UIComponent uiComponent, UIFormId uiFormId, string uiGroupName = null)
        {
            return uiComponent.GetUIForm((int)uiFormId, uiGroupName);
        }

        public static UGuiForm GetUIForm(this UIComponent uiComponent, int uiFormId, string uiGroupName = null)
        {
            IDataTable<DRUIForm> dtUIForm = Entry.DataTable.GetDataTable<DRUIForm>();
            DRUIForm drUIForm = dtUIForm.GetDataRow(uiFormId);
            if (drUIForm == null)
            {
                return null;
            }

            string assetName = AssetUtility.GetUIFormAsset(drUIForm.AssetName);
            UIForm uiForm = null;
            if (string.IsNullOrEmpty(uiGroupName))
            {
                uiForm = uiComponent.GetUIForm(assetName);
                if (uiForm == null)
                {
                    return null;
                }

                return (UGuiForm)uiForm.Logic;
            }

            IUIGroup uiGroup = uiComponent.GetUIGroup(uiGroupName);
            if (uiGroup == null)
            {
                return null;
            }

            uiForm = (UIForm)uiGroup.GetUIForm(assetName);
            if (uiForm == null)
            {
                return null;
            }

            return (UGuiForm)uiForm.Logic;
        }

        public static void CloseUIForm(this UIComponent uiComponent, UGuiForm uiForm)
        {
            uiComponent.CloseUIForm(uiForm.UIForm);
        }

        public static int? OpenUIForm(this UIComponent uiComponent, Type logicType, UGuiFormData userData)
        {
            return OpenUIForm(uiComponent, logicType, userData.Id);
        }

        public static int? OpenUIForm(this UIComponent uiComponent, Type logicType, int uiFormId,  object userData = null)
        {
            IDataTable<DRUIForm> dtUIForm = Entry.DataTable.GetDataTable<DRUIForm>();
            DRUIForm drUIForm = dtUIForm.GetDataRow(uiFormId);
            if (drUIForm == null)
            {
                Log.Warning("Can not load UI form '{0}' from data table.", uiFormId.ToString());
                return null;
            }

            string assetName = AssetUtility.GetUIFormAsset(drUIForm.AssetName);
            if (!drUIForm.AllowMultiInstance)
            {
                if (uiComponent.IsLoadingUIForm(assetName))
                {
                    return null;
                }

                if (uiComponent.HasUIForm(assetName))
                {
                    return null;
                }
            }

            return uiComponent.OpenUIForm(logicType, assetName, drUIForm.UIGroupName, drUIForm.PauseCoveredUIForm, userData);
        }
    }
}