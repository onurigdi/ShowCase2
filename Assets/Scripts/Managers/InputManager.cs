using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System;

[DefaultExecutionOrder(700)]
public class InputManager : MonoBehaviour
{
    private IDisposable InputTapRX;
    bool allowInputs;



    private void OnEnable()
    {
        GameManager.instance.EventManager.levelLoaded += gameLoaded;
        GameManager.instance.EventManager.levelStarted += gameStarted;
        GameManager.instance.EventManager.levelSuccess += gameSuccees;
        GameManager.instance.EventManager.levelFailed += gameFailed;
    }

    private void OnDisable()
    {
        GameManager.instance.EventManager.levelLoaded -= gameLoaded;
        GameManager.instance.EventManager.levelStarted -= gameStarted;
        GameManager.instance.EventManager.levelSuccess -= gameSuccees;
        GameManager.instance.EventManager.levelFailed -= gameFailed;
    }



    private void Start()
    {
        InputTapRX = this.UpdateAsObservable()
         .TakeUntilDisable(this)
         .Where(l => Input.GetMouseButtonDown(0) && allowInputs)
         .Subscribe(l => handleInputTap());      
    }


    void handleInputTap()
    {
        GameManager.instance.EventManager.CallInputTap();
    }



    void gameLoaded()
    {
        allowInputs = false;
    }

    void gameStarted()
    {
        allowInputs = true;
    }

    void gameFailed()
    {
        allowInputs = false;
    }

    void gameSuccees()
    {
        allowInputs = false;
    }

}
