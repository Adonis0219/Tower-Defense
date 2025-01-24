using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ResearchType
{
    Main, Attak, Defense, Utility
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
    public float[] values;
    public int maxLevel;
}
