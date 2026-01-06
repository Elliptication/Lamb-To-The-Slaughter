using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillTrigger : MonoBehaviour
{
 

    [Header("Ink JSON")]
    [SerializeField] private TextAsset inkJSON;

    private bool playerInRange;

    private void Awake()
    {
        playerInRange = false;
        
    }

    private void Update()
    {
        if (playerInRange && !ManagerChoices.GetInstance().dialogueIsPlaying)
        {
            
            if (InputManager.GetInstance().GetInteractPressed())
            {
                ManagerChoices.GetInstance().EnterDialogueMode(inkJSON);
            }
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            playerInRange = false;
        }
    }
}
