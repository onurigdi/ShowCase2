using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(200)]
public class EventMaster : MonoBehaviour
{
    public delegate void LevelLoaded();
    public event LevelLoaded levelLoaded;

    public delegate void LevelStarted();
    public event LevelStarted levelStarted;

    public delegate void LevelFailed();
    public event LevelFailed levelFailed;

    public delegate void LevelSuccess();
    public event LevelSuccess levelSuccess;

    public delegate void InputTap();
    public event InputTap inputTap;

    public void CallLevelLoaded()
    {
        if (levelLoaded != null)
        {
            levelLoaded();
        }
    }

    public void CallLevelStarted()
    {
        if (levelStarted != null)
        {
            levelStarted();
        }
    }

    public void CallLevelFailed()
    {
        if (levelFailed != null)
        {
            levelFailed();
        }
    }

    public void CallLevelSuccess()
    {
        if (levelSuccess != null)
        {
            levelSuccess();
        }
    }

    public void CallInputTap()
    {
        if (inputTap != null)
        {
            inputTap();
        }
    }
}
