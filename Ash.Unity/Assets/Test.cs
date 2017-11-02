using Ash;
using AshUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        AshApp.GetComponent<DebuggerComponent>();

        Log.Info("AAAAAAAAAAA");
        Log.Debug("BBBBBBBBBB");
        Log.Warning("CCCCCCCCCCC");
        Log.Error("DDDDDDDDDDD");
        Log.Error("EEEEEEEEEEEEE");
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
