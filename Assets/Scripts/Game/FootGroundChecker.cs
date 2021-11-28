using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootGroundChecker : MonoBehaviour
{

    RunnerController runner;
    LayerMask lm;

    private void Awake()
    {
        runner = GetComponentInParent<RunnerController>();        
    }

    private void Start()
    {
        lm = GameManager.instance.Settings.StackLayer;
    }
    public bool IsGrounded()
    {
        RaycastHit hit;
        float rayDistance = 0.5f;
        Vector3 rayPos = transform.position;        
        Ray ray = new Ray(rayPos, Vector3.down);
        Physics.Raycast(ray, out hit, rayDistance, lm);
        Debug.DrawRay(rayPos, Vector3.down * rayDistance, Color.red, 3f);


        return hit.collider;
         
    }
}
