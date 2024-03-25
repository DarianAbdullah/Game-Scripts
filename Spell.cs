using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Spell", menuName = "Spells")]
public class Spell : ScriptableObject
{
    public string spellName;  
    public string altName;  
    public string spellElement;
    public string subtype;
    public int baseSpellDamage; 
    private int spellDamage;
    public int SpellDamage
    {
        get
        {
            return spellDamage;
        }
        set
        {
            Debug.Log($"Changing SpellDamage from {spellDamage} to {value}");
            spellDamage = value;
        }
    }
    public int spellStun;
    public int spellFreeze;
    public int spellBurn;
    public int spellPoison;
    public int spellCurse;
    public int spellChill;
    public int spellConcuss;
    public bool spellDouse;
    public int spellDPS;
    public float duration;
    public int spellHeal;
    public GameObject vfx;
    public AudioSource source;
    public AudioClip clip;
    public Sprite spellIcon;
    public bool isUpgraded;
    public bool spellSurge;
    public bool hueShift;
    public bool allegiance;
    public SpellType spellType;
    public enum SpellType
    {
        attack, 
        skill
    }
    public SpellTarget spellTarget;
    public enum SpellTarget
    {
        single, 
        all
    }
    public string spellDescription;
    private void OnEnable()
    {
        spellDamage = baseSpellDamage;  // Initialize the private field with the value from the inspector
    }
}