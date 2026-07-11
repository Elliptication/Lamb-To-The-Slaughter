using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Ink.Runtime;

public class SuspicionManager : MonoBehaviour
{
    public static Image suspicionBar;
    public UnityEvent _onExceeded70;
    public UnityEvent _onBelow70;
    public UnityEvent _onBelow30;
    public bool bodyHidden;
    public bool lambThrown;
    public bool lambCooked;

    // The ink story that we're tracking (set by ManagerChoices)
    static Story currentStory;

    private bool sub30;
    private bool sub70;
    private bool sup70;

    public static float suspicionAmount = 0f;

    // Start is called before the first frame update
    void Start()
    {
        // don't create a separate Story here; rely on ManagerChoices to set the active story
        currentStory = ManagerChoices.currentStory;
    }

    // Update is called once per frame
    void Update()
    {
        // always prefer the active story from ManagerChoices if available
        if (ManagerChoices.currentStory != null)
            currentStory = ManagerChoices.currentStory;

        if (currentStory != null)
        {
            object varObj = null;
            try
            {
                varObj = currentStory.variablesState["suspicionDiff"];
            }
            catch
            {
                varObj = null;
            }

            float currentDiff = 0f;
            if (varObj != null)
            {
                if (varObj is int) currentDiff = (int)varObj;
                else if (varObj is float) currentDiff = (float)varObj;
                else
                {
                    try { currentDiff = System.Convert.ToSingle(varObj); } catch { currentDiff = 0f; }
                }
            }

            // If Ink set a non-zero delta, apply it once and reset the variable in the story
            if (Mathf.Abs(currentDiff) > 0.0001f)
            {
                ChangeSuspicion(currentDiff);
                try { currentStory.variablesState["suspicionDiff"] = 0; } catch { }
            }
        }

        //PROCESSING: determine which range we're in
        if (suspicionAmount >= 70)
        {
            sup70 = true;
            sub30 = false;
            sub70 = false;
        }
        else if (suspicionAmount < 30)
        {
            sub30 = true;
            sub70 = false;
            sup70 = false;
        }
        else
        {
            sub70 = true;
            sub30 = false;
            sup70 = false;
        }

        //EXECUTION: invoke relevant events
        if(sub30)
        {
            _onBelow30.Invoke();
        }
        else if(sub70)
        {
            _onBelow70.Invoke();
        }
        else if(sup70)
        {
            _onExceeded70.Invoke();
        }
    }

    public static void ChangeSuspicion(float suspDelta)
    {
        suspicionAmount += suspDelta;
        suspicionBar.fillAmount = suspicionAmount / 100f;
        suspicionAmount = Mathf.Clamp(suspicionAmount, 0 , 100);
    }

    public static void RefreshActiveStory()
    {
        currentStory = ManagerChoices.currentStory;
    }

}


