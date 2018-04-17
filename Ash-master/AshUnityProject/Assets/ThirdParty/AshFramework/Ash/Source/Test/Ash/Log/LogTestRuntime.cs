using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using Ash;
using Ash.Runtime;

public class LogTestRuntime {

    [Test]
    public void LogTestSimplePasses()
    {
        AshUnityEntry.New();

        // Use the Assert class to test conditions.
        Log.Debug("DeBug");
        Log.Info("Info");
        Log.Warning("Warning");
        Log.Error("Error");
        Log.Fatal("Fatal");
    }

    [Test]
    public void LogToFileTest()
    {
        AshUnityEntry.New();

        Log.SetIsLogToFile(true);
        // Use the Assert class to test conditions.
        Log.Debug("LogToFileDeBug true");
        Log.Info("LogToFileInfo true");
        Log.Warning("LogToFileWarning true");
        //Log.Error("LogToFileError true");
        //Log.Fatal("LogToFileFatal true");

        Log.SetIsLogToFile(false);
        Log.Debug("LogToFileDebug  false");
        Log.Info("LogToFileInfo false");
        Log.Warning("LogToFileWarning false");
        //Log.Error("LogToFileError false");
        //Log.Fatal("LogToFileFatal false");
    }

    [Test]
    public void LogLevelTest()
    {
        AshUnityEntry.New();

        Log.Debug("开始输出 LogLevel.Debug");
        Log.SetLogLevel(LogLevel.Debug);
        Log.Debug("DeBug");
        Log.Info("Info");
        Log.Warning("Warning");
        Log.Error("Error");
        Log.Fatal("Fatal");

        Log.Debug("开始输出 LogLevel.Info");
        Log.SetLogLevel(LogLevel.Info);
        Log.Debug("DeBug");
        Log.Info("Info");
        Log.Warning("Warning");
        Log.Error("Error");
        Log.Fatal("Fatal");

        Log.Fatal("开始输出 LogLevel.Warning");
        Log.SetLogLevel(LogLevel.Warning);
        Log.Debug("DeBug");
        Log.Info("Info");
        Log.Warning("Warning");
        Log.Error("Error");
        Log.Fatal("Fatal");

        Log.Fatal("开始输出 LogLevel.Error");
        Log.SetLogLevel(LogLevel.Error);
        Log.Debug("DeBug");
        Log.Info("Info");
        Log.Warning("Warning");
        Log.Error("Error");
        Log.Fatal("Fatal");

        Log.Fatal("开始输出 LogLevel.Fatal");
        Log.SetLogLevel(LogLevel.Fatal);
        Log.Debug("DeBug");
        Log.Info("Info");
        Log.Warning("Warning");
        Log.Error("Error");
        Log.Fatal("Fatal");
    }

    // A UnityTest behaves like a coroutine in PlayMode
    // and allows you to yield null to skip a frame in EditMode
    [UnityTest]
	public IEnumerator LogTestRuntimeWithEnumeratorPasses() {
		// Use the Assert class to test conditions.
		// yield to skip a frame
		yield return null;
	}
}
