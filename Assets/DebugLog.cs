using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using TMPro;

public class DebugLog : MonoBehaviour
{
    public TextMeshPro debugBox;
    public InputReader inputs;

    uint qsize = 10;  // number of messages to keep
    Queue myLogQueue = new Queue();

    void Start() {
        Debug.Log("Started up logging.");
    }

    void OnEnable() {
        Application.logMessageReceived += HandleLog;
    }

    void OnDisable() {
        Application.logMessageReceived -= HandleLog;
    }

    void HandleLog(string logString, string stackTrace, LogType type) {
        myLogQueue.Enqueue("[" + type + "] : " + logString);
        if (type == LogType.Exception)
            myLogQueue.Enqueue(stackTrace);
        while (myLogQueue.Count > qsize)
            myLogQueue.Dequeue();
    }

    void OnGUI() {
        if (inputs.ButtonBDown)
        {
            myLogQueue.Clear();
        }
        debugBox.SetText("\n" + "Press B to clear" + "\n" + string.Join("\n", myLogQueue.ToArray()));
    }
}
