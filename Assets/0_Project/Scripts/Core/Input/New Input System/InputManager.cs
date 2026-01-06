using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    public static event Action<Vector2> OnTap;
    public static event Action<Vector2> OnSwipe;
    public static event Action<bool> OnTouchState;

    [Header("Settings")]
    public float tapTime = 0.25f;
    public float tapMaxMovement = 25f;
    public float swipeThreshold = 50f;
    public float deadzone = 6f;

    private Vector2 startTouch;
    private float startTime;
    private bool tapCandidate;

    private bool ignoreCurrentTouch;
    private void Update()
    {
#if UNITY_EDITOR
        HandleMouse();
#else
        HandleTouch();
#endif
    }

    private bool IsPointerOverUI()
    {
        if (EventSystem.current == null) return false;
#if UNITY_EDITOR
        return EventSystem.current.IsPointerOverGameObject();
#else
        return Input.touchCount > 0 &&
               EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId);
#endif
    }

    private void BeginTouch(Vector2 pos)
    {
        if (IsPointerOverUI())
        {
            ignoreCurrentTouch = true;
            return;
        }
        ignoreCurrentTouch = false;


        startTouch = pos;
        startTime = Time.unscaledTime;
        tapCandidate = true;
        OnTouchState?.Invoke(true);
    }

    private void EndTouch(Vector2 pos)
    {
        if (ignoreCurrentTouch)
        {
            ignoreCurrentTouch = false;
            OnTouchState?.Invoke(false);
            return;
        }

        OnTouchState?.Invoke(false);

        float held = Time.unscaledTime - startTime;
        float dist = (pos - startTouch).magnitude;

        if (tapCandidate && held <= tapTime && dist <= tapMaxMovement)
            OnTap?.Invoke(pos);
        else
            ProcessSwipe(pos);
    }

    private void ProcessSwipe(Vector2 pos)
    {
        Vector2 delta = pos - startTouch;

        if (delta.magnitude < deadzone) return;
        if (delta.magnitude < swipeThreshold) return;

        OnSwipe?.Invoke(delta);
    }

    private void HandleMouse()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (IsPointerOverUI()) return;
            BeginTouch(Input.mousePosition);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            EndTouch(Input.mousePosition);
        }
    }

    private void HandleTouch()
    {
        if (Input.touchCount == 0) return;

        Touch t = Input.GetTouch(0);

        if (t.phase == TouchPhase.Began)
        {
            if (IsPointerOverUI()) return;
            BeginTouch(t.position);
        }
        else if (t.phase == TouchPhase.Ended || t.phase == TouchPhase.Canceled)
        {
            EndTouch(t.position);
        }
    }

    #region Test
    /*public static event Action<Vector2> OnTap;

    private void Update()
    {
#if UNITY_EDITOR
        HandleMouse();
#else
        HandleTouch();
#endif
    }
    private bool IsPointerOverUI()
    {
#if UNITY_EDITOR
        return EventSystem.current != null &&
               EventSystem.current.IsPointerOverGameObject();
#else
        if (EventSystem.current == null) return false;

        if (Input.touchCount > 0)
            return EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId);

        return false;
#endif
    }

    private void HandleMouse()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (IsPointerOverUI()) return; // ⛔ chạm UI (joystick)

            OnTap?.Invoke(Input.mousePosition);
        }
    }

    private void HandleTouch()
    {
        if (Input.touchCount == 0) return;

        Touch t = Input.GetTouch(0);
        if (t.phase == TouchPhase.Began)
        {
            if (IsPointerOverUI()) return; // ⛔ chạm UI

            OnTap?.Invoke(t.position);
        }
    }*/
    #endregion

}
