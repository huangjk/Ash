namespace Venipuncture.ManagementPlatform
{
    internal interface IUserManager
    {
        int AddUser<T>(T user) where T : UserBase;
        Student[] GetStudents();
        Teacher[] GetTeachers();
        UserBase GetUserById(int id);
        T GetUserById<T>(int id) where T : UserBase;
        UserBase GetUserByUsreName(string usreName);
        T GetUserByUsreName<T>(string usreName) where T : UserBase;
        bool HasUserById(int id);
        bool HasUserById<T>(int id) where T : UserBase;
        bool HasUserByUsreName(string usreName);
        bool HasUserByUsreName<T>(string usreName) where T : UserBase;
        void Init();
        void LoadStudents(string path);
        void LoadTeachers(string path);
        bool RemoveUser(UserBase user);
        void Save();
        void Shotdown();
        int UpdateData<T>(T oldData, T newData) where T : UserBase;
    }
}