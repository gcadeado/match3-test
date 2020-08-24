using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SwipeDirection
{
    None,
    Up,
    Right,
    Down,
    Left
}

public class InputHelpers
{
    public static SwipeDirection GetSwipeDirection(Vector2 initial, Vector2 final, float threashold)
    {
        float angle = CalculateAngle(initial, final);
        SwipeDirection direction = SwipeDirection.None;
        float max = GetSwipeMajorAxis(initial, final);
        if (max > threashold)
        {
            if (angle > -45f && angle <= 45f)
            {
                direction = SwipeDirection.Right;
            }
            else if (angle > 45f && angle <= 135f)
            {
                direction = SwipeDirection.Up;
            }
            else if (angle > 135f || angle <= -135f)
            {
                direction = SwipeDirection.Left;
            }
            else if (angle > -135f && angle <= -45f)
            {
                direction = SwipeDirection.Down;
            }
        }
        return direction;
    }

    static float CalculateAngle(Vector2 initial, Vector2 final)
    {
        float xDiff = final.x - initial.x;
        float yDiff = final.y - initial.y;
        float angle = Mathf.Atan2(yDiff, xDiff) * 180f / Mathf.PI;
        return angle;
    }

    static float GetSwipeMajorAxis(Vector2 initial, Vector2 final)
    {
        float maxX = Mathf.Abs(final.x - initial.x);
        float maxY = Mathf.Abs(final.y - initial.y);

        return Mathf.Max(maxX, maxY);
    }

}
