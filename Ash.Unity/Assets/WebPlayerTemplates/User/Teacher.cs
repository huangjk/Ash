using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Venipuncture.ManagementPlatform
{
    public class Teacher : UserBase
    {
        public Teacher(int id, string userName, string password) : base(UserType.Teacher, id, userName, password)
        {
        }
    }
}
