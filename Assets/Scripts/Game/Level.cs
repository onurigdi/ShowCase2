using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(300)]
public class Level : MonoBehaviour
{
    public RunnerController Runner;
    public Stack LastPlacedStack;
    public Stack PrevPlacedStack;
    public FinishLine LastFinishLine;
    public int LevelMaxStackCount;
    int levelStackCount = 1;
    int perfectScoreCounter;
    public int succeedLevelCount;

    


    private void OnEnable()
    {
        GameManager.instance.EventManager.levelLoaded += gameLoaded;
        GameManager.instance.EventManager.levelStarted += gameStarted;
        GameManager.instance.EventManager.inputTap += inputTapped;
        GameManager.instance.EventManager.levelSuccess += gameSuccees;
        GameManager.instance.EventManager.levelFailed += gameFailed;
    }
    
    private void OnDisable()
    {
        GameManager.instance.EventManager.levelLoaded -= gameLoaded;
        GameManager.instance.EventManager.levelStarted -= gameStarted;
        GameManager.instance.EventManager.inputTap -= inputTapped;
        GameManager.instance.EventManager.levelSuccess -= gameSuccees;
        GameManager.instance.EventManager.levelFailed -= gameFailed;
    }

    private void Start()
    {
        GameManager.instance.EventManager.CallLevelLoaded();
    }


    void gameLoaded()
    {
        Runner.SetAnimation(RunnerController.Anims.Idle);
        randomGenerateNewLevel();
        InitFirstStackAfterSuccess();
    }

    void gameStarted()
    {
        LastPlacedStack?.StopPingPong();
        PrevPlacedStack = LastPlacedStack;        
        playerMoveToNextStack();
        callNextStack();
    }

    void gameFailed()
    {
        
    }

    void gameSuccees()
    {
        succeedLevelCount++;
        randomGenerateNewLevel();
        GameManager.instance.ActiveLevel.InitFirstStackAfterSuccess();
    }

    void playerMoveToNextStack()
    {
        Runner.StartRunning();
    }

    void inputTapped()
    {
        callNextStack();
    }

    public void InitFirstStackAfterSuccess()
    {                
        Vector3 pos = Runner.GetPosition();
        pos.y = 0;
        pos.z += GameManager.instance.Settings.FinishLineWorldLength;
        LastPlacedStack = ObjectPooler.instance.GetPooledObject("Stack", pos, Quaternion.identity).GetComponent<Stack>();
        LastPlacedStack.SetPosition(new Vector3(0, LastPlacedStack.GetPosition().y, LastPlacedStack.GetPosition().z));
        LastPlacedStack?.StopPingPong();
        

    }

    void callNextStack()
    {
        if (levelStackCount > LevelMaxStackCount)
            return;

        LastPlacedStack.StopPingPong();

        if (levelStackCount > 1)
            CheckPrevAndLast();

        if (levelStackCount == LevelMaxStackCount)
        {            
            Runner.GoToFinishLine();
            levelStackCount++;            
            return;
        }
            
        levelStackCount++;
        
        Vector3 pos = LastPlacedStack.GetPosition();
        pos.z += GameManager.instance.Settings.StackLength;
        PrevPlacedStack = LastPlacedStack;
        LastPlacedStack = ObjectPooler.instance.GetPooledObject("Stack", pos, Quaternion.identity).GetComponent<Stack>();
        LastPlacedStack.SetScale(PrevPlacedStack.GetTransform().lossyScale);

        playerMoveToNextStack();
    }


    void randomGenerateNewLevel()
    {
        levelStackCount = 1;
        LevelMaxStackCount = Random.Range(10 + succeedLevelCount, 15 + succeedLevelCount);
        Vector3 finishPos = new Vector3(0, 0.4857f, (Runner.GetPosition().z  + GameManager.instance.Settings.FinishLineLength) + (LevelMaxStackCount * GameManager.instance.Settings.StackLength));
        LastFinishLine = ObjectPooler.instance.GetPooledObject("Finish", finishPos, Quaternion.identity).GetComponent<FinishLine>();

    }






    void CheckPrevAndLast()
    {


        Vector3 currentBlockPosVector = VectorHelper.GetXVector(LastPlacedStack.GetPosition());
        Vector3 baseBlockPosVector = VectorHelper.GetXVector(PrevPlacedStack.GetPosition());

        float gameOverDist = LastPlacedStack.GetTransform().lossyScale.x * 0.5f + PrevPlacedStack.GetTransform().lossyScale.x * 0.5f;

        
        if (Vector3.Distance(currentBlockPosVector, baseBlockPosVector) > gameOverDist)
        {
            LastPlacedStack.LetItFall();
            return;
        }

        
        if (Vector3.Distance(currentBlockPosVector, baseBlockPosVector) < GameManager.instance.Settings.perfectDist)
        {
            PerfectScore();            
        }
        else
        {
           perfectScoreCounter = 0;
           ChopStack();
        }
    }


    void PerfectScore()
    {
        perfectScoreCounter++;
        GameManager.instance.SoundManager.PlayPerfectSFX(perfectScoreCounter);
    }



    void ChopStack()
    {
        VectorHelper.Vector3Coord axis = VectorHelper.Vector3Coord.x;

        GameManager.instance.SoundManager.PlaySplitSFX();

        Material oldMat = LastPlacedStack.GetActiveMaterial();
        
        float baseBlockPos = PrevPlacedStack.GetPosition().x;
        float baseBlockScale = PrevPlacedStack.GetTransform().lossyScale.x;
        float currentBlockPos = LastPlacedStack.GetPosition().x;
        
        float relDist = Mathf.Abs(baseBlockPos - currentBlockPos);
        
        float remainingBlockScale = baseBlockScale - relDist;
        
        Stack leftoverBlockPiece = ObjectPooler.instance.GetPooledObject("Stack", LastPlacedStack.GetPosition(),Quaternion.identity).GetComponent<Stack>();
        leftoverBlockPiece.SetMaterial(oldMat);

        int sign = currentBlockPos < baseBlockPos ? -1 : 1;

        float remainingBlockPos = (baseBlockPos + sign * baseBlockScale * 0.5f) - sign * remainingBlockScale * 0.5f;
        float leftoverBlockScale = relDist;
        float leftoverBlockPos = remainingBlockPos + sign * remainingBlockScale * 0.5f + sign * leftoverBlockScale * 0.5f;

        leftoverBlockPiece.SetScale(VectorHelper.GetVectorWith(axis, LastPlacedStack.GetTransform().lossyScale, leftoverBlockScale));
        leftoverBlockPiece.SetPosition(VectorHelper.GetVectorWith(axis, LastPlacedStack.GetPosition(), leftoverBlockPos));
        leftoverBlockPiece.LetItFall();
        
        LastPlacedStack.SetPosition(VectorHelper.GetVectorWith(axis, LastPlacedStack.GetPosition(), remainingBlockPos));
        LastPlacedStack.SetScale(VectorHelper.GetVectorWith(axis, LastPlacedStack.GetTransform().lossyScale, remainingBlockScale));        
        //baseBlock = currentBlock.transform;
    }


}
