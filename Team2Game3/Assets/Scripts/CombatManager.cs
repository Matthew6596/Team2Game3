using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;

public class CombatManager : MonoBehaviour
{
    public GameObject[] mainBtns;
    public GameObject itemSubPanel, magicSubPanel;
    public TMP_Dropdown itemDrop,magicDrop;
    public TMP_Text itemDescTxt,magicDescTxt;

    EnemyScript enemy;

    bool playerDefending=false;

    GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.gm;
        enemy = gm.enemy;


        //Set dropdown values
        setDropdowns();
        //Default item desc text = item 0 desc
        if(gm.playerItems.Count>0) itemDescTxt.text = gm.playerItems[0].description;
        //When player select item in dropdown, show item description
        itemDrop.onValueChanged.AddListener((int val) => {
            itemDescTxt.text = gm.playerItems[val].description;
        });

        magicDrop.onValueChanged.AddListener((int val) => {
            MagicScript m = gm.magicOptions[val];
            magicDescTxt.text = m.description;
            if (m.manaCost>gm.playerMana)
            {
                //disable confirm spell btn
            }
        });
    }

    //Player Actions --- Main buttons
    public void FightBtn()
    {
        //Fight
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
        MagicScript selectedSpell = gm.magicOptions[magicDrop.value];
        gm.playerMana -= selectedSpell.manaCost;
        DoMagicAction(selectedSpell.magicType);
        DoEnemyTurn();
    }

    //Enemy Actions
    void DoEnemyTurn() //enemy ai
    {
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
                gm.playerHp -= enemy.attackPower;
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
            gm.playerHp = 0;
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
            case (MagicType.placeholdSpell):

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
        }
        else
        {
            //game over
        }
    }
    void UpdateBars()
    {

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
        foreach (MagicScript spell in gm.magicOptions)
        {
            magicDrop.options.Add(new TMP_Dropdown.OptionData(spell.spellName));
        }
    }
}
