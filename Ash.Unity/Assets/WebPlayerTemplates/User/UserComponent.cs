using AshUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Ash.DataTable;
using Framework;

namespace Venipuncture.ManagementPlatform
{
    public class UserComponent : AshComponent
    {
        [SerializeField]
        private IUserManager m_UserManager;

        void Start()
        {
            m_UserManager = new UserManager();
        }

        // Update is called once per frame
        void Update()
        {
            if(Input.GetKeyDown(KeyCode.S))
            {
                m_UserManager.Save();
            }
        }

        void OnDestroy()
        {
            Shotdown();
        }

        public void Init()
        {
            m_UserManager.Init();


#if UNITY_EDITOR
            IDataTable<DRStudents> dtStudent = Entry.DataTable.GetDataTable<DRStudents>();
            DRStudents[] dtStudents = dtStudent.GetAllDataRows();

            foreach (DRStudents drs in dtStudents)
            {
                Student student = new Student(drs.Id, drs.UserName, drs.Password, drs.Class, drs.Major);
                student.Name = drs.Name;
                student.Contact = drs.Contact;
                int s = m_UserManager.AddUser<Student>(student);

                Ash.Log.Debug("{0} : {1} : {2}", s, drs.Name, drs.Id);
            }
#endif

        }

        public int AddUser<T>(T user) where T : UserBase
        {
            return m_UserManager.AddUser<T>(user);
        }

        public Student[] GetStudents()
        {
            return m_UserManager.GetStudents();
        }

        public Teacher[] GetTeachers()
        {
            return m_UserManager.GetTeachers();
        }

        public T GetUserById<T>(int id) where T : UserBase
        {
            return m_UserManager.GetUserById<T>(id);
        }

        public T GetUserByUsreName<T>(string usreName) where T : UserBase
        {
            return m_UserManager.GetUserByUsreName<T>(usreName);
        }

        public bool HasUserById(int id)
        {
            return m_UserManager.HasUserById(id);
        }

        public bool HasUserById<T>(int id) where T : UserBase
        {
            return m_UserManager.HasUserById(id);
        }

        public bool HasUserByUsreName(string usreName)
        {
            return m_UserManager.HasUserByUsreName(usreName);
        }

        public bool HasUserByUsreName<T>(string usreName) where T : UserBase
        {
            return m_UserManager.HasUserByUsreName<T>(usreName);
        }

        public void LoadStudents(string path)
        {
            m_UserManager.LoadStudents(path);
        }

        public void LoadTeachers(string path)
        {
            m_UserManager.LoadTeachers(path);
        }

        public void Save()
        {
            m_UserManager.Save();
        }

        public void Shotdown()
        {
            m_UserManager.Shotdown();
        }

        public UserBase GetUserById(int id)
        {
            return m_UserManager.GetUserById(id);
        }

        public UserBase GetUserByUsreName(string usreName)
        {
            return m_UserManager.GetUserByUsreName(usreName);
        }

        public bool RemoveUser(UserBase user)
        {
            return m_UserManager.RemoveUser(user);
        }

        public int UpdateData<T>(T oldData, T newData) where T : UserBase
        {
            return m_UserManager.UpdateData<T>(oldData, newData);
        }
    }
}
