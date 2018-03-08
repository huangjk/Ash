using Ash.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        AshUnityEntry.New();
        Debug.Log(AshUnityEntry.Instance.Version);

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
