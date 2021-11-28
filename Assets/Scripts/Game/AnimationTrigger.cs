using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTrigger : MonoBehaviour
{
    RunnerController runner;

    private void Awake()
    {
         runner = GetComponentInParent<RunnerController>();
    }


    public void RightStep()
    {
        runner.CheckRightStep();
    }

    public void LeftStep()
    {
        runner.CheckLeftStep();
    }

}
