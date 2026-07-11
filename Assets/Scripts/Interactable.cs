using UnityEngine;
using UnityEngine.UI;

public class Interactable : MonoBehaviour
{
    public GameObject linked;
    public InteractionHandler handler;
    public float cooldown;
    public bool isDisabled = false;

    private Button linkedButton;
    private CanvasGroup linkedCanvasGroup;

    void Awake()
    {
        if (linked != null)
        {
            linkedButton = linked.GetComponent<Button>();
            linkedCanvasGroup = linked.GetComponent<CanvasGroup>();
        }
    }

    void Start()
    {
        if (linked != null && linked != gameObject)
            linked.SetActive(false);
    }

    public void SetUIActive(bool active)
    {
        if (linked == null)
            return;

        if (linked != gameObject)
        {
            linked.SetActive(active);
            return;
        }

        if (linkedCanvasGroup != null)
        {
            linkedCanvasGroup.alpha = active ? 1f : 0f;
            linkedCanvasGroup.interactable = active;
            linkedCanvasGroup.blocksRaycasts = active;
        }
        else if (linkedButton != null)
        {
            linkedButton.interactable = active;
        }
    }

    public void Highlight()
    {
        if (linkedButton != null && linkedButton.animator != null)
            linkedButton.animator.Play("Highlighted");
    }

    public void SetNormalState()
    {
        if (linkedButton != null && linkedButton.animator != null)
            linkedButton.animator.CrossFade("Normal", 0.3f);
    }

    public void SetDisabledState()
    {
        if (linkedButton != null && linkedButton.animator != null)
            linkedButton.animator.CrossFade("Disabled", 0.3f);
    }

    public void Press()
    {
        if (linkedButton != null)
        {
            linkedButton.onClick.Invoke();
            if (linkedButton.animator != null)
                linkedButton.animator.Play("Pressed");
        }

        isDisabled = true;
    }

    void OnTriggerEnter2D(Collider2D c)
    {
        if (c.CompareTag("InteractionHandler"))
        {
            SetUIActive(true);
            handler?.RegisterInteractable(this);
        }
    }

    void OnTriggerExit2D(Collider2D c)
    {
        if (c.CompareTag("InteractionHandler"))
        {
            SetUIActive(false);
            handler?.UnregisterInteractable(this);
        }
    }

    void OnDisable()
    {
        handler?.UnregisterInteractable(this);
    }
}
