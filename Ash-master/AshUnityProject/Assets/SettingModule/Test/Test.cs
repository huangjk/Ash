using AppSettings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {

    // Use this for initialization
    void Start()
    {
        AshTestSetting aaa = AshTestSettings.Get("1");
        Debug.Log("aaa: " + aaa.UserName);
        Debug.Log("aaa: " + aaa.Age);
    }
	
	// Update is called once per frame
	void Update ()
    {		
        if(Input.GetKeyDown(KeyCode.A))
        {
            AshTestSetting aaa = AshTestSettings.Get("1");
            Debug.Log("aaa: " + aaa.UserName);
            Debug.Log("aaa: " + aaa.Age);
        }
	}

    void OnGUI()
    {
        AshTestSetting aaa = AshTestSettings.Get("1");
        GUI.Label(new Rect(30, 30, 500, 500), aaa.UserName);
    }
}
