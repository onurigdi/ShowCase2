using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLine : MonoBehaviour
{
    Transform refTransform;

    private void Awake()
    {
        refTransform = transform;    
    }

    public Transform GetTransform()
    {
        return refTransform;
    }

    public Vector3 GetPosition()
    {
        return refTransform.position;
    }

}
