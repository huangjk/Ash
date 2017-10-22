using Ash;
using AshUnity;
using AshUnity.Config;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Test001 : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        AshApp.Init();
        EventComponent AshEvent =  AshApp.GetComponent<EventComponent>();

        bool isCall = false;

        AshEvent.On("event", (payload) =>
        {
            isCall = true;
            Debug.Log(payload.ToString());
            return 1;
        });

        var result = AshEvent.Trigger("event", 123) as object[];

        Debug.Log(isCall.ToString());
        Debug.Log(result[0].ToString());
        //var path = Application.streamingAssetsPath;
        //var path2 = Application.dataPath;
        //FileSystem steam = new FileSystem(path);
        //FileSystem steam2 = new FileSystem(path2);
        //Save
        //AshBaseConfig ashConfig = new AshBaseConfig();
        //AshBaseData data = new AshBaseData();
        //data.ID = 1;
        //data.ashRoot = "AAABBBCCC";
        //ashConfig.UpdateConfig(data);
        //ashConfig.SaveToDisk(steam);

        //AshBaseConfig ashConfig = AshBaseConfig.LoadConfig<AshBaseConfig>(steam);
        //ashConfig.SearchByID(1).ashRoot = "HHHJJJKKK";
        //ashConfig.SaveToDisk();

        //load1
        //AshBaseConfig ashConfig2 = AshBaseConfig.LoadConfig<AshBaseConfig>(steam);
        //Log.Info(ashConfig2.SearchByID(1).ashRoot);

        //load2
        //AshBaseConfig ashConfig2 = AshBaseConfig.LoadConfig<AshBaseConfig>(steam);
        ////Log.Info(ashConfig2.SearchByID(1).ashRoot);
        //ashConfig2.SearchByID(1).ashRoot = "HHHJJJKKK";
        //ashConfig2.SaveToDisk(steam2);

        //AshBaseConfig ashConfig1 = AshBaseConfig.LoadConfig<AshBaseConfig>(steam, "AAA/bbbb");
        //Log.Info(ashConfig1.SearchByID(1).ashRoot);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
