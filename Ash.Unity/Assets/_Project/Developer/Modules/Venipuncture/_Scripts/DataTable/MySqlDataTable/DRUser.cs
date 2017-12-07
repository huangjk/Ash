using Ash;
using Ash.DataTable;
using Framework;

public class DRUser : IMySqlDataRow
{
    public enum UsetType
    {
        Teacher = 1,
        Student = 2,
    }

    public DRUser(){}

    public DRUser(int id, int studentID, string userName, string password, string classInfo, string specialty, string contact, UsetType type)
    {
        this.Id = id;
        this.StudentID = studentID;
        this.UserName = userName;
        this.Password = password;
        this.ClassInfo = classInfo;
        this.Specialty = specialty;
        this.Contact = contact;
        this.Type = type;
    }

    public int Id { get; private set; }
    public int StudentID { get; private set; }
    public string UserName { get; private set; }
    public string Password { get; private set; }
    public string ClassInfo { get; private set; }
    public string Specialty { get; private set; }
    public string Contact { get; private set; }
    public UsetType Type { get; private set; }

    public void ParseDataRow(string dataRowText)
    {
        string[] text = DataTableExtension.SplitDataRow(dataRowText);

        int index = 0;
        index++;
        StudentID = int.Parse(text[index++]);
        Id = int.Parse(text[index++]);
        UserName = text[index++];
        Password = text[index++];
        ClassInfo = text[index++];
        Specialty = text[index++];
        Contact = text[index++];
        int typeInt = int.Parse(text[index++]);
        Type = (UsetType)typeInt;
    }

    public string GetMySqlInsertString()
    {
        return string.Format("insert into user (studentID,userName,password,classInfo,specialty,contact,type) values({0},'{1}', '{2}','{3}','{4}','{5}',{6});",
                StudentID, UserName, Password, ClassInfo, Specialty, Contact, (int)Type);       
    }

    public string GetMySqlUpdateString()
    {
        return string.Format("update user set studentID={1},userName='{2}',password='{3}',classInfo='{4}',specialty='{5}',contact='{6}',type={7} where id={0};"
                , Id, StudentID, UserName, Password, ClassInfo, Specialty, Contact, (int)Type);
    }

    public string GetMySqlDeleteString()
    {
        return string.Format("delete from user where id={0};", Id);
    }
}
