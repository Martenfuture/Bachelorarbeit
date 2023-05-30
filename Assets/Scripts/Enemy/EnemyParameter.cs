using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyParameter
{
    public float Speed;
    public float Health;
    public int Damage;

    public EnemyParameter(float speed, float health, int damage)
    {
        Speed = speed;
        Health = health;
        Damage = damage;
    }
}
