using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class StatsChanged : UnityEvent { }

public class Unit : Actor {

    public StatsChanged statsChanged = new StatsChanged();

    public int maxHealth;
    public int currentHealth;

    public int armor;
    public int magicResist;

    public int strength;
    public int agility;
    public int intelligence;

    public void takeDamage(int amount) {
        currentHealth -= amount;
        statsChanged.Invoke();
    }
}