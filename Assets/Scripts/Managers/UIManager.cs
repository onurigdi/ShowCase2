using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(400)]
public class UIManager : MonoBehaviour
{
    public GameObject PnlMenu;
    public GameObject PnlGame;
    public GameObject PnlSuccees;
    public GameObject PnlFail;


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



    void showPanel(GameObject pnl)
    {
        PnlMenu.SetActive(false);
        PnlGame.SetActive(false);
        PnlSuccees.SetActive(false);
        PnlFail.SetActive(false);
        pnl.SetActive(true);
    }

    void gameLoaded()
    {
        showPanel(PnlMenu);
    }

    void gameStarted()
    {
        showPanel(PnlGame);
    }

    void gameFailed()
    {
        showPanel(PnlFail);
    }

    void gameSuccees()
    {
        showPanel(PnlSuccees);
    }

    public void BtnPlayClick()
    {
        GameManager.instance.EventManager.CallLevelStarted();
    }


    public void BtnContinueClick()
    {
        
        GameManager.instance.EventManager.CallLevelStarted();
    }

    public void BtnPlayAgain()
    {

        SceneManager.LoadScene("GameScene");// I didn't have enough time to reset all objects to use again without restart scene :) 
    }
}
