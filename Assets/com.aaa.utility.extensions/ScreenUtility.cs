using UnityEngine;

namespace AAA.Utility.Extensions
{
    public static class ScreenUtility
    {
        public static Vector2 ScreenToViewPort(Vector2 vector2)
        {
            vector2.x = vector2.x / Screen.width;
            vector2.y = vector2.y / Screen.height;
            return vector2;
        }
        public static Vector2 ViewportToScreen(Vector2 vector2)
        {
            vector2.x = vector2.x * Screen.width;
            vector2.y = vector2.y * Screen.height;
            return vector2;
        }

        // This is useful when using a delta Value in viewport space. in Screenspace delta values are correct, but viewport space is
        // relative and therefore the aspect ratio of the screen will affect the delta value.
        // This will make the distance of the vector not based on aspect ratio. The y value may go above 1 in portrait mode.
        public static Vector2 FixViewportDistanceHeight(Vector2 vector2, bool isPortrait = false)
        {
            if (isPortrait)
            {
                vector2.y *= Screen.height;
                vector2.y /= Screen.width;
                return vector2;
            }
            else
            {
                vector2.y *= Screen.width;
                vector2.y /= Screen.height;
                return vector2;
            }
        }

        // This is useful when using a delta Value in viewport space. in Screenspace delta values are correct, but viewport space is
        // relative and therefore the aspect ratio of the screen will affect the delta value.
        // This will make the distance of the vector not based on aspect ratio. The x value may go above 1 in landscape mode.
        public static Vector2 FixViewportDistanceWidth(Vector2 vector2, bool isPortrait = false)
        {
            if (isPortrait)
            {
                vector2.x *= Screen.width;
                vector2.x /= Screen.height;
                return vector2;
            }
            else
            {
                vector2.x *= Screen.height;
                vector2.x /= Screen.width;
                return vector2;
            }
        }

    }
}
