using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptables/Debug", fileName = "DebugLoggerEvent")]
public class DebugLogEvent : ScriptableObject
{
    public void PrintStringLog(string value)
    {
        Debug.Log(value);
    }

    public void PrintStringLog()
    {
        Debug.Log("Hooray");
    }
}
