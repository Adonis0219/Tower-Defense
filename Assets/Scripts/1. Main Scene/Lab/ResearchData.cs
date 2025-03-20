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
    ���Ӽӵ�, ���۴޷�, ��������, �������, ��ƿ����, ����������, �����Ǽӵ�, �¼�
}

public enum AtkRschType
{
    ������, ���ݼӵ�, ġ��Ÿ����, ����
}

public enum DefRschType
{
    ü��, ü��ȸ��, ������
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
    public float oriValue;      // �⺻��
    public float increaseValue; // ������
    public int maxLevel;
    public int[] costs;
    public float[] reqTimes;
}
