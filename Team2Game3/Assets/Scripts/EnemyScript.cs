using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    //Script for enemy stats / enemy combat stuff
    [NonSerialized]
    public int maxHealth;
    public int health;
    [NonSerialized]
    public int maxMana;
    public int mana;
    public int attackPower;
    public int scoreValue;

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
        if(health<=0) health = 0;
    }
    public void LoseMana(int amt)
    {
        mana -= amt;
        if (mana <= 0) mana = 0;
    }
}
