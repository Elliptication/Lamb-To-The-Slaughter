using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UnderlyingLogic : MonoBehaviour
{
    [SerializeField] SuspicionManager suspmang;
    [SerializeField] GameObject blackScreen;
    [SerializeField] GameObject redTint;
    public UnityEvent _afterCutToBlack;

    public void CutToBlackForTime(float time)
    {
        StartCoroutine(Cut(time));
    }

    public void CutBlack(bool be)
    {
        blackScreen.SetActive(be);
    }

    IEnumerator Cut(float time)
    {
        blackScreen.SetActive(true);

        yield return new WaitForSeconds(time);

        _afterCutToBlack.Invoke();
        blackScreen.SetActive(false);
        

    }

    public void RedTintState(bool en)
    {
        redTint.SetActive(en);
    }



}
