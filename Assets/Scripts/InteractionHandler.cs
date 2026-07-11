using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionHandler : MonoBehaviour
{
    public List<Interactable> interactables = new List<Interactable>();
    private Transform currentPos;
    public bool isInCycle = false;

    void Start()
    {
        currentPos = transform;
    }

    void Update()
    {
        currentPos = transform;

        if (interactables.Count == 0)
            return;

        RemoveDestroyedInteractables();

        int minDex = GetClosestInteractableIndex();
        if (minDex == -1)
            return;

        Interactable closest = interactables[minDex];

        if (!isInCycle)
        {
            if (!closest.isDisabled)
                closest.Highlight();
            else
                closest.SetDisabledState();
        }

        if (Input.GetKeyDown(KeyCode.E) && !closest.isDisabled)
        {
            isInCycle = true;
            closest.Press();
            StartCoroutine(Cooldown(closest));
        }

        for (int i = 0; i < interactables.Count; i++)
        {
            if (i == minDex)
                continue;

            Interactable item = interactables[i];
            if (item == null)
                continue;

            if (!item.isDisabled)
                item.SetNormalState();
            else
                item.SetDisabledState();
        }
    }

    public void RegisterInteractable(Interactable interactable)
    {
        if (interactable == null || interactables.Contains(interactable))
            return;

        interactables.Add(interactable);
    }

    public void UnregisterInteractable(Interactable interactable)
    {
        if (interactable == null)
            return;

        interactables.Remove(interactable);
    }

    int GetClosestInteractableIndex()
    {
        float minVal = 15f;
        int minDex = -1;

        for (int i = 0; i < interactables.Count; i++)
        {
            Interactable item = interactables[i];
            if (item == null)
                continue;

            float dist = Vector2.Distance(
                new Vector2(currentPos.position.x, currentPos.position.y),
                new Vector2(item.transform.position.x, item.transform.position.y));

            if (dist < minVal)
            {
                minVal = dist;
                minDex = i;
            }
        }

        return minDex;
    }

    IEnumerator Cooldown(Interactable interactable)
    {
        if (interactable == null)
        {
            isInCycle = false;
            yield break;
        }

        yield return new WaitForSeconds(interactable.cooldown);
        interactable.isDisabled = false;
        isInCycle = false;
    }

    void RemoveDestroyedInteractables()
    {
        interactables.RemoveAll(item => item == null);
    }


}
