using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ResearchType
{
    Attak, Defense, Utility, Main, Length
}

public enum MainRschType
{
    게임속도, 시작달러, 공격할인, 방어할인, 유틸할인, 연구실할인, 연구실속도, 승수
}

public enum AtkRschType
{
    데미지, 공격속도, 치명타배율, 범위
}

public enum DefRschType
{
    체력, 체력회복, 절대방어
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
    public float oriValue;      // 기본값
    public float increaseValue; // 증가값
    public int maxLevel;
    public int[] costs;
    public float[] reqTimes;
}
