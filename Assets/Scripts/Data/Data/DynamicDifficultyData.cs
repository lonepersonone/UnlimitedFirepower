using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DynamicDifficultyData 
{
    public static float[] FirstStage = new float[4] { 0.3f, 0.5f, 0.15f, 0.05f };
    public static float[] SecondStage = new float[4] { 0.2f, 0.5f, 0.25f, 0.05f };
    public static float[] ThirdStage = new float[4] { 0f, 0.5f, 0.4f, 0.1f };
    public static float[] FourthStage = new float[4] { 0f, 0.3f, 0.5f, 0.2f };
    public static float[] FifthStage = new float[4] { 0f, 0f, 0.5f, 0.5f };

    public static float[] EasyWealth = new float[2] { 1000f, 0.2f };
    public static float[] NormalWealth = new float[2] { 1000f, 0.2f };
    public static float[] DifficultyWealth = new float[2] { 1000f, 0.2f };
    public static float[] NightmareWealth = new float[2] { 1000f, 0.2f };

    public static string Easy = "简单难度";
    public static string EasyColor = "#f0f8ff";
    public static string Normal = "普通模式";
    public static string NormalColor = "#7fffd4";
    public static string Difficulty = "困难模式";
    public static string DifficultyColor = "#a52a2a";
    public static string Nightmare = "噩梦模式";
    public static string NightmareColor = "#000000";

}
