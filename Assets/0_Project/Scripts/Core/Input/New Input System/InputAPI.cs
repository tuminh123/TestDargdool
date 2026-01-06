using UnityEngine;

public static class InputAPI
{
    public static bool SwipeLeft => InputContext.SwipeLeft;
    public static bool SwipeRight => InputContext.SwipeRight;
    public static bool SwipeUp => InputContext.SwipeUp;
    public static bool SwipeDown => InputContext.SwipeDown;

    public static bool IsTouching => InputContext.IsTouching;
    public static bool IsSwiping => InputContext.IsSwiping;

    public static float MoveDirection => InputContext.MoveDirection;
    public static Vector2 TapPosition => InputContext.LastTapPosition;
}
