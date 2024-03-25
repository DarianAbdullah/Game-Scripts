using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
//using System;

public class SpellUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Spell spell;
	public TMP_Text spellTitle;
    public TMP_Text spellDescription;
    public TMP_Text effectDescription;
    public TMP_Text burnDescription;
    public TMP_Text spellCost;
    public Image spellIcon;
    public bool isBurned = false;
    //public GameObject discardEffect;
    BattleManager battleManager;
    GameManager gm;
    GameObject deck;
    GameObject shopParent;
    SceneMan sm;
    //Animator animator;
    //public Action onSpellRemoved;

    public GameObject tooltip;
    public GameObject tooltipBG;

    void Start()
    {
        Button button = gameObject.GetComponent<Button>();
        //shop = GameObject.Find("Shop");
    }
    private void Awake()
    {
        battleManager = FindObjectOfType<BattleManager>();
        gm = FindObjectOfType<GameManager>();
        sm = FindObjectOfType<SceneMan>();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (effectDescription.text != "") 
        {
            // Enable the tooltip and set the description based on the card's keywords
            tooltip.SetActive(true);
            tooltipBG.SetActive(true);
            TextMeshProUGUI tmpComponent = tooltip.GetComponent<TextMeshProUGUI>();
            tmpComponent.text = effectDescription.text;

            // Update size of the RectTransform
            RectTransform rectTransform = tooltip.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, tmpComponent.preferredHeight);
        }
    }
    public void AddSpell()
    {
        gm.playerDeck.Add(spell);
        sm.ObtainedReward();
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        // Hide the tooltip
        tooltip.SetActive(false);
        tooltipBG.SetActive(false);
    }
    public string GetSpellDescription(Spell spell)
    {
        spell.spellDescription = "";
        if (spell.SpellDamage != 0)
        {
            if (spell.spellTarget == Spell.SpellTarget.all)
            {
                spell.spellDescription += "Deal " + spell.SpellDamage.ToString() + " " + spell.spellElement + " damage to all enemies. ";
            }
            else
            {
                spell.spellDescription += "Deal " + spell.SpellDamage.ToString() + " " + spell.spellElement + " damage. ";
            }
        }
        if (spell.spellBurn != 0)
        {
            spell.spellDescription += "Burn " + spell.spellBurn.ToString() + ". ";
        }
        if (spell.spellStun != 0)
        {
            spell.spellDescription += "Stun " + spell.spellStun.ToString() + ". ";
        }
        if (spell.spellFreeze != 0)
        {
            spell.spellDescription += "Freeze " + spell.spellFreeze.ToString() + ". ";
        }
        if (spell.spellCurse != 0)
        {
            spell.spellDescription += "Curse " + spell.spellCurse.ToString() + ". ";
        }
        if (spell.spellPoison != 0)
        {
            spell.spellDescription += "Poison " + spell.spellPoison.ToString() + ". ";
        }
        if (spell.spellConcuss != 0)
        {
            spell.spellDescription += "Concuss " + spell.spellConcuss.ToString() + ". ";
        }
        if (spell.spellChill != 0)
        {
            spell.spellDescription += "Chill " + spell.spellChill.ToString() + ". ";
        }
        if (spell.spellHeal != 0)
        {
            spell.spellDescription += "Heal self " + spell.spellHeal.ToString() + ". ";
        }
        if (spell.spellDouse == true)
        {
            spell.spellDescription += "Douse. ";
        }
        if (spell.hueShift == true)
        {
            spell.spellDescription += "Hue Shift. ";
        }
        if (spell.allegiance == true)
        {
            spell.spellDescription += "Allegiance. ";
        }
        if (spell.spellName == "Contagion")
        {
            spell.spellDescription = "Poison " + (spell.spellPoison+2).ToString() + " to target and Poison " + spell.spellPoison.ToString() + " to adjacent enemies.";
        }
        if (spell.spellName.Contains("Magnitude"))
        {
            string[] parts = spell.spellName.Split(' ');
            // Get the number portion only
            string numberPart = parts[1];
            // Increment magnitude
            int magNumber = int.Parse(numberPart) + 1;
            // Want to append to description instead of replace.
            spell.spellDescription += "Destroy. Create a Magnitude " + magNumber + " in your Discard pile.";
        }
        if (spell.spellName == "Doublecast")
        {
            spell.spellDescription = "The next spell will be Casted twice.";
        }
        if (spell.spellDPS != 0 && spell.duration != 0 && spell.spellTarget == Spell.SpellTarget.all)
        {
            spell.spellDescription += "Deal " + spell.spellDPS + " " + spell.spellElement + " damage per second to all enemies for " + spell.duration + " seconds.";
        }
        if (spell.spellDPS != 0 && spell.duration != 0 && spell.spellTarget == Spell.SpellTarget.all)
        {
            spell.spellDescription += "Deal " + spell.spellDPS + " " + spell.spellElement + " damage per second for " + spell.duration + " seconds.";
        }
        if (spell.spellName == "Chain Lightning")
        {
            spell.spellDescription = "Deal " + spell.SpellDamage + " " + spell.spellElement + " damage. This can bounce to a random adjacent enemy up to 4 times.";
        }
        if (spell.spellName == "Chronostasis")
        {
            spell.spellDescription += "Time doesn't move until your next spell is Casted.";
        }
        if (spell.spellName == "Hiatus")
        {
            spell.spellDescription = "Stop time for 7 real-time seconds.";
        }
        if (spell.spellName == "Doodle Dividend")
        {
            spell.spellDescription += "If this kills, gain 25 gold.";
        }
        if (spell.spellName == "Vital Vengeance")
        {
            spell.spellDescription += "If this kills, gain 5 max health.";
        }
        if (spell.spellName == "Amplifying Arcane")
        {
            spell.spellDescription += "If this kills, grant all copies of Amplifying Arcane everywhere 2 damage.";
        }
        if (spell.spellName == "Thunderstorm")
        {
            spell.spellDescription = "Deal " + spell.spellDPS + " " + spell.spellElement + " damage per second to a random enemy for " + spell.duration + " seconds.";
        }

        return spell.spellDescription;
    }
    public string GetEffectDescription(Spell spell)
    {
        string newEffectDescription = "";

        if (spell.spellDescription.Contains("Stun"))
        {
            newEffectDescription += "Stun: Halts enemy actions for X seconds, decreasing by 1 each second.\n";
        }
        if (spell.spellDescription.Contains("Freeze"))
        {
            newEffectDescription += "Freeze: Halts enemy actions for X seconds, decreasing by 1 each second and by 3 every time it receives damage.\n";
        }
        if (spell.spellDescription.Contains("Curse"))
        {
            newEffectDescription += "Curse: Deals X-2 damage per second.\n";
        }
        if (spell.spellDescription.Contains("Burn"))
        {
            newEffectDescription += "Burn: Deals X damage per second, decreasing by 1 each second. Has a 20% chance to spread to a random adjacent enemy every second.\n";
        }
        if (spell.spellDescription.Contains("Poison"))
        {
            newEffectDescription += "Poison: Deals X damage per second, decreasing by 1 each second. Poisoned enemies deal 25% less damage.\n";
        }         
        if (spell.spellDescription.Contains("Concuss"))
        {
            newEffectDescription += "Concuss: Concussed enemies have a 20% chance to miss their action for X seconds, decreasing by 1 each second. For every stack of Concuss, increase the chance to miss by 1%.\n";
        }   
        if (spell.spellDescription.Contains("Chill"))
        {
            newEffectDescription += "Chill: Reduce attack speed by 25% for X seconds, decreasing by 1 each second.\n";
        }
        if (spell.spellDescription.Contains("Stumble"))
        {
            newEffectDescription += "Stumble: 25% chance to instantly reset the targets action bar.\n";
        }   
        if (spell.spellDescription.Contains("Douse"))
        {
            newEffectDescription += "Douse: If an adjacent enemy becomes Poisoned or Stunned, apply that effect to me and lose Douse.\n";
        }
        if (spell.spellDescription.Contains("Alignment"))
        {
            newEffectDescription += "Alignment: If the top card of your deck is the same element as this card, deal double damage.\n";
        }
        if (spell.spellDescription.Contains("Instacast"))
        {
            newEffectDescription += "Instacast: This card is casted immediately if the conditions are met.\n";
        }
        if (spell.spellDescription.Contains("Banish"))
        {
            newEffectDescription += "Banish: Move this card to the Void.\n";
        }
        if (spell.spellDescription.Contains("Surge"))
        {
            newEffectDescription += "Surge: Increase your base damage by 25%, rounded up.\n";
        }
        if (spell.spellDescription.Contains("Hue Shift"))
        {
            newEffectDescription += "Hue Shift: The element is determined by the color you draw.\n";
        }
        if (spell.spellDescription.Contains("Allegiance"))
        {
            newEffectDescription += "Allegiance: If the element matches the top card of your deck, deal 2x damage.\n";
        }
        return newEffectDescription;
    }
    public void LoadSpell(Spell _spell)
    {
        spell = _spell;
        gameObject.GetComponent<RectTransform>().localScale=new Vector3(1,1,1);
        spellTitle.text = spell.spellName;
        spellIcon.sprite = spell.spellIcon;
        spellDescription.text = GetSpellDescription(spell);
        effectDescription.text = GetEffectDescription(spell);
        switch (spell.spellElement)
        {
            case "Normal":
                // Off black
                spellIcon.GetComponent<Image>().color = new Color32(27,27,27,255);
                break;
            case "Wind":
                // Gray
                spellIcon.GetComponent<Image>().color = new Color32(189,185,183,255);
                break; 
            case "Earth":
                // Green
                spellIcon.GetComponent<Image>().color = new Color32(102,186,122,255);
                break;      
            case "Thunder":
                // Yellow
                spellIcon.GetComponent<Image>().color = new Color32(255,236,153,255);
                break;  
            case "Fire":
                // Red
                spellIcon.GetComponent<Image>().color = new Color32(227,94,84,255);
                break;
            case "Water":
                // Red
                spellIcon.GetComponent<Image>().color = new Color32(148,204,251,255);
                break;
            default:
                Debug.Log("No color selected");
                spellIcon.GetComponent<Image>().color = new Color32(27,27,27,255);
                break;
        }
    }
    public void UnloadSpell(Spell _spell)
    {
        spell = _spell;
        gameObject.GetComponent<RectTransform>().localScale=new Vector3(1,1,1);
        spellTitle.text = "";
        spellIcon.sprite = null;
        spellDescription.text = "";
        effectDescription.text = "";
        spellIcon.GetComponent<Image>().color = new Color32(0,0,0,0);
    }
    public void RemoveSpell()
    {
        gm.playerDeck.Remove(spell);
    }
    // Moving to DeckManager
    /*
    public void RemoveSpellFromEvent()
    {
        gm.playerDeck.Remove(spell);
        deck = GameObject.Find("DisplayDeck");
        deck.SetActive(false);
        // Invoke the event after removing the spell
        onSpellRemoved?.Invoke(); // ?. checks if onSpellRemoved is not null
        onSpellRemoved = null; // Reset the delegate
    }
    */
    public void RemoveSpellAtShop(int cost)
    {
        // Cannot activate an inactive object because we deactive it from the square gameobject. Instead, get the parent
        // and then set the child to active.
        shopParent = GameObject.Find("ShopParent");
        GameObject shop = shopParent.transform.Find("Shop").gameObject;
        deck = GameObject.Find("DisplayDeck");
        gm.SubtractGoldNumber(cost);
        deck.SetActive(false);
        gm.playerDeck.Remove(spell);
        shop.SetActive(true);
    }
    public void BurnCard()
    {
        this.GetComponent<Image>().color = new Color32(255,0,0,255);
        isBurned = true;
        burnDescription.gameObject.SetActive(true);
        StartCoroutine(StartUnburnTimer());
    }
    public void Unburned()
    {
        this.GetComponent<Image>().color = new Color32(255,255,255,255);
        isBurned = false;  
        burnDescription.gameObject.SetActive(false);      
    }
    private IEnumerator StartUnburnTimer()
    {
        yield return new WaitForSeconds(3f);
        Unburned();
    }
    //spellCanvas.GetComponent<SpriteRenderer>().color = new Color32(255,255,255,255);
}
