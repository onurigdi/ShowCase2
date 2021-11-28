using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;
using DG.Tweening;

[DefaultExecutionOrder(600)]
public class Stack : MonoBehaviour
{
    
    enum PingPongDir
    {
        Right2Left = 0,
        Left2Right = 1
        
    }

    Rigidbody rb;
    Material activeMaterial;
    MeshRenderer mr;
    Transform refTransform;
    private IDisposable PingPongRX;
    PingPongDir pingPongDir;
    Tween pingPongTween;


    private void Awake()
    {
        SetInitialReferences();
    }

    void SetInitialReferences()
    {
        refTransform = transform;
        mr = GetComponent<MeshRenderer>();
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        SetMaterial(null);
    }

    public Vector3 GetPosition()
    {
        return refTransform.position;
    }

    public Transform GetTransform()
    {
        return refTransform;
    }

    public void SetPosition(Vector3 pos)
    {
        transform.position = pos;
    }

    public void SetScale(Vector3 scale)
    {
        transform.localScale = scale;
    }

    public Material GetActiveMaterial()
    {
        return activeMaterial;
    }

    
    private void OnEnable()
    {
        GameManager.instance.EventManager.levelLoaded += gameLoaded;
        GameManager.instance.EventManager.levelStarted += gameStarted;
        GameManager.instance.EventManager.levelSuccess += gameSuccees;
        GameManager.instance.EventManager.levelFailed+= gameFailed;
        StartPingPong();
    }

    private void OnDisable()
    {
        GameManager.instance.EventManager.levelLoaded -= gameLoaded;
        GameManager.instance.EventManager.levelStarted -= gameStarted;
        GameManager.instance.EventManager.levelSuccess -= gameSuccees;
        GameManager.instance.EventManager.levelFailed -= gameFailed;
    }
    

    public void StopPingPong()
    {
        pingPongTween?.Kill();
    }

    void StartPingPong()
    {
        StopPingPong();
        pingPongDir = (PingPongDir)Random.Range(0,2);
        float pingPongX = SetInitialPingPongPos(pingPongDir);
        float speed = Mathf.Clamp( GameManager.instance.Settings.SlowestPingPongSpeed - (GameManager.instance.ActiveLevel.succeedLevelCount * 0.2f), 1, GameManager.instance.Settings.SlowestPingPongSpeed);
        pingPongTween = transform.DOMoveX(pingPongX * -1, speed).SetLoops(-1,LoopType.Yoyo).SetUpdate(UpdateType.Fixed).SetEase(Ease.Linear);
      
    }

   
    public void SetMaterial(Material newMat)
    {
        
        activeMaterial = newMat == null ? GameManager.instance.Settings.StackMaterials[Random.Range(0, GameManager.instance.Settings.StackMaterials.Length)] : newMat;
        mr.material = activeMaterial;
    }
   

    void gameLoaded()
    {
        
    }

    void gameStarted()
    {
        
    }

    void gameFailed()
    {
        StopPingPong();
    }

    void gameSuccees()
    {

    }



    float SetInitialPingPongPos(PingPongDir dir)
    {
        float calculatedX = GameManager.instance.Settings.pingPongArray[(int)dir] * GameManager.instance.Settings.PingPongOfsetX;
        transform.position = new Vector3(calculatedX, transform.position.y, transform.position.z);
        return calculatedX;
    }

    

    public void LetItFall()
    {
        StopPingPong();
        rb.isKinematic = false;
    }

}
