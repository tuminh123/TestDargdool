using UnityEngine;

public class InputContextUpdater : MonoBehaviour
{
    private void OnEnable()
    {
        InputManager.OnTap += OnTap;
        InputManager.OnSwipe += OnSwipe;
        InputManager.OnTouchState += OnTouch;
    }

    private void OnDisable()
    {
        InputManager.OnTap -= OnTap;
        InputManager.OnSwipe -= OnSwipe;
        InputManager.OnTouchState -= OnTouch;
    }

    private void OnTouch(bool touching)
    {
        InputContext.IsTouching = touching;
        if (!touching) InputContext.IsSwiping = false;
    }

    private void OnTap(Vector2 pos)
    {
        InputContext.LastTapPosition = pos;
    }

    private void OnSwipe(Vector2 delta)
    {
        InputContext.IsSwiping = true;

        float x = delta.x;
        float y = delta.y;

        if (Mathf.Abs(x) > Mathf.Abs(y))
        {
            if (x < 0) InputContext.SwipeLeft = true;
            else InputContext.SwipeRight = true;

            InputContext.MoveDirection = Mathf.Sign(x);
        }
        else
        {
            if (y < 0) InputContext.SwipeDown = true;
            else InputContext.SwipeUp = true;
        }
    }
}
