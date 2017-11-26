using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class UGuiFormData
{
    [SerializeField]
    private int m_Id = 0;

    /// <summary>
    /// UI编号。
    /// </summary>
    public int Id
    {
        get
        {
            return m_Id;
        }
    }

    public UGuiFormData(int uiId)
    {
        m_Id = uiId;
    }

}
