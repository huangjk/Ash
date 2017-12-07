using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Venipuncture.ManagementPlatform
{
    internal partial class UserManager
    {
        public class Loader
        {
            private ILoadAdapter m_LoadAdapter;

            public Loader(ILoadAdapter loadAdapter)
            {
                m_LoadAdapter = loadAdapter;
            }

            public List<Student> LoadStudentData(string path)
            {
                List<Student> studentData = m_LoadAdapter.Load<List<Student>>(path);
                return studentData;
            }

            //public List<Teacher> LoadTeacherData(string path)
            //{
            //    TeacherData teacherData = m_LoadAdapter.Load<TeacherData>(path);
            //    return teacherData.m_Teachers;
            //}

            //internal void SaveStudentData(StudentData studentData,string path)
            //{
            //    m_LoadAdapter.Save<StudentData>(studentData, path);
            //}

            //internal void SaveTeacherData(TeacherData teacherData, string path)
            //{
            //    m_LoadAdapter.Save<TeacherData>(teacherData, path);
            //}
        }
    }
}
