using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType { FishingRod,}

public class ItemScript : MonoBehaviour
{
    public ItemType itemType;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public string GetItemDescription()
    {
        switch (itemType)
        {
            case ItemType.FishingRod: return "description";
        }
        return "";
    }
}
