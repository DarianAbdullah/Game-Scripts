using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy : MonoBehaviour
{
    public float hitPoints;
    public float maxHP = 10;

    void Start()
    {
        hitPoints = maxHP;
    }

    public void takeHit(float damage)
    {
        hitPoints -= damage;
        if (hitPoints <= 0)
        {
            Destroy(gameObject);
        }
    }
}
