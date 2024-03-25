using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
	public Wizard wizard;
	RegularRelics regularRelics;
    public PlayerStats playerStats;
	public List<Spell> playerDeck = new List<Spell>();
	public int goldAmount;
	public List<Relic> relics = new List<Relic>();
	public List<Relic> relicLibrary = new List<Relic>();
	private int counter;
	private string relicToRemove;
	public BattleManager bm;
	public RegularSpells regularSpells;
	//private bool visited = false;
	private void Awake()
	{
		regularRelics = FindObjectOfType<RegularRelics>();
		AddGoldNumber(100);
	}

	public void AddGoldNumber(int newGold)
	{
		goldAmount += newGold;
		playerStats.gold.text = "Gold: " + goldAmount.ToString();
	}
	public void SubtractGoldNumber(int newGold)
	{
		goldAmount -= newGold;
		playerStats.gold.text = "Gold: " + goldAmount.ToString();
	}
    public void ActivateRelic()
    {
		switch (relics[counter].name)
		{
			case "Pencil":
				bm.pencil = true;
				relicToRemove = "Pencil";
				break;
			case "Donut":
				wizard.AddHealth(15);
				relicToRemove = "Donut";
				break;

			case "Takoyaki":
				wizard.AddHealth(30);
				relicToRemove = "Takoyaki";
				break;

			case "Boba":
				wizard.AddHealth(50);
				relicToRemove = "Boba";
				break;

			case "Bandage":
				bm.bandage = true;
				relicToRemove = "Bandage";
				break;

			case "Fire Mastery":
				for (int i = 0; i < regularSpells.regSpellList.Count; i++)
				{
					if (regularSpells.regSpellList[i].spellElement.ToString().ToLower() == "fire")
					{
						regularSpells.regSpellList[i].SpellDamage += 5;
						regularSpells.regSpellList[i].spellBurn += 1;
					}
				}
				break;
			case "Water Mastery":
				for (int i = 0; i < regularSpells.regSpellList.Count; i++)
				{
					if (regularSpells.regSpellList[i].spellElement.ToString().ToLower() == "water")
					{
						regularSpells.regSpellList[i].SpellDamage += 5;
						regularSpells.regSpellList[i].spellBurn += 1;
					}
				}
				break;
			case "Earth Mastery":
				for (int i = 0; i < regularSpells.regSpellList.Count; i++)
				{
					if (regularSpells.regSpellList[i].spellElement.ToString().ToLower() == "earth")
					{
						regularSpells.regSpellList[i].SpellDamage += 5;
						regularSpells.regSpellList[i].spellBurn += 1;
					}
				}
				break;
			case "Wind Mastery":
				for (int i = 0; i < regularSpells.regSpellList.Count; i++)
				{
					if (regularSpells.regSpellList[i].spellElement.ToString().ToLower() == "wind")
					{
						regularSpells.regSpellList[i].SpellDamage += 5;
						regularSpells.regSpellList[i].spellBurn += 1;
					}
				}
				break;
			/*
			case "Poison Mastery":
				for (int i = 0; i < playerDeck.Count; i++)
				{
					if (playerDeck[i].spellElement.ToString().ToLower() == "poison" && playerDeck[i].spellPoison != 0)
					{
						playerDeck[i].spellPoison += 2;
					}
				}
				break;
			*/
			case "Thunder Mastery":
				for (int i = 0; i < regularSpells.regSpellList.Count; i++)
				{
					if (regularSpells.regSpellList[i].spellElement.ToString().ToLower() == "thunder")
					{
						regularSpells.regSpellList[i].SpellDamage += 5;
						regularSpells.regSpellList[i].spellStun += 1;
					}
				}
				break;

			default:
				break;
		}
		regularRelics.regRelicList.RemoveAll(p => p.relicName == relicToRemove);
		// To do: Some type of Breakfast thing
		counter++;
    }

	/*
    private void EnableRelics()
    {
        if (relics.Count > 0)
        {
            for (int i = 0; i < relics.Count; i++)
            {
                switch (relics[i].name)
                {
                    case "Takoyaki":
                        wizard.healthAmount += 30;
                        wizard.totalHealth += 30;
                        break;
                    default:
                        Debug.Log(relics[i]);
                        break;
                }
            }
        }
    }
	*/
}
