using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Data;
using System.Text.RegularExpressions;

public class JsonTest : MonoBehaviour
{
// Use this for initialization
void Start () {
        Product product = new Product();
        product.listT.Add("1222");
        product.listT.Add("士大夫十分");
        product.listT.Add("的撒法");
        product.listT.Add("asdf");
        string output = JsonUtility.ToJson(product);

        Deserializejson dj = new Deserializejson();
        DataTable dt = JsonToDataTable(output);

        //foreach()
        //JsonReader reader = new JsonReader(new StringReader(jsonText));

        //dt.Rows
        //读取数据表行数和列数
        int rowCount = dt.Rows.Count;
        int colCount = dt.Columns.Count;
        Debug.Log(output);
        //读取数据
        for (int i = 1; i < rowCount; i++)
        {
            //准备一个字典存储每一行的数据
            for (int j = 0; j < colCount; j++)
            {
                //读取第1行数据作为表头字段
                string field = dt.Rows[0][j].ToString();

                Debug.Log(field);
            }
        }


        //while (reader.Read())
        //{
        //    Console.WriteLine(reader.TokenType + "\t\t" + WriteValue(reader.ValueType) + "\t\t" + WriteValue(reader.Value))
        //}
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    [Serializable]
    public class Product
    {
        public Product()
        {
            listT = new List<string>();
            id = 0;
            Expiry = DateTime.Now;
            price = 3.999f;
            c3 = Color.red;
            v3 =new Vector3(1.0f, 3.5335f, 22.0f);
            q = Quaternion.identity;

            listaaa = new List<cccc>();

            listaaa.Add(new cccc());
            listaaa.Add(new cccc());
            listaaa.Add(new cccc());
        }


        public int id;
        public DateTime Expiry;
        public float price;
        public Color c3;
        public Vector3 v3;
        public Quaternion q;
        public List<string> listT;
        public List<cccc> listaaa;
    }

    [Serializable]
    public class cccc
    {
        public cccc()
        {
            sdfds = "吊死扶伤";
            dd = new Dictionary<string, string>();

            dd.Add("的说法是的","sdfsdf");
            dd.Add("112312", "aaaaaaaaaa");
        }

        public string sdfds;
        public Dictionary<string, string> dd;
    }

        /// <summary>   
        /// 根据Json返回DateTable,JSON数据格式如:   
        /// {table:[{column1:1,column2:2,column3:3},{column1:1,column2:2,column3:3}]}   
        /// items:{"2750884":{clicknum:"50",title:"鲍鱼",href:"/shop/E06B14B40110/dish/2750884#menu",desc:"<br/>",src:"15f38721-49da-48f0-a283-8057c621b472.jpg",price:78.00,units:"",list:[],joiner:""}}
        /// </summary>   
        /// <param name="strJson">Json字符串</param>   
        /// <returns></returns>   
        public static DataTable JsonToDataTable(string strJson)
    {
        //取出表名   
        //var rg = new Regex(@"(?<={)[^:]+(?=:\[)", RegexOptions.IgnoreCase);
        var rg = new Regex(@"([^:])+(?=:\{)", RegexOptions.IgnoreCase);
        string strName = rg.Match(strJson).Value;
        DataTable tb = null;
        //去除表名   
        //strJson = strJson.Substring(strJson.IndexOf("{") + 1);
        //strJson = strJson.Substring(0, strJson.IndexOf("}"));

        //获取数据   
        //rg = new Regex(@"(?<={)[^}]+(?=})");
        rg = new Regex(@"(?<={)[^}]+(?=})");

        System.Text.RegularExpressions.MatchCollection mc = rg.Matches(strJson);
        for (int i = 0; i < mc.Count; i++)
        {
            string strRow = mc[i].Value;
            string[] strRows = strRow.Split(',');

            //创建表   
            if (tb == null)
            {
                tb = new DataTable();
                tb.TableName = strName;
                foreach (string str in strRows)
                {
                    var dc = new DataColumn();
                    string[] strCell = str.Split(':');
                    dc.ColumnName = strCell[0];
                    tb.Columns.Add(dc);
                }
                tb.AcceptChanges();
            }

            //增加内容   
            DataRow dr = tb.NewRow();
            for (int r = 0; r < strRows.Length; r++)
            {
                //dr[r] = strRows[r].Split(':')[1].Trim().Replace("，", ",").Replace("：", ":").Replace("\"", "");
                dr[r] = strRows[r];
            }
            tb.Rows.Add(dr);
            tb.AcceptChanges();
        }

        return tb;
    }
}






{"id":0,"price":3.999000072479248,"c3":{"r":1.0,"g":0.0,"b":0.0,"a":1.0},"v3":{"x":1.0,"y":3.5334999561309816,"z":22.0},"q":{"x":0.0,"y":0.0,"z":0.0,"w":1.0},"listT":["1222","士大夫十分","的撒法","asdf"],"listaaa":[{"sdfds":"吊死扶伤"},{"sdfds":"吊死扶伤"},{"sdfds":"吊死扶伤"}]}
