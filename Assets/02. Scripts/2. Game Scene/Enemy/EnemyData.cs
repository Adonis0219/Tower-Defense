using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "Enemy Data", menuName = "Scriptable Object/Enemy Data")]
public class EnemyData : ScriptableObject
{
    [Header("# Enemy Info")]
    public PoolObejectType type;
    public float spawnRate;
    public int maxSpawnCount;
    public Sprite sprite;
    public float scale;
    
    [Header("# Enemy Status")]
    public float killedDollar;
    public float killedCoin;
    public float moveSpeed;
    public float baseMaxHp;
    public float baseDmg;
}
