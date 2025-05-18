using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ColorUtil 
{
    public static Color GetColor(string hexColor)
    {
        Color color;
        ColorUtility.TryParseHtmlString(hexColor, out color);
        return color;
    }
}
