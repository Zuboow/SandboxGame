using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityHealthManager : MonoBehaviour
{
    public int healthPoints;
    public bool DamageEntity(int damage)
    {
        healthPoints -= damage;
        if (healthPoints <= 0)
        {
            return true;
        }
        return false;
    }
}
