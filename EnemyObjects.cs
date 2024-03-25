using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "EnemyObjects")]
public class EnemyObjects : ScriptableObject
{
    public string enemyName;
    public string element;
    public Sprite sprite;
    public int health;
    public float attackSpeed;
    public int damage;
    public int gold;
}