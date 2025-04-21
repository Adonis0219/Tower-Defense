using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "Enemy Data", menuName = "Scriptable Object/Enemy Data")]
public class EnemyData : ScriptableObject
{
    public EnemyType type;
    public float killedDollar;
    public float moveSpeed;
    public float baseMaxHp;
    public float baseDmg;
    public float spawnRate;
    public int maxSpawnCount;
}
