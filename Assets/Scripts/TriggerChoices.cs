using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerChoices : MonoBehaviour
{
    [Header("Visual Cue")]
    [SerializeField] private GameObject visualCue;

    [Header("Ink JSON")]
    [SerializeField] private TextAsset inkJSON;

    private bool playerInRange;
    private GameObject playerObject;
    private BasicMove playerMove;
    private Stop playerStop;

    private void Awake()
    {
        playerInRange = false;
        visualCue.SetActive(false);
    }

    private void Update()
    {
        if (playerInRange && !ManagerChoices.GetInstance().dialogueIsPlaying)
        {
            visualCue.SetActive(true);
            if (InputManager.GetInstance().GetInteractPressed())
            {
                playerMove?.FaceTowards(transform.position);
                playerMove?.StopMovement();
                playerStop?.StopPlayer();
                ManagerChoices.GetInstance().EnterDialogueMode(inkJSON);
            }
        }
        else
        {
            visualCue.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            playerInRange = true;
            playerObject = collider.gameObject;
            playerMove = playerObject.GetComponent<BasicMove>();
            playerStop = playerObject.GetComponent<Stop>();
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            playerInRange = false;
            playerObject = null;
            playerMove = null;
            playerStop = null;
        }
    }
}



