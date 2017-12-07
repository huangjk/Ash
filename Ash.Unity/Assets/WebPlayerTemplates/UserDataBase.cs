using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class UserDataBase : DataModel
{
    public string UserName;

    public string Password;

    public string Contact;

    public UserDataBase()
    {
    }
}

