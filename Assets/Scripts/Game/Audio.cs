using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio : MonoBehaviour
{
    private void OnEnable()
    {
        Invoke("Disable", 1);       
    }

    void Disable()
    {
        gameObject.SetActive(false);
    }

}
