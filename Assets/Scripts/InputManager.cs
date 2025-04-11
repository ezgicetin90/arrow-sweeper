using UnityEngine;
using System;

public class InputManager : MonoBehaviour
{
    public static Action<string> OnInputDetected;

    private Vector2 startPos;
    private float startTime;
    private bool isHolding = false;
    private float holdThreshold = 0.6f; // Seconds to trigger "hold"

    void Update()
    {
        
#if UNITY_EDITOR || UNITY_STANDALONE
        KeyboardControls();
#else
        HandleTouchInput();
#endif
    }
    private void Start()
    {
        Debug.Log("🧠 InputManager is alive!");
    }
    void KeyboardControls()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow)) OnInputDetected?.Invoke("Up");
        else if (Input.GetKeyDown(KeyCode.DownArrow)) OnInputDetected?.Invoke("Down");
        else if (Input.GetKeyDown(KeyCode.LeftArrow)) OnInputDetected?.Invoke("Left");
        else if (Input.GetKeyDown(KeyCode.RightArrow)) OnInputDetected?.Invoke("Right");
        else if (Input.GetKeyDown(KeyCode.Space)) {
            OnInputDetected?.Invoke("Tap");
            Debug.Log("⏹ TAP detected");
        }
        else if (Input.GetKey(KeyCode.Space)) OnInputDetected?.Invoke("Hold");
    }



    void HandleTouchInput()
    {
        if (Input.touchCount == 0) return;

        Touch touch = Input.GetTouch(0);

        if (touch.phase == TouchPhase.Began)
        {
            startPos = touch.position;
            startTime = Time.time;
            isHolding = true;
        }

        if (touch.phase == TouchPhase.Ended)
        {
            float duration = Time.time - startTime;
            Vector2 endPos = touch.position;
            Vector2 delta = endPos - startPos;
            isHolding = false;

            if (duration >= holdThreshold && delta.magnitude < 30f)
            {
                OnInputDetected?.Invoke("Hold");
            }
            else if (delta.magnitude < 30f)
            {
                OnInputDetected?.Invoke("Tap");
            }
            else
            {
                DetectSwipeDirection(delta);
            }
        }
    }

    void DetectSwipeDirection(Vector2 delta)
    {
        if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
        {
            OnInputDetected?.Invoke(delta.x > 0 ? "Right" : "Left");
        }
        else
        {
            OnInputDetected?.Invoke(delta.y > 0 ? "Up" : "Down");
        }
    }
}
