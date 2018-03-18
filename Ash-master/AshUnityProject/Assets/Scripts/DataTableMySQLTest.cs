using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DataTableMySQLTest : MonoBehaviour {

    public Text text;

	// Use this for initialization
	void Start ()
    {

    }
    Ash.DTMySQL_Test test;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            text.text = Ash.DTMySQL_MyTest_Manager.Instance.LoadById(1).Info;
            Debug.Log(Ash.DTMySQL_MyTest_Manager.Instance.LoadById(1).Info );
            //Debug.Log("MaxID: " + Ash.DTMySQL_Test_Manager.MySQL_GetMaxID());
            //test = new Ash.DTMySQL_Test();
            //Debug.Log("ID: " + test.Id);
            //test.CloneButId(Ash.DTMySQL_Test_Manager.Instance.LoadById(1));
            //test.Value1 = 2.22f;
            //test.Value = "bbbb";
            //test.UpdateToMySQL();
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            //Ash.DTMySQL_Test_Manager.Instance.LoadByValue("aa");
            //Ash.DTMySQL_Test_Manager.Instance.LoadByValue("bb");
            //Ash.DTMySQL_Test_Manager.Instance.LoadByValue("cc");

            //Debug.Log(Ash.DTMySQL_Test_Manager.LoadAllByLimit(5, 5).Count);
            //Debug.Log(Ash.DTMySQL_Test_Manager.Instance.Count + " : " + Ash.DTMySQL_Test_Manager.MySQL_GetRowsCount());
        }
    }
}
