using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Venipuncture.ManagementPlatform
{
    public class UserClass
    {
        private string m_Name;

        private List<Student> m_Students;

        public string Name
        {
            get
            {
                return m_Name;
            }

            set
            {
                m_Name = value;
            }
        }

        public UserClass(string name, Teacher teacher)
        {
            if (string.IsNullOrEmpty(name))
            {
                Ash.Log.Error("UserClass name is invalid.");
            }

            if(teacher == null)
            {
                Ash.Log.Error("UserClass teacher is invalid.");
            }

            Name = name;

            m_Students = new List<Student>();
        }

        bool HasStudentById(int id)
        {
            foreach (Student student in m_Students)
            {
                if (student.Id == id)
                {
                    return true;
                }
            }

            return false;
        }

        bool HasStudentByUsreName(string usreName)
        {
            if (string.IsNullOrEmpty(usreName))
            {
                Ash.Log.Warning("Student usreName is invalid.");
            }

            foreach (Student student in m_Students)
            {
                if (student.UserName == usreName)
                {
                    return true;
                }
            }

            return false;
        }

        Student GetStudentById(int id)
        {
            foreach (Student student in m_Students)
            {
                if (student.Id == id)
                {
                    return student;
                }
            }

            return null;
        }

        Student GetStudentByUsreName(string usreName)
        {
            if (string.IsNullOrEmpty(usreName))
            {
                Ash.Log.Warning("Student usreName is invalid.");
            }

            foreach (Student student in m_Students)
            {
                if (student.UserName == usreName)
                {
                    return student;
                }
            }

            return null;
        }

        Student[] Getm_Students()
        {
            List<Student> m_Students = new List<Student>();
            foreach (Student student in m_Students)
            {
                m_Students.Add(student);
            }

            return m_Students.ToArray();
        }

        public void AddStudents(Student[] students)
        {
            int lengh = students.Length;

            for (int i = 0; i < lengh; i++)
            {
                AddStudent(students[i]);
            }
        }

        public void AddStudent(Student student)
        {
            if (m_Students.Contains(student))
            {
                Ash.Log.Warning("Student exsit.");
                return;
            }

            m_Students.Add(student);
        }

        public bool RemoveStudent(Student student)
        {
            return m_Students.Remove(student);
        }
    }
}