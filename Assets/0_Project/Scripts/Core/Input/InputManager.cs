using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    public static event Action<Vector2> OnTap;

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
    }
}
