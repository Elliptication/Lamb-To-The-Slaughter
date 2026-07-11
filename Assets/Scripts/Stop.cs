using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stop : MonoBehaviour
{
    public BasicMove moveScript;
    public void StopPlayer()
    {
        if (moveScript != null)
        {
            moveScript.StopMovement();
            moveScript.runSpeed = 0;
            moveScript.enabled = false;
        }
    }

    public void StartPlayer()
    {
        if (moveScript != null)
        {
            moveScript.enabled = true;
            moveScript.runSpeed = 2.0f;
        }
    }
}
