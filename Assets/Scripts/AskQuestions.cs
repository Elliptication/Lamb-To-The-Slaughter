using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// NOTE: This script appears to be redundant with PoliceQuestions.cs
// If you want player-initiated dialogue (press a button to talk), keep this script.
// If dialogue should be automatic based on time, you can delete this script entirely.

public class AskQuestions : MonoBehaviour
{
    [Header("Ink JSON")]
    [SerializeField] private TextAsset inkJSON;

    [Header("Interaction")]
    [SerializeField] private GameObject interactPrompt; // Optional: UI prompt showing "Press E to talk"

    private bool playerInRange = false;

    void Update()
    {
        // Show/hide interact prompt based on player proximity
        if (interactPrompt != null)
        {
            interactPrompt.SetActive(playerInRange && !ManagerChoices.GetInstance().dialogueIsPlaying);
        }

        // Check for player interaction
        if (playerInRange && !ManagerChoices.GetInstance().dialogueIsPlaying)
        {
            // Check if player presses interact button (you'll need to define this in InputManager)
            if (Input.GetKeyDown(KeyCode.E)) // Or use: InputManager.GetInstance().GetInteractPressed()
            {
                StartDialogue();
            }
        }
    }

    private void StartDialogue()
    {
        if (inkJSON != null)
        {
            ManagerChoices.GetInstance().EnterDialogueMode(inkJSON);
        }
        else
        {
            Debug.LogWarning("No dialogue assigned to AskQuestions script!");
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}