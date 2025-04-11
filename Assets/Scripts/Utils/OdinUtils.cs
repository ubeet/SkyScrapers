using Sirenix.Utilities;
using UnityEngine;


public static class OdinUtils
{
    public static Color FieldValid(bool valid)
        => valid ? Color.white : Color.gray;

    public static void DrawImage(Sprite image, float baseSize = 40f, float maxSize = 40f)
    {
        if (image == null) return;
        if (Application.isPlaying) return;
        GUIStyle style = new GUIStyle();
        style.alignment = TextAnchor.MiddleCenter;
        GUILayout.Label(image.texture, style, GUILayoutOptions.Width(baseSize).Height(baseSize).MaxHeight(maxSize).MaxWidth(maxSize));
    }

    public static void DrawText(string text, float baseSize = 40f, float maxSize = 40f)
    {
        if (string.IsNullOrWhiteSpace(text)) return;
        GUIStyle style = new GUIStyle();
        style.normal.textColor = Color.white;
        style.alignment = TextAnchor.MiddleCenter;
        GUILayout.Label(text, style, GUILayoutOptions.Width(baseSize).MaxWidth(maxSize).ExpandHeight(false));
    }

    public static string FormatTimeMessage(float time) =>
        time == 0 ? "seconds" : $" {Mathf.Floor(time / 60):00}:{Mathf.RoundToInt(time % 60):00}s";

    public static string FormatTimeMessage(float time, bool detailed) =>
        time == 0 ? "seconds" : $" {Mathf.Floor(time / 60):00}:{time-Mathf.Floor(time / 60) *60:00.##}s";
    
    public static string SuffixPercentage(float value) => $"{value:P}";
}