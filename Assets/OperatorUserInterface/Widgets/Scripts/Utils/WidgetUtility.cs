using UnityEngine;

namespace Widgets
{
    public static class WidgetUtility
    {
        /// <summary>
        /// Converts the Json friendly byte array to determine the color to the
        /// Unity friendly type Color32. Also checks for invalid input.
        /// </summary>
        /// <param name="b">the byte array that should be converted to a Color</param>
        /// <returns>the color as type Color</returns>
        public static Color BytesToColor(byte[] b)
        {
            if (b == null || !(b.Length == 3 || b.Length == 4))
            {
                return new Color32(255, 255, 255, 255);
            }

            if (b.Length == 3)
            {
                return new Color32(b[0], b[1], b[2], 255);
            }
            return new Color32(b[0], b[1], b[2], b[3]);
        }

        public static WidgetPosition StringToWidgetPosition(string widgetPositionAsString)
        {
            switch (widgetPositionAsString)
            {
                case "top":
                    return WidgetPosition.Top;

                case "left":
                    return WidgetPosition.Left;

                case "right":
                    return WidgetPosition.Right;

                case "bottom":
                    return WidgetPosition.Bottom;

                case "center":
                    return WidgetPosition.Center;

                case "child":
                    return WidgetPosition.Child;
                
                case "bottomleft":
                    return WidgetPosition.BottomLeft;

                default:
                    Debug.LogWarning("Widget position " + widgetPositionAsString + " not known.");
                    return WidgetPosition.Incorrect;
            }
        }

        public static RelativeChildPosition StringToRelativeChildPosition(string relativeChildPositionAsString)
        {
            switch (relativeChildPositionAsString)
            {
                case "top":
                    return RelativeChildPosition.Top;

                case "left":
                    return RelativeChildPosition.Left;

                case "right":
                    return RelativeChildPosition.Right;

                case "bottom":
                    return RelativeChildPosition.Bottom;

                case "fixedCenter":
                    return RelativeChildPosition.FixedCenter;

                case null:
                    return RelativeChildPosition.Incorrect; 
                    
                default:
                    Debug.LogWarning("Relative child position " + relativeChildPositionAsString + " not known.");
                    return RelativeChildPosition.Incorrect;
            }
        }
    }

    public enum WidgetPosition { Top, Left, Right, Center, Bottom, Child, BottomLeft, Incorrect };
    public enum RelativeChildPosition { Top, Left, Right, FixedCenter, Bottom, Incorrect };
}