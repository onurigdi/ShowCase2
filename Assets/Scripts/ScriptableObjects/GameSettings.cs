using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EditorGameSettings", menuName = "Scriptlable Objects/EditorGameSettings")]
public class GameSettings : ScriptableObject
{
    public float RunnerDefaultRunDistance;
    public float SlowestPingPongSpeed;
    public float  StackHalfDistance;
    public float StackLength;
    public float FinishLineLength;
    public float FinishLineWorldLength;
    public float PingPongOfsetX;
    public float perfectDist;
    public Material[] StackMaterials;
    public int[] pingPongArray;
    public LayerMask StackLayer;
    public Vector3 DefaultStackScale;
    



}

