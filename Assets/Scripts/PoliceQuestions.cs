using System.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PoliceQuestions : MonoBehaviour
{
    //Dialogue stuff
    [Header("Ink JSON")]
    [SerializeField] private TextAsset inkJSON;
    [SerializeField] private TextAsset inkJSON2;
    [SerializeField] private TextAsset inkJSON3;
    [SerializeField] private TextAsset inkJSON4;
    [SerializeField] private TextAsset inkJSON5;

    [Header("Suspicion")]
    public UnityEngine.UI.Image suspicionBar;

    [Header("UI")]
    [SerializeField] private GameObject Timer;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private Stop stop;

    // Timer
    public static float rem = 301;

    // Flags to ensure each dialogue triggers only once
    private bool dialogue1Triggered = false;
    private bool dialogue2Triggered = false;
    private bool dialogue3Triggered = false;
    private bool dialogue4Triggered = false;
    private bool dialogue5Triggered = false;
    private bool gameEnded = false;

    // Track which dialogue should play next
    private int currentDialogueIndex = 1;
    
    // Track if we were in dialogue last frame to detect when dialogue ends
    private bool wasInDialogue = false;

    void Start()
    {
        // Initialize timer
        rem = 301;
        gameEnded = false;

        stop.StopPlayer();
        
        // Reset all flags
        dialogue1Triggered = false;
        dialogue2Triggered = false;
        dialogue3Triggered = false;
        dialogue4Triggered = false;
        dialogue5Triggered = false;
        
        // Start with first dialogue
        currentDialogueIndex = 1;
        wasInDialogue = false;
    }

    void Update()
    {
        UpdateTimer();
        CheckDialogueTriggers();
        CheckDialogueCompletion();
    }

    private void UpdateTimer()
    {
        Timer.SetActive(true);

        if (rem > 0)
        {
            rem -= Time.deltaTime;
            
            // Update timer display
            int minutes = Mathf.FloorToInt(rem / 60);
            int seconds = Mathf.FloorToInt(rem % 60);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
        else if (!gameEnded)
        {
            // Time's up!
            rem = 0;
            timerText.color = Color.red;
            timerText.text = "00:00";
            
            gameEnded = true;
            EndGame();
        }
    }

    private void CheckDialogueTriggers()
    {
        // Don't trigger dialogues if game has ended or dialogue is already playing
        if (gameEnded || ManagerChoices.GetInstance().dialogueIsPlaying)
        {
            return;
        }

        // Trigger dialogues based on current index OR time thresholds (whichever comes first)
        
        // Dialogue 1: Start immediately or when time hits 300
        if ((currentDialogueIndex == 1 && rem <= 300) && !dialogue1Triggered)
        {
            dialogue1Triggered = true;
            ManagerChoices.GetInstance().EnterDialogueMode(inkJSON);
            Debug.Log("Dialogue 1 triggered");
        }
        // Dialogue 2: After dialogue 1 completes OR when time hits 240
        else if ((currentDialogueIndex == 2 || rem <= 240) && !dialogue2Triggered)
        {
            dialogue2Triggered = true;
            currentDialogueIndex = 2;
            ManagerChoices.GetInstance().EnterDialogueMode(inkJSON2);
            Debug.Log("Dialogue 2 triggered" + (rem > 240 ? " (naturally)" : " (time skip)"));
        }
        // Dialogue 3: After dialogue 2 completes OR when time hits 180
        else if ((currentDialogueIndex == 3 || rem <= 180) && !dialogue3Triggered)
        {
            dialogue3Triggered = true;
            currentDialogueIndex = 3;
            ManagerChoices.GetInstance().EnterDialogueMode(inkJSON3);
            Debug.Log("Dialogue 3 triggered" + (rem > 180 ? " (naturally)" : " (time skip)"));
        }
        // Dialogue 4: After dialogue 3 completes OR when time hits 120
        else if ((currentDialogueIndex == 4 || rem <= 120) && !dialogue4Triggered)
        {
            dialogue4Triggered = true;
            currentDialogueIndex = 4;
            ManagerChoices.GetInstance().EnterDialogueMode(inkJSON4);
            Debug.Log("Dialogue 4 triggered" + (rem > 120 ? " (naturally)" : " (time skip)"));
        }
        // Dialogue 5: After dialogue 4 completes OR when time hits 60
        else if ((currentDialogueIndex == 5 || rem <= 60) && !dialogue5Triggered)
        {
            dialogue5Triggered = true;
            currentDialogueIndex = 5;
            ManagerChoices.GetInstance().EnterDialogueMode(inkJSON5);
            Debug.Log("Dialogue 5 triggered" + (rem > 60 ? " (naturally)" : " (time skip)"));
        }
    }

    private void CheckDialogueCompletion()
    {
        bool isInDialogue = ManagerChoices.GetInstance().dialogueIsPlaying;
        
        // Detect when dialogue just finished (was in dialogue, now not)
        if (wasInDialogue && !isInDialogue)
        {
            // Move to next dialogue
            currentDialogueIndex++;
            stop.StopPlayer();
            Debug.Log("Dialogue completed. Ready for dialogue " + currentDialogueIndex);
            if(dialogue5Triggered)
            {
                stop.StartPlayer();
                EndGame();
            }
        }
        
        wasInDialogue = isInDialogue;
    }

    private void EndGame()
    {
        SceneManager.LoadScene("Win");
    }
}