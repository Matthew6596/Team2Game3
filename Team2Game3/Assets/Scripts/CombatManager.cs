using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    public TMP_Dropdown itemDrop;

    EnemyScript enemy;

    GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.gm;
        enemy = gm.enemy;
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
}
