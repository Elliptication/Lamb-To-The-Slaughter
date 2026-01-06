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

    // Set this file to your compiled json asset
	public TextAsset inkAsset;

	// The ink story that we're wrapping
	static Story currentStory;

    private int diff;

    private bool sub30;
    private bool sub70;
    private bool sup70;

    private float prevSusp = 0f;
    public static float suspicionAmount = 0f;

    bool hasDiffBeenSet = false;

    // Start is called before the first frame update
    void Start()
    {
        currentStory = new Story(inkAsset.text);
    }

    // Update is called once per frame
    void Update()
    {
        float currentSusp = (int) currentStory.variablesState["suspicionDiff"];

        if(prevSusp != currentSusp)
        {
            ChangeSuspicion(currentSusp);
        }
        
//ACTION

//PROCESSING

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
        else if (suspicionAmount < 70)
        {
            sub70 = true;
            sub30 = false;
            sup70 = false;
        }

//EXECUTION

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
        
        prevSusp = currentSusp;

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


