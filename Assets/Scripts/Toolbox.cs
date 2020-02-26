using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class Toolbox
{
    public static void DrawGrid(Rect viewRect, float gridSpacing, float gridOpacity, Color gridColor, Vector2 offset)
    {
        int widthDivs = Mathf.CeilToInt(viewRect.width / gridSpacing);
        int heightDivs = Mathf.CeilToInt(viewRect.height / gridSpacing);
        Handles.BeginGUI();
        Handles.color = new Color(gridColor.r, gridColor.g, gridColor.b, gridOpacity);

        for (int x = 0; x < widthDivs; x++) {
            Handles.DrawLine(new Vector3(gridSpacing * x + offset.x, 0f, 0f), new Vector3(gridSpacing * x + offset.x, viewRect.height, 0f));
        }

        for (int y = 0; y < heightDivs; y++) {
            Handles.DrawLine(new Vector3(0f, gridSpacing * y + offset.y, 0f), new Vector3(viewRect.width, gridSpacing * y + offset.y, 0f));
        }
        Handles.color = Color.white;
        Handles.EndGUI();
    }

    public static bool IsNullOrEmpty(this string str)
    {
        return str == null || str.Length == 0;
    }
}

public static class RectExtensions
{
    public static Vector2 TopLeft(this Rect rect)
    {
        return new Vector2(rect.xMin, rect.yMin);
    }

    public static Rect ScaleSizeBy(this Rect rect, float scale)
    {
        return rect.ScaleSizeBy(scale, rect.center);
    }

    public static Rect ScaleSizeBy(this Rect rect, float scale, Vector2 pivotPoint)
    {
        Rect result = rect;
        result.x -= pivotPoint.x;
        result.y -= pivotPoint.y;
        result.xMin *= scale;
        result.xMax *= scale;
        result.yMin *= scale;
        result.yMax *= scale;
        result.x += pivotPoint.x;
        result.y += pivotPoint.y;
        return result;
    }

    public static Rect ScaleSizeBy(this Rect rect, Vector2 scale)
    {
        return rect.ScaleSizeBy(scale, rect.center);
    }

    public static Rect ScaleSizeBy(this Rect rect, Vector2 scale, Vector2 pivotPoint)
    {
        Rect result = rect;
        result.x -= pivotPoint.x;
        result.y -= pivotPoint.y;
        result.xMin *= scale.x;
        result.xMax *= scale.x;
        result.yMin *= scale.y;
        result.yMax *= scale.y;
        result.x += pivotPoint.x;
        result.y += pivotPoint.y;
        return result;
    }
}