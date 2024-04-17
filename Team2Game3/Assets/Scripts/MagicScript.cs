using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MagicType { placeholdSpell, }
public class MagicScript : MonoBehaviour
{
    public MagicType magicType;
    public string spellName;
    [TextArea]
    public string description;
    public int manaCost;
}
