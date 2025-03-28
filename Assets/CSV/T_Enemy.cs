using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "New Enemy_", menuName = "Assets/New Enemy")]
public class T_Enemy : ScriptableObject
{
    public string enemyName;
    public int hp;
    public int strength;
    public int xpReward;
}