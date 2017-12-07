using System;
using Ash;
using Ash.DataTable;
using Framework;

public class DRCasehistory : IMySqlDataRow
{
    public DRCasehistory()
    {
        IsLocalDefault = false;
    }

    /// <summary>
    /// 病例数据
    /// </summary>
    /// <param name="id">表ID</param>
    /// <param name="name">病人姓名</param>
    /// <param name="age">病人年纪</param>
    /// <param name="skinColor">病人肤色</param>
    /// <param name="model">病人体型</param>
    /// <param name="information">病例信息</param>
    /// <param name="adice">医嘱</param>
    /// <param name="skinTest">皮试结果</param>
    /// <param name="note">提示信息</param>
    public DRCasehistory(int id,string name, int age,int skinColor, int model, string information, string adice,string skinTest, string note)
    {
        Id = id;
        Name = name;
        Age = age;
        SkinColor = skinColor;
        Model = model;
        Information = information;
        Advice = adice;
        SkinTest = skinTest;
        Note = note;
        IsLocalDefault = false;
    }


    public int Id { get; private set; }
    public string Name { get; private set; }
    public int Age { get; private set; }
    public int SkinColor { get; private set; }
    public int Model { get; private set; }
    public string Information { get; private set; }
    public string Advice { get; private set; }
    public string SkinTest { get; private set; }
    public string Note { get; private set; }
    public bool IsLocalDefault{ get; set; }


    public void ParseDataRow(string dataRowText)
    {
        string[] text = DataTableExtension.SplitDataRow(dataRowText);

        int index = 0;
        index++;
        Id = int.Parse(text[index++]);
        Name = text[index++];
        Age = int.Parse( text[index++]);
        SkinColor = int.Parse(text[index++]);
        Model = int.Parse(text[index++]);
        Information = text[index++];
        Advice = text[index++];
        SkinTest = text[index++];
        Note = text[index++];
        if (index == text.Length)
        {
            IsLocalDefault = false;
            return;
        }

        IsLocalDefault =  true;
    }

    public string GetMySqlInsertString()
    {
        return string.Format("insert into casehistory (name,age,skinColor,model,information,advice,skinTest,note) values('{0}',{1},{2},{3},'{4}','{5}','{6}','{7}');",
                Name, Age, SkinColor, Model, Information, Advice, SkinTest, Note);
    }

    public string GetMySqlUpdateString()
    {
        return string.Format("update casehistory set name='{1}',age={2},skinColor={3},model={4},information='{5}',advice='{6}',skinTest='{7}',note='{8}' where id={0};"
                , Id, Name, Age, SkinColor, Model, Information, Advice, SkinTest, Note);
    }

    public string GetMySqlDeleteString()
    {
        return string.Format("delete from casehistory where id={0};", Id);
    }
}
