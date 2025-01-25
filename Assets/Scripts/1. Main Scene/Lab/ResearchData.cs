using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ResearchType
{
    Main, Attak, Defense, Utility, Length
}

[CreateAssetMenu(fileName = "Research Data", menuName = "Scriptable Object/Lab Data")]
public class ResearchData : ScriptableObject
{
    [Header("# Researh Info")]
    public ResearchType researchType;
    public int researchID;
    public string researchName;
    public string researchDesc;

    [Header("# Level Data")]
    public int[] costs;
    public float[] reqTimes;
    public float oriValue;      // 기본값
    public float increaseValue; // 증가값
    public int maxLevel;
}
