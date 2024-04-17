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
    public GameObject[] mainBtns;
    public GameObject itemSubPanel, magicSubPanel;
    public TMP_Dropdown itemDrop,magicDrop;
    public TMP_Text itemDescTxt,magicDescTxt;
    public MagicScript[] magicOptions;

    public Transform playerHpBar,playerManaBar,enemyHpBar,enemyManaBar;

    EnemyScript enemy;

    bool playerDefending=false;

    Animator anim;

    GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.gm;
        enemy = gm.enemy;
        gm.enemyObj.transform.position = new Vector3(4.36f, 0.44f, 0);

        anim = gameObject.GetComponent<Animator>();

        //Set dropdown values
        setDropdowns();
        //Default item desc text = item 0 desc
        if(gm.playerItems.Count>0) itemDescTxt.text = gm.playerItems[0].description;
        //When player select item in dropdown, show item description
        itemDrop.onValueChanged.AddListener((int val) => {
            itemDescTxt.text = gm.playerItems[val].description;
            itemDrop.value = 0;
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
            if (m.manaCost>gm.playerMana)
            {
                //disable confirm spell btn
                magicSubPanel.transform.GetChild(1).gameObject.GetComponent<Button>().enabled = false;
            }
            else
            {
                magicSubPanel.transform.GetChild(1).gameObject.GetComponent<Button>().enabled = true;
            }
            magicDrop.value = 0;
        });
    }

    //Player Actions --- Main buttons
    public void FightBtn()
    {
        //Fight
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
        toggleMainBtns(false);
    }
    public void ItemBtn()
    {
        if (gm.playerItems.Count > 0)
        {
            //Open item sub menu
            itemSubPanel.SetActive(true);
            toggleMainBtns(false);
        }
    }

    //Player Actions --- Sub buttons
    public void BackBtn()
    {
        //Close sub menu
        magicSubPanel.SetActive(false);
        itemSubPanel.SetActive(false);
        toggleMainBtns(true);
    }
    void toggleMainBtns(bool on)
    {
        for(int i=0; i<4; i++) mainBtns[i].SetActive(on);
    }
    public void ConfirmUseItem()
    {
        ItemScript selectedItem = gm.playerItems[itemDrop.value];
        if (selectedItem != null)
        {
            DoItemAction(selectedItem.itemType);
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
        UpdateBars();

        //Check if enemy dead
        if (enemy.health <= 0)
        {
            StartCoroutine(endBattle(false));
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
        //Check healths > 0, if not end battle
        if(gm.playerHp <= 0)
        {
            StartCoroutine(endBattle(true));
        }

        UpdateBars();

    }
    void DoItemAction(ItemType t)
    {
        switch (t)
        {
            case (ItemType.FishingRod):

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
            default: break;
        }
    }
    IEnumerator endBattle(bool playerWon)
    {
        //disable buttons
        //Show battle over text
        yield return new WaitForSeconds(2);
        if (playerWon)
        {
            //return to swim
            gm.IncrementScore(enemy.scoreValue);
            gm.inCombat = false;
            SceneManager.LoadScene("SampleScene");
        }
        else
        {
            //game over
            SceneManager.LoadScene("GameOver");
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
