using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[DefaultExecutionOrder(800)]
public class RunnerController : MonoBehaviour
{
    public enum Anims
    {
        Idle,
        Dance,
        Run
    }


    Tween runTween;
    Tween runComplateTween;
    private Animator animator;
    Transform refTransform;

    public FootGroundChecker LeftChecker;
    public FootGroundChecker RightChecker;

    private void Awake()
    {

        refTransform = transform;
    }

    public Vector3 GetPosition()
    {
        return refTransform.position;
    }

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

    void gameLoaded()
    {

    }

    void gameStarted()
    {

    }

    void gameFailed()
    {
        StopRunning();
    }

    void gameSuccees()
    {

    }


    public void StopRunning()
    {
        runTween?.Kill();
        runComplateTween?.Kill();
    }
    private void Start()
    {
        SetInitialReferences();
    }

    void SetInitialReferences()
    {
     
        animator = GetComponentInChildren<Animator>();
        SetKinematic(true);
    }

    public void SetAnimation(Anims animType)
    {
        animator?.SetTrigger(animType.ToString());
    }

    Vector3 calculateEndPosition()
    {
        Vector3 pos = GameManager.instance.ActiveLevel.PrevPlacedStack.GetPosition() ;
        pos.x = transform.position.x;
        pos.y = transform.position.y;
        pos.z += GameManager.instance.Settings.StackHalfDistance;
        return pos;
    }


    public void StartRunning()
    {
        SetAnimation(Anims.Run);
        StopRunning();
        Vector3 destPos = calculateEndPosition();
        float dist = Vector3.Distance(GetPosition(), destPos);
        float runSpeed = dist / GameManager.instance.Settings.RunnerDefaultRunDistance;

        

        runTween = transform.DOMove(destPos, runSpeed).SetEase(Ease.Linear).SetUpdate(UpdateType.Fixed);
        runComplateTween = DOVirtual.DelayedCall(runSpeed, () =>
        {
            GameManager.instance.ActiveLevel.Runner.SetAnimation(Anims.Idle);
        });

        /*
        .OnComplete(()=>
        {            
            
        });
        */
    }


    public void GoToFinishLine()
    {
        SetAnimation(Anims.Run);
        StopRunning();
        Vector3 destPos = GameManager.instance.ActiveLevel.LastFinishLine.GetPosition();
        destPos.y = 0;
        float dist = Vector3.Distance(GetPosition(), destPos);
        float runSpeed = dist / GameManager.instance.Settings.RunnerDefaultRunDistance;




        
        runTween = transform.DOMove(destPos, runSpeed).SetEase(Ease.Linear).SetUpdate(UpdateType.Fixed).OnComplete(() =>
        {
            GameManager.instance.ActiveLevel.Runner.SetAnimation(Anims.Dance);
            GameManager.instance.EventManager.CallLevelSuccess();
        });
    }


    public void CheckRightStep()
    {
        if (!RightChecker.IsGrounded())
        {
            LetItFall();
            
        }
    }

    public void CheckLeftStep()
    {
        if (!LeftChecker.IsGrounded())
        {
            LetItFall();

        }
    }


    void LetItFall()
    {
        SetKinematic(false);
        GameManager.instance.EventManager.CallLevelFailed();
    }

    void SetKinematic(bool newValue)
    {
        animator.enabled = newValue;
        Rigidbody[] bodies = GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody rb in bodies)
        {
            rb.isKinematic = newValue;
        }
    }

}
