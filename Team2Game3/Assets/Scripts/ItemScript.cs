using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType { FishingRod,}

public class ItemScript : MonoBehaviour
{
    public ItemType itemType;
    public string itemName;
    [TextArea]
    public string description;
}
