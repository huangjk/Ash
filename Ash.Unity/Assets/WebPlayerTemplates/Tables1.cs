using System;
using Ash.DataTable;
using Framework;
using Ash;

namespace ManagementPlatform
{

    public class Casehistory : IDataRow
    {
        //private int id { get; set; }
        public string name { get; set; }
        public int age { get; set; }
        public int skinColor { get; set; }
        public int model { get; set; }
        public string information { get; set; }
        public string advice { get; set; }
        public string skinTest { get; set; }
        public string note { get; set; }

        public int Id
        {
            get;
            private set;
        }

        public void ParseDataRow(string dataRowText)
        {
            
        }
    }


    public class ExamQuestion
    {
        public int id { get; set; }
        public string name { get; set; }
        public string item1 { get; set; }
        public string item2 { get; set; }
        public string item3 { get; set; }
        public string item4 { get; set; }
        public string rightAnswer { get; set; }
        public string TheoreticalPaper { get; set; }
    }

    public class PracticalScore
    {
        public int id { get; set; }
        public int userId { get; set; }
        public string operationTime { get; set; }
        public float operationScore { get; set; }
        public float orderAccuracy { get; set; }
        public float evaluateScore { get; set; }
        public float finishScore { get; set; }
        public string impressionEvaluate { get; set; }
        public string checkAdvice { get; set; }
        public float checkAdviceScore { get; set; }
        public string checkPatient { get; set; }
        public float checkPatientScore { get; set; }
        public string talk { get; set; }
        public float talkScore { get; set; }
        public string talkFileName { get; set; }
        public string partSelcet { get; set; }
        public float partSelcetScore { get; set; }
        public string wash { get; set; }
        public float washScore { get; set; }
        public string wearMask { get; set; }
        public float wearMaskScore { get; set; }
        public string confectSolution { get; set; }
        public float confectSolutionScore { get; set; }
        public string closingRegulator { get; set; }
        public float closingRegulatorScore { get; set; }
        public string checkAgain { get; set; }
        public float checkAgainScore { get; set; }
        public string fiveItem { get; set; }
        public float fiveItemScore { get; set; }
        public string disinfect1 { get; set; }
        public float disinfect1Score { get; set; }
        public string tourniquet { get; set; }
        public float tourniquetScore { get; set; }
        public string disinfect2 { get; set; }
        public float disinfect2Score { get; set; }
        public string scavenging2 { get; set; }
        public float scavenging2Score { get; set; }
        public string puncture { get; set; }
        public float punctureScore { get; set; }
        public string fixation { get; set; }
        public float fixationScore { get; set; }
        public string loosenTourniquet { get; set; }
        public float loosenTourniquetScore { get; set; }
        public string openingRegulator { get; set; }
        public float openingRegulatorScore { get; set; }
        public string infusionStick { get; set; }
        public float infusionStickScore { get; set; }
        public string drippingSpeed { get; set; }
        public float drippingSpeedScore { get; set; }
    }
    public class PracticalScoreSetting
    {
        public int id { get; set; }
        public int userId { get; set; }
        public string checkAdvice { get; set; }
        public string checkPatient { get; set; }
        public string talk { get; set; }
        public string partSelcet { get; set; }
        public string wash { get; set; }
        public string wearMask { get; set; }
        public string confectSolution { get; set; }
        public string closingRegulator { get; set; }
        public string checkAgain { get; set; }
        public string fiveItem { get; set; }
        public string disinfect1 { get; set; }
        public string tourniquet { get; set; }
        public string disinfect2 { get; set; }
        public string scavenging2 { get; set; }
        public string puncture { get; set; }
        public string fixation { get; set; }
        public string loosenTourniquet { get; set; }
        public string openingRegulator { get; set; }
        public string infusionStick { get; set; }
        public string drippingSpeed { get; set; }
    }

    public class PracticalTask
    {
        public int id { get; set; }
        public string name { get; set; }
        public string content { get; set; }
        public string time { get; set; }
        public int caseHistoryId { get; set; }
    }

    public class TheoreticalPaper
    {
        public int id { get; set; }
        public string name { get; set; }
    }

    public class TheoreticalScore
    {
        public int id { get; set; }
        public string time { get; set; }
        public int userId { get; set; }
        public int theoreticalPaperId { get; set; }
        public string studentAnswer { get; set; }
    }

    public class TheoreticalTask
    {
        public int id { get; set; }
        public string name { get; set; }
        public string content { get; set; }
        public string time { get; set; }
        public string theoreticalPaperList { get; set; }
        public string userList { get; set; }
    }

    public enum UsetType
    {
        Teacher = 0,
        Student = 1,
    }

    //public class DRUser : IDataRow
    //{
    //    //public int id { get; set; }
    //    public string userName { get; set; }
    //    public string password { get; set; }
    //    public string classInfo { get; set; }
    //    public string specialty { get; set; }
    //    public string contact { get; set; }
    //    public UsetType type { get; set; }

    //    public int Id
    //    {
    //        get;
    //        private set;
    //    }

    //    public void ParseDataRow(string dataRowText)
    //    {
    //        string[] text = DataTableExtension.SplitDataRow(dataRowText);

    //        foreach(string s in text)
    //        {
    //            Log.Info(text);
    //        }

    //        int index = 0;
    //        index++;
    //        Id = int.Parse(text[index++]);
    //        userName = text[index++];
    //        password = text[index++];
    //        classInfo = text[index++];
    //        specialty = text[index++];
    //        contact = text[index++];
    //        int typeInt = int.Parse(text[index++]);
    //        type = (UsetType)typeInt;
    //    }
    //}

}