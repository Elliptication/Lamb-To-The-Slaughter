using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMove : MonoBehaviour
{
    Rigidbody2D body;
    [SerializeField] SpriteRenderer playerRenderer;
    [SerializeField] Sprite up;
    [SerializeField] Sprite left;
    [SerializeField] Sprite right;
    [SerializeField] Sprite down;
    [SerializeField] SpriteRenderer lamb;
    [SerializeField] Transform t;
    private Vector2 moveDirection;
    private Vector2 lastDirection;

    public float runSpeed = 2.0f;
    public static bool canMove;
        private bool holdingRight, holdingLeft, holdingUp, holdingDown;
        private float pressRightTime, pressLeftTime, pressUpTime, pressDownTime;
        
        // Deadzone / debounce: require new direction to be held briefly before switching
        [SerializeField] private float deadzoneHoldSeconds = 0.08f;
        private Vector2 pendingDirection = Vector2.zero;
        private float pendingSince = -1f;
        private float lastDirectionChangeTime = -10f;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();

        Application.targetFrameRate = 60;
        t = GetComponent<Transform>();

        canMove = true;

    }

    // Update is called once per frame
    void Update()
    {
        moveDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (moveDirection.x != 0f || moveDirection.y != 0f)
        {
            // Use last-pressed direction for cardinal movement.
            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.A))
            {
                lastDirection = new Vector2(Mathf.Sign(moveDirection.x), 0f);
            }
            else if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S))
            {
                lastDirection = new Vector2(0f, Mathf.Sign(moveDirection.y));
            }
        }

        if (lastDirection.x != 0f && lastDirection.y != 0f)
        {
            // Should not happen, but fallback to horizontal if it does.
            lastDirection = new Vector2(lastDirection.x, 0f);
        }
        
            // Keyboard cardinal input handling (supports arrow keys and WASD)
            // Update hold states on key down
            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                holdingRight = true; pressRightTime = Time.time; lastDirection = Vector2.right;
            }
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                holdingLeft = true; pressLeftTime = Time.time; lastDirection = Vector2.left;
            }
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                holdingUp = true; pressUpTime = Time.time; lastDirection = Vector2.up;
            }
            if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                holdingDown = true; pressDownTime = Time.time; lastDirection = Vector2.down;
            }

            // Update hold states on key up
            if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.RightArrow)) holdingRight = false;
            if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.LeftArrow)) holdingLeft = false;
            if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.UpArrow)) holdingUp = false;
            if (Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.DownArrow)) holdingDown = false;

            // If multiple directions are held, pick the one with the most recent press time
            Vector2 mostRecentHeld = Vector2.zero;
            float latest = -1f;
            if (holdingRight && pressRightTime > latest) { latest = pressRightTime; mostRecentHeld = Vector2.right; }
            if (holdingLeft && pressLeftTime > latest)   { latest = pressLeftTime; mostRecentHeld = Vector2.left; }
            if (holdingUp && pressUpTime > latest)       { latest = pressUpTime; mostRecentHeld = Vector2.up; }
            if (holdingDown && pressDownTime > latest)   { latest = pressDownTime; mostRecentHeld = Vector2.down; }

            // If we are not moving yet, accept immediate direction
            if (lastDirection == Vector2.zero && mostRecentHeld != Vector2.zero)
            {
                lastDirection = mostRecentHeld;
                lastDirectionChangeTime = Time.time;
                pendingDirection = Vector2.zero;
                pendingSince = -1f;
            }
            else if (mostRecentHeld != Vector2.zero)
            {
                // If a different direction is held than current, set pending and require hold for deadzone
                if (mostRecentHeld != lastDirection)
                {
                    if (pendingDirection != mostRecentHeld)
                    {
                        pendingDirection = mostRecentHeld;
                        pendingSince = Time.time;
                    }
                    else
                    {
                        // If held long enough, commit pending
                        if (Time.time - pendingSince >= deadzoneHoldSeconds)
                        {
                            lastDirection = pendingDirection;
                            lastDirectionChangeTime = Time.time;
                            pendingDirection = Vector2.zero;
                            pendingSince = -1f;
                        }
                    }
                }
                else
                {
                    // same as current, clear pending
                    pendingDirection = Vector2.zero;
                    pendingSince = -1f;
                }
            }
            else
            {
                // no holds -> clear movement
                pendingDirection = Vector2.zero;
                pendingSince = -1f;
                // optionally we could let lastDirection persist while key released; current behavior keeps it until StopMovement invoked
            }

        if (canMove)
        {
            if (lastDirection.x > 0f)
            {
                playerRenderer.sprite = right;
                lamb.flipX = false;
            }
            else if (lastDirection.x < 0f)
            {
                playerRenderer.sprite = left;
                lamb.flipX = true;
            }
            else if (lastDirection.y < 0f)
            {
                playerRenderer.sprite = down;
            }
            else if (lastDirection.y > 0f)
            {
                playerRenderer.sprite = up;
            }
        }
    }

    void FixedUpdate()
    {
        if (!canMove)
        {
            if (body != null)
                body.velocity = Vector2.zero;
            return;
        }

        // Move only while a direction key is being held; stop immediately when released
        bool anyHeld = holdingRight || holdingLeft || holdingUp || holdingDown;
        if (anyHeld && lastDirection != Vector2.zero)
        {
            body.velocity = lastDirection * runSpeed;
        }
        else
        {
            body.velocity = Vector2.zero;
        }
    }

    public void StopMovement()
    {
        if (body != null)
            body.velocity = Vector2.zero;

        lastDirection = Vector2.zero;
    }

    public void FaceTowards(Vector2 worldTargetPosition)
    {
        Vector2 delta = worldTargetPosition - new Vector2(t.position.x, t.position.y);
        if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
        {
            if (delta.x > 0f)
            {
                playerRenderer.sprite = right;
                lamb.flipX = false;
            }
            else
            {
                playerRenderer.sprite = left;
                lamb.flipX = true;
            }
        }
        else if (Mathf.Abs(delta.y) > 0f)
        {
            if (delta.y > 0f)
                playerRenderer.sprite = up;
            else
                playerRenderer.sprite = down;
        }
    }

    public void SetRunSpeed(float newSpeed)
    {
        runSpeed = newSpeed;
    }

    public void TranslateZ(float z)
    {
        StartCoroutine(TranslateAfterTime(2, 0, 0, z));
    }

    public void TranslateY(float y)
    {
        StartCoroutine(TranslateAfterTime(0.25f, 0, y, 0));
    }

    public void TranslateX(float x)
    {
        StartCoroutine(TranslateAfterTime(2, x, 0, 0));
    }

    IEnumerator TranslateAfterTime(float seconds, float x, float y, float z)
    {
        yield return new WaitForSeconds(seconds);
        t.Translate(x, y, z);
    }

    public static void setCanMove(bool f)
    {
        canMove = f;
    }


}

