using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stop : MonoBehaviour
{
    public BasicMove moveScript;
    public void StopPlayer()
    {
        moveScript.runSpeed = 0;
        moveScript.enabled = false;
    }

    public void StartPlayer()
    {
        moveScript.enabled = true;
        moveScript.runSpeed = 2.0f;
    }
}
