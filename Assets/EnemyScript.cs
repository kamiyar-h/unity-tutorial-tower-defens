using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{

    private string _uniqueId;
    
    public int health = 10;
    
    private List<TowerScript> _towersAttackers = new List<TowerScript>();

    private void Awake()
    {
        _towersAttackers = new List<TowerScript>();

        _uniqueId = Helper.i.GetUniqueID(GetHashCode().ToString());

        name += " (" + _uniqueId +")";
    }

    public string GetUniqueID()
    {
        return _uniqueId;
    }
    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Die();
        }
    }

    public void AddTowerAttacker(TowerScript towerScript)
    {
        _towersAttackers.Add(towerScript);
    }

    private void Die()
    {

        foreach (TowerScript tower in _towersAttackers.ToArray())
        {
            tower.EnemyDead(_uniqueId);
        }
        
        Destroy(gameObject, 0.1f);
    }
}
