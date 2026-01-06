using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PhaseManager : MonoBehaviour
{

    public UnityEvent _onRevelationStart;
    public UnityEvent _onRevelationEnd;
    public UnityEvent _onDeathStart;
    public UnityEvent _onDeathEnd;
    public UnityEvent _onHideStart;
    public UnityEvent _onHideEnd;
    public UnityEvent _onSamStart;
    public UnityEvent _onSamEnd;
    public UnityEvent _onRegStart;
    public UnityEvent _onRegEnd;
    public UnityEvent _onWin;
    public UnityEvent _onEnd;
    public enum CurrentState{
        REV, 
        DEA, 
        HID, 
        SAM, 
        REG,
        REVE
    }
    public CurrentState currentState = CurrentState.REV;


    // Start is called before the first frame update
    void Start()
    {
        _onRevelationStart.Invoke();
        currentState = CurrentState.REV;
    }

    public void RevelationEnd()
    {
        _onRevelationEnd.Invoke();
        currentState = CurrentState.REVE;
    }

    public void DeathStart()
    {
        _onDeathStart.Invoke();
        currentState = CurrentState.DEA;
    }

    public void DeathEnd()
    {
        _onDeathEnd.Invoke();
    }

    public void HideStart()
    {
        _onHideStart.Invoke();
        currentState = CurrentState.HID;
    }

    public void HideEnd()
    {
        _onHideEnd.Invoke();
    }

    public void SamStart()
    {
        _onSamStart.Invoke();
        currentState = CurrentState.SAM;
    }

    public void SamEnd()
    {
        _onSamEnd.Invoke();
    }

    public void RegStart()
    {
        _onRegStart.Invoke();
        currentState = CurrentState.REG;
    }

    public void RegEnd()
    {
        _onRegEnd.Invoke();
    }

    public void Win()
    {
        _onWin.Invoke();
    }

    public void UponEnd()
    {
        _onEnd.Invoke();
    }


}
