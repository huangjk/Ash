using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Venipuncture.ManagementPlatform
{
    [Serializable]
    public class Student : UserBase
    {
        private string m_Class;
        private string m_Major;

        public Student(int id, string userName, string password, string classInfo,string major) : base(UserType.Student, id, userName, password)
        {
            m_Class = classInfo;

            m_Major = major;
        }

        public string Class
        {
            get
            {
                return m_Class;
            }
            set
            {
                m_Class = value;
            }
        }

        public string Major
        {
            get
            {
                return m_Major;
            }
            set
            {
                m_Major = value;
            }
        }
    }
}
