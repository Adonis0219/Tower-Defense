using UnityEngine;

public enum CardRarity
{
    일반, 레어, 에픽, Length
}

[System.Serializable]
[CreateAssetMenu(fileName = "Card Data", 
    menuName = "Scriptable Object/Card Data")]
public class CardData : ScriptableObject
{
    [Header("# Card Info")]
    public int cardID;
    public string cardName;
    public string cardDesc;
    public Sprite cardIcon;
    public CardRarity rarity;
    public bool isGet = false;
    public float weight;
    public Color myColor;

    [Header("# Level Data")]
    public int curLv;
    public int MaxLv;
    public float[] value;
    public int curCardCount;
}
