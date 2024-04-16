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
    public void Fight()
    {
        //Fight
    }
    public void Defend()
    {
        //Defend
    }
    public void Magic()
    {
        //Open magic sub menu?
    }
    public void Item()
    {
        if (gm.playerItems.Count > 0)
        {
            //Open item sub menu
        }
    }
    //Player Actions --- Sub buttons
    public void ConfirmUseItem()
    {
        ItemScript selectedItem = gm.playerItems[itemDrop.value];
        if (selectedItem != null)
        {
            DoItemAction(selectedItem.itemType);
        }
    }


    //Items - Do Item function
    public void DoItemAction(ItemType t)
    {
        switch (t)
        {
            case (ItemType.FishingRod):

                break;
            default: break;
        }
        //end player turn
    }
}
