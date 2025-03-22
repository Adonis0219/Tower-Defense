using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum CardRarity
{
    Common, Rare, Epic, Legendary
}

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
    public int curCardCout;        // 현재 가지고 있는 카드 개수
}
