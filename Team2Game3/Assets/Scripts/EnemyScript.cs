using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    //Script for enemy stats / enemy combat stuff
    int maxHealth;
    public int health;
    int maxMana;
    public int mana;
    public int attackPower;

    SpriteRenderer render;

    // Start is called before the first frame update
    void Start()
    {
        maxHealth = health;
        maxMana = mana;
        render = GetComponent<SpriteRenderer>();
    }

    public void GetAttacked(int amt)
    {
        health -= amt;
    }
}
