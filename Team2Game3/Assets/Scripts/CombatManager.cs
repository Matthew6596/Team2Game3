using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Animations;

public class CombatManager : MonoBehaviour
{
    public static CombatManager inst;

    public GameObject[] mainBtns;
    public GameObject itemSubPanel, magicSubPanel;
    public TMP_Dropdown itemDrop,magicDrop;
    public TMP_Text itemDescTxt,magicDescTxt;
    public MagicScript[] magicOptions;

    public Transform playerHpBar,playerManaBar,enemyHpBar,enemyManaBar;

    EnemyScript enemy;

    bool playerDefending=false;

    Animator anim;
    Animator enemyAnim;

    GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
        inst = this;
        gm = GameManager.gm;

        anim = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
        StartBattle();

    }

    public void StartBattle()
    {
        enemy = gm.enemy;
        enemyAnim = gm.enemyObj.GetComponent<Animator>();
        gm.enemyObj.transform.position = new Vector3(4.36f, 0.44f, 0);

        //Set dropdown values
        setDropdowns();
        //Default item desc text = item 0 desc
        if (gm.playerItems.Count > 0) itemDescTxt.text = gm.playerItems[0].description;
        //When player select item in dropdown, show item description
        itemDrop.onValueChanged.AddListener((int val) => {
            itemDescTxt.text = gm.playerItems[val].description;
        });

        magicSubPanel.transform.GetChild(1).gameObject.GetComponent<Button>().enabled = false;
        if (gm.playerItems.Count > 0)
        {
            itemSubPanel.transform.GetChild(1).gameObject.GetComponent<Button>().enabled = true;
        }
        MagicScript m = magicOptions[0];
        magicDescTxt.text = m.description;
        magicDrop.onValueChanged.AddListener((int val) => {
            MagicScript m = magicOptions[val];
            magicDescTxt.text = m.description;
            if (m.manaCost > gm.playerMana)
            {
                //disable confirm spell btn
                magicSubPanel.transform.GetChild(1).gameObject.GetComponent<Button>().enabled = false;
            }
            else
            {
                magicSubPanel.transform.GetChild(1).gameObject.GetComponent<Button>().enabled = true;
            }
        });
        UpdateBars();
    }

    //Player Actions --- Main buttons
    public void FightBtn()
    {
        //Fight
        anim.SetTrigger("attack");
        anim.SetBool("isAttacking", true);
        enemy.GetAttacked(gm.playerAttack);
        DoEnemyTurn();
    }
    public void DefendBtn()
    {
        //Defend
        playerDefending = true;
        DoEnemyTurn();
    }
    public void MagicBtn()
    {
        anim.SetBool("isAttacking", true);
        //Open magic sub menu
        magicSubPanel.SetActive(true);
    }
    public void ItemBtn()
    {
        if (gm.playerItems.Count > 0)
        {
            //Open item sub menu
            itemSubPanel.SetActive(true);
        }
    }

    //Player Actions --- Sub buttons
    public void BackBtn()
    {
        //Close sub menu
        magicSubPanel.SetActive(false);
        itemSubPanel.SetActive(false);
    }
    public void ConfirmUseItem()
    {
        ItemScript selectedItem = gm.playerItems[itemDrop.value];
        if (selectedItem != null)
        {
            DoItemAction(selectedItem.itemType);
            gm.playerItems.Remove(selectedItem);
            setDropdowns();
        }
        DoEnemyTurn();
    }
    public void ConfirmUseMagic()
    {
        MagicScript selectedSpell = magicOptions[magicDrop.value];
        gm.playerMana -= selectedSpell.manaCost;
        DoMagicAction(selectedSpell.magicType);
        DoEnemyTurn();
    }

    //Enemy Actions
    void DoEnemyTurn() //enemy ai
    {
        anim.SetBool("isAttacking", false);
        enemyAnim.SetBool("isAttacking", true);
        enemyAnim.SetTrigger("attack");
        UpdateBars();

        //Check if enemy dead
        if (enemy.health <= 0)
        {
            StartCoroutine(endBattle(true));
            return;
        }

        //do enemy ai
        int rand = Random.Range(0, 100);
        if (rand >= 50 || enemy.mana <= 0) //enemy attack
        {
            if (!playerDefending)
                AttackPlayer(enemy.attackPower);
        }
        else//enemy magic
        {
            int rand2 = Random.Range(0, 100);
            if (rand2 >= 50)
            {
                if (enemy.health < enemy.maxHealth)
                    enemy.health++;
            }
            else
            {
                gm.playerMana--;
                if (gm.playerMana <= 0) gm.playerMana = 0;
            }
            enemy.mana--;
        }

        UpdateBars();

        //
        EndTurn();
    }


    //General Stuff
    void EndTurn()
    {
        enemyAnim.SetBool("isAttacking", false);
        //Check healths > 0, if not end battle
        if (gm.playerHp <= 0)
        {
            StartCoroutine(endBattle(false));
        }

        UpdateBars();

    }
    void DoItemAction(ItemType t)
    {
        switch (t)
        {
            case (ItemType.FishingRod):
                enemy.GetAttacked(1);
                enemy.LoseMana(1);
                gm.playerMana++;
                if(gm.playerMana>gm.playerMaxMana)gm.playerMana=gm.playerMaxMana;
                break;
            case (ItemType.KelpSandwich):
                HealPlayer(1);
                gm.playerMana+=2;
                if (gm.playerMana > gm.playerMaxMana) gm.playerMana = gm.playerMaxMana;
                break;
            default: break;
        }
    }
    void DoMagicAction(MagicType t)
    {
        switch (t)
        {
            case (MagicType.placeholdSpell):break;
            case (MagicType.FishHeal):
                HealPlayer(2);
                break;
            case (MagicType.BubbleGun):
                enemy.GetAttacked(gm.playerAttack * 2);
                break;
            default: break;
        }
    }
    IEnumerator endBattle(bool playerWon)
    {
        //disable buttons
        //Show battle over text
        yield return new WaitForSeconds(1);
        if (playerWon)
        {
            gm.enemyObj.SetActive(false);
            //return to swim
            gm.IncrementScore(enemy.scoreValue);
            gm.inCombat = false;
            gm.LoadSwimming();
        }
        else
        {
            //game over
            SceneManager.LoadScene("GameOverScene");
        }
    }
    void UpdateBars()
    {
        UpdateBar(playerHpBar, gm.playerHp);
        UpdateBar(playerManaBar, gm.playerMana);
        UpdateBar(enemyHpBar, enemy.health);
        UpdateBar(enemyManaBar, enemy.mana);
    }
    void UpdateBar(Transform bar, int amt)
    {
        int barLen = bar.childCount/2;
        for(int i=barLen; i<barLen*2; i++)bar.GetChild(i).gameObject.SetActive(false);
        for (int i = barLen; i < barLen + amt; i++) bar.GetChild(i).gameObject.SetActive(true);
    }

    //
    void setDropdowns()
    {
        itemDrop.options.Clear();
        foreach(ItemScript item in gm.playerItems)
        {
            itemDrop.options.Add(new TMP_Dropdown.OptionData(item.itemName));
        }
        magicDrop.options.Clear();
        foreach (MagicScript spell in magicOptions)
        {
            magicDrop.options.Add(new TMP_Dropdown.OptionData(spell.spellName));
        }
    }

    public void AttackPlayer(int amt)
    {
        gm.playerHp -= amt;
        if (gm.playerHp <= 0) gm.playerHp = 0;
    }
    public void HealPlayer(int amt)
    {
        gm.playerHp += amt;
        if (gm.playerHp >= gm.playerMaxHp) gm.playerHp = gm.playerMaxHp;
    }
}
