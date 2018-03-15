using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {

    }
	
	// Update is called once per frame
	void Update ()
    {
		if(Input.GetKeyDown(KeyCode.A))
        {
            string commandText = "CREATE TABLE `TestTest` (char_id INT NOT NULL PRIMARY KEY AUTO_INCREMENT, PlayerName VARCHAR(20));";

            DatabaseManager.GetInstance().OpenDatabase();
            DatabaseManager.GetInstance().DoSQLUpdateDelete(commandText);
        }
	}
}
