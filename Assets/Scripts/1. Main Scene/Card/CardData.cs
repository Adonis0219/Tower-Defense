using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum CardRarity
{
    �Ϲ�, ����, ����, ����
}

[System.Serializable]
[CreateAssetMenu(fileName = "Card Data", menuName = "Scriptable Object/Card Data")]
public class CardData : ScriptableObject
{
    [Header("# Card Info")]
    public int cardID;
    public string cardName;
    public string cardDesc;
    public Sprite cardIcon;
    public CardRarity rarity;

    [Header("# Level Data")]
    public int curLv;
    public int MaxLv;
    public float[] value;
    public int curCardCount;        // ���� ������ �ִ� ī�� ����
}
