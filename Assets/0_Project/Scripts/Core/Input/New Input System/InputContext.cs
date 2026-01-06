using UnityEngine;

public static class InputContext
{
    public static bool IsTouching;
    public static bool IsSwiping;

    public static bool SwipeLeft;
    public static bool SwipeRight;
    public static bool SwipeUp;
    public static bool SwipeDown;

    public static float MoveDirection; // -1 | 0 | 1
    public static Vector2 LastTapPosition;

    public static void ResetFrame()
    {
        SwipeLeft = SwipeRight = SwipeUp = SwipeDown = false;
        MoveDirection = 0;
        IsSwiping = false;
    }
}
