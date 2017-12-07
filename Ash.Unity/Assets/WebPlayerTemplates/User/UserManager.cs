using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Venipuncture.ManagementPlatform
{
    internal partial class UserManager : IUserManager
    {
        //[SerializeField]
        //private List<UserBase> m_UsersList;
        private List<Student> m_Students;
        private List<Teacher> m_Teachers;

        [SerializeField]
        private Dictionary<UserInfo, UserBase> m_Users { get; set; }

        private Loader loader;

        private string m_StrudentDataPath ;
        private string m_TeacherDataPath ;

        public UserManager()
        {
            //m_UsersList = new List<UserBase>();
            m_Users = new Dictionary<UserInfo, UserBase>();

            loader = new Loader(new LoadUserAdapter());

            m_StrudentDataPath = Application.streamingAssetsPath + "/Data/StrudentData";
            m_TeacherDataPath = Application.streamingAssetsPath + "/Data/TeacherData";
        }

        public void Init()
        {
            //读取历史数据
            LoadStudents(m_StrudentDataPath);
            LoadTeachers(m_TeacherDataPath);
        }

        public void Shotdown()
        {
            m_Users.Clear();
        }


        public void LoadStudents(string path)
        {
            if (!File.Exists(path)) return;

            List<Student> students = loader.LoadStudentData(path);

            for (int i = 0; i < students.Count; i++)
            {
                AddUser<Student>(students[i]);
            }
        }

        public void LoadTeachers(string path)
        {
            if (!File.Exists(path)) return;

            List<Teacher> teachers = loader.LoadTeacherData(path);

            for (int i = 0; i < teachers.Count; i++)
            {
                AddUser<Teacher>(teachers[i]);
            }
        }

        public void Save()
        {
            //Student[] students = GetStudents();
            //Teacher[] teachers = GetTeachers();

            //StudentData studentData = new StudentData();
            //studentData.m_Students = students.ToList();

            //TeacherData teacherData = new TeacherData();
            //teacherData.m_Teachers = teachers.ToList();

            //Ash.Log.Debug("AAAAA: {0}", studentData.m_Students.Count);
            

            //loader.SaveStudentData(studentData, m_StrudentDataPath);
            //loader.SaveTeacherData(teacherData, m_TeacherDataPath);
        }

        public bool HasUserById(int id)
        {
            foreach (KeyValuePair<UserInfo, UserBase> user in m_Users)
            {
                if (user.Key.id == id)
                {
                    return true;
                }
            }

            return false;
        }

        public bool HasUserById<T>(int id) where T : UserBase
        {
            foreach (KeyValuePair<UserInfo, UserBase> user in m_Users)
            {
                if (user.Key.id == id)
                {
                    if(user.Key.userType == user.Value.UserType)
                    {
                        return true;
                    }
                    return false;
                }
            }

            return false;
        }

        public bool HasUserByUsreName(string usreName)
        {
            if (string.IsNullOrEmpty(usreName))
            {
                Ash.Log.Warning("Student usreName is invalid.");
            }

            foreach (KeyValuePair<UserInfo, UserBase> user in m_Users)
            {
                if (user.Key.name == usreName)
                {
                    return true;
                }
            }

            return false;
        }

        public bool HasUserByUsreName<T>(string usreName) where T : UserBase
        {
            if (string.IsNullOrEmpty(usreName))
            {
                Ash.Log.Warning("Student usreName is invalid.");
            }

            foreach (KeyValuePair<UserInfo, UserBase> user in m_Users)
            {
                if (user.Key.name == usreName)
                {
                    if (user.Key.userType == user.Value.UserType)
                    {
                        return true;
                    }
                    return false;
                }
            }

            return false;
        }

        public UserBase GetUserById(int id)
        {
            if (!HasUserById(id))
            {
                Ash.Log.Warning("用户{0}不存在", id);
                return null;
            }

            foreach (KeyValuePair<UserInfo, UserBase> user in m_Users)
            {
                if (user.Key.id == id)
                {
                    return user.Value;
                }
            }

            return null;
        }

        public T GetUserById<T>(int id) where T : UserBase
        {
            if(!HasUserById<T>(id))
            {
                Ash.Log.Warning("用户{0}不存在", id);
                return null;
            }

            foreach (KeyValuePair<UserInfo, UserBase> user in m_Users)
            {           
                if (user.Key.id == id)
                {
                    T t = user.Value as T;

                    if (t.UserType == user.Key.userType)
                    {
                        return t;
                    }
                    else
                    {
                        Ash.Log.Warning("类型不符");
                        return null;
                    }
                }


            }

            return null;
        }

        public UserBase GetUserByUsreName(string usreName)
        {
            if (string.IsNullOrEmpty(usreName))
            {
                Ash.Log.Warning("usreName usreName is invalid.");
            }

            if (!HasUserByUsreName(usreName))
            {
                Ash.Log.Warning("用户{0}不存在", usreName);
                return null;
            }

            foreach (KeyValuePair<UserInfo, UserBase> user in m_Users)
            {
                if (user.Key.name == usreName)
                {
                    return user.Value;
                }
            }

            return null;
        }

        public T GetUserByUsreName<T>(string usreName) where T : UserBase
        {
            if (string.IsNullOrEmpty(usreName))
            {
                Ash.Log.Warning("usreName usreName is invalid.");
            }

            if (!HasUserByUsreName<T>(usreName))
            {
                Ash.Log.Warning("用户{0}不存在", usreName);
                return null;
            }

            foreach (KeyValuePair<UserInfo, UserBase> user in m_Users)
            {
                if (user.Key.name == usreName)
                {
                    T t = user.Value as T;

                    if(t.UserType == user.Key.userType)
                    {
                        return t;
                    }else
                    {
                        Ash.Log.Warning("类型不符");
                        return null;
                    }

                }
            }

            return null;
        }

        public Student[] GetStudents()
        {
            List<Student> m_Students = new List<Student>();
            foreach (KeyValuePair<UserInfo, UserBase> user in m_Users)
            {
                if (user.Value.UserType == UserType.Student)
                {
                    m_Students.Add(user.Value as Student);
                }
            }

            return m_Students.ToArray();
        }

        public Teacher[] GetTeachers()
        {
            List<Teacher> m_Teachers = new List<Teacher>();
            foreach (KeyValuePair<UserInfo, UserBase> user in m_Users)
            {
                if (user.Value.UserType == UserType.Teacher)
                {
                    m_Teachers.Add(user.Value as Teacher);
                }
            }

            return m_Teachers.ToArray();
        }

        /// <summary>
        /// 增加用户
        /// </summary>
        /// <typeparam name="T">用户的类型</typeparam>
        /// <param name="user">用户</param>
        /// <returns>返回 增加结果，0 = 用户信息非法，1 = 成功， 2 = 用户ID已存在， 3 = 用户名已存在</returns>
        public int AddUser<T>(T user) where T : UserBase
        {
            if(string.IsNullOrEmpty(user.Name))
            {
                Ash.Log.Warning("用户信息非法");
                return 0;
            }

            UserInfo newUserInfo = new UserInfo(user.Id, user.UserName, user.UserType);

            foreach (UserInfo userInfo in m_Users.Keys)
            {
                if(newUserInfo.id == userInfo.id)
                {
                    Ash.Log.Warning("用户ID已经存在 ");
                    return 2;
                }
                else if (newUserInfo.name == userInfo.name)
                {
                    Ash.Log.Warning("用户名已经存在 ");
                    return 3;
                }
            }
            user.SetUserManager(this);
            m_Users.Add(newUserInfo, user);
            return 1;
        }

        public bool RemoveUser(UserBase user)
        {
            if (string.IsNullOrEmpty(user.Name))
            {
                Ash.Log.Warning("用户信息非法");
                return false;
            }

            foreach (UserInfo userInfo in m_Users.Keys)
            {
                if (user.Id == userInfo.id)
                {
                    return m_Users.Remove(userInfo);
                }
            }

            return false;
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <typeparam name="T">用户的类型</typeparam>
        /// <param name="oldData">旧用户数据</param>
        /// <param name="newData">新用户数据</param>
        /// <returns>返回 更新结果，0 = 删除旧的失败，1 = 成功， 2 = 用户ID已存在， 3 = 用户名已存在</returns>
        public int UpdateData<T>(T oldData,T newData) where T : UserBase
        {
            //判断新的数据ID是否已经存在
            if (newData.Id != oldData.Id && HasUserById<T>(newData.Id))
            {
                return 2;
            }
            //判断新的数据用户名是否已经存在
            if (newData.UserName != oldData.UserName && HasUserByUsreName<T>(newData.UserName))
            {
                return 3;
            }

            if (RemoveUser(oldData))
            {
                return AddUser(newData);
            }

            return 0;
        }
    }
}
