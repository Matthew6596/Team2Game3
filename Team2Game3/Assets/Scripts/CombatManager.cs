using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;

public class CombatManager : MonoBehaviour
{
    public TMP_Dropdown itemDrop;
    public TMP_Text itemDescTxt;

    EnemyScript enemy;

    GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.gm;
        enemy = gm.enemy;


        //Set item dropdown values
        setItemDropdown();
        //Default item desc text = item 0 desc
        if(gm.playerItems.Count>0) itemDescTxt.text = gm.playerItems[0].description;
        //When player select item in dropdown, show item description
        itemDrop.onValueChanged.AddListener((int val) => {
            itemDescTxt.text = gm.playerItems[val].description;
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Player Actions --- Main buttons
    public void FightBtn()
    {
        //Fight
        DoEnemyTurn();
    }
    public void DefendBtn()
    {
        //Defend
        DoEnemyTurn();
    }
    public void MagicBtn()
    {
        //Open magic sub menu?
        DoEnemyTurn();
    }
    public void ItemBtn()
    {
        if (gm.playerItems.Count > 0)
        {
            //Open item sub menu
        }
    }

    //Player Actions --- Sub buttons
    public void BackBtn()
    {
        //Close sub menu
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

    //Enemy Actions
    void DoEnemyTurn() //enemy ai
    {


        //
        EndTurn();
    }


    //General Stuff
    void EndTurn()
    {
        //Check healths > 0, if not end battle
        if (enemy.health <= 0)
        {
            enemy.health = 0;
            StartCoroutine(endBattle(false));
        }
        else if(gm.playerHp <= 0)
        {
            gm.playerHp = 0;
            StartCoroutine(endBattle(true));
        }

        //Update hp/mana bars

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

    //
    void setItemDropdown()
    {
        itemDrop.options.Clear();
        foreach(ItemScript item in gm.playerItems)
        {
            itemDrop.options.Add(new TMP_Dropdown.OptionData(item.itemName));
        }
    }
}
