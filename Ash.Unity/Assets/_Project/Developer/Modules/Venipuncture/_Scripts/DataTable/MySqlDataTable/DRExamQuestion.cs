using System;
using Ash;
using Ash.DataTable;
using Framework;
using System.Collections.Generic;

public class DRExamQuestion : IMySqlDataRow
{
    public DRExamQuestion()
    {
        IsLocalDefault = false;
    }

    /// <summary>
    /// 选择题
    /// </summary>
    /// <param name="id">ID</param>
    /// <param name="name">考题内容</param>
    /// <param name="item1">考题选项1</param>
    /// <param name="item2">考题选项2</param>
    /// <param name="item3">考题选项3</param>
    /// <param name="item4">考题选项4</param>
    /// <param name="rightAnswer">考题答案</param>
    /// <param name="theoreticalPaper">所属试卷</param>
    public DRExamQuestion(int id,string name,string item1, string item2, string item3, string item4,int[] rightAnswer, int[] theoreticalPaper)
    {
        Id = id;
        Name = name;
        Item1 = item1;
        Item2 = item2;
        Item3 = item3;
        Item4 = item4;
        RightAnswer = rightAnswer;
        TheoreticalPaper = theoreticalPaper;
        IsLocalDefault = false;
    }

    public int Id { get; private set; }
    public string Name { get; private set; }
    public string Item1 { get; private set; }
    public string Item2 { get; private set; }
    public string Item3 { get; private set; }
    public string Item4 { get; private set; }
    public int[] RightAnswer { get; private set; }
    public int[] TheoreticalPaper { get; private set; }
    public bool IsLocalDefault { get; private set; }

    public void ParseDataRow(string dataRowText)
    {
        string[] text = DataTableExtension.SplitDataRow(dataRowText);

        int index = 0;
        index++;
        Id = int.Parse(text[index++]);
        Name = text[index++];
        Item1 = text[index++];
        Item2 = text[index++];
        Item3 = text[index++];
        Item4 = text[index++];

        string rightAnswerAllStr = text[index++];
        string[] rightAnswerStrs = rightAnswerAllStr.Split(',');
        List<int> rightAnswers = new List<int>();        
        foreach(string s in rightAnswerStrs)
        {
            rightAnswers.Add(int.Parse(s));
        }
        RightAnswer = rightAnswers.ToArray();

        string theoreticalPaperAllStr = text[index++];
        string[] theoreticalPaperStrs = theoreticalPaperAllStr.Split(',');
        List<int> theoreticalPapers = new List<int>();
        foreach (string s in theoreticalPaperStrs)
        {
            theoreticalPapers.Add(int.Parse(s));
        }
        TheoreticalPaper = theoreticalPapers.ToArray();

        if (index == text.Length)
        {
            IsLocalDefault = false;
            return;
        }
        IsLocalDefault = true;
    }


    public string GetMySqlInsertString()
    {
        return string.Format("insert into examquestion (name,item1,item2,item3,item4,rightAnswer,TheoreticalPaper) values('{0}','{1}','{2}','{3}','{4}','{5}','{6}');",
                Name, Item1, Item2, Item3, Item4, GetStr(RightAnswer), GetStr(TheoreticalPaper));
    }

    public string GetMySqlUpdateString()
    {
        return string.Format("update examquestion set name='{1}',item1='{2}',item2='{3}',item3='{4}',item4='{5}',rightAnswer='{6}',TheoreticalPaper='{7}' where id={0};"
                , Id, Name, Item1, Item2, Item3, Item4, GetStr(RightAnswer), GetStr(TheoreticalPaper));
    }

    public string GetMySqlDeleteString()
    {
        return string.Format("delete from examquestion where id={0};", Id);
    }

    private string GetStr(int[] iniDatas)
    {
        string str = "";
        int length = iniDatas.Length;
        for (int i = 0; i < length; i++)
        {
            if (i == length - 1)
            {
                str += i;
            }
            else
            {
                str += i + ",";
            }
        }
        return str;
    }
}
