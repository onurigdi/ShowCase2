using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(100)]
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Level ActiveLevel;
    [Header("Settings")]
    public GameSettings Settings;    
    [Header("Managers")]
    public EventMaster EventManager;
    public ObjectPooler Pooler;
    public UIManager UIManager;
    public AudioManager SoundManager;



    private void Awake()
    {
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;
        if (!instance)
            instance = this;
    }

    


}
