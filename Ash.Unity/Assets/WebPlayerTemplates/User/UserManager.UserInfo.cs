﻿using System.Collections; using System.Collections.Generic; using UnityEngine;  namespace Venipuncture.ManagementPlatform {     internal partial class UserManager     {         class UserInfo         {             public int id;             public string name;              public UserType userType;              public UserInfo(int id, string name, UserType userType)             {                 this.id = id;                 this.name = name;                 this.userType = userType;             }         }     } } 