using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DeckManager : MonoBehaviour
{
    GameManager gm;
    SceneMan sm;
    public Transform parent;
    public GameObject spellRef;
    public GameObject rowRef;
    //public GameObject deck;
    //public SpellUI spell0;
    private int deckLength;
    private int rows;
    private int cols = 4;
    private int spellIndex = 0;
    private string spellName;
    private string spellRowName;
    public GameObject deckSlot0;
    public GameObject deckSlot1;
    public GameObject deckSlot2;
    public GameObject deckSlot3;
    GameObject deck;
    public RandomEvents randomEvents;
    //PlayerStats playerStats;

    void Start()
    {
        gm = FindObjectOfType<GameManager>();
        sm = FindObjectOfType<SceneMan>();
    }
    void WakeUp()
    {
        gameObject.SetActive(true);
    }

    void Update()
    {
        //gm.PlayerDeck
    }
    public void RemoveSpellFromEvent(Spell spell)
    {
        gm.playerDeck.Remove(spell);
        deck = GameObject.Find("DisplayDeck");
        deck.SetActive(false);
        // Invoke the event after removing the spell
        //randomEvents.cardRemoved = true;
        randomEvents.cardToRemove = spell;
        randomEvents.CardRemoved(spell);
    }
    public void DisplayDeck(string command, int cost)
    {
        Debug.Log(command);
        Debug.Log("Displaying deck");
        //myPosition = comboPosition.gameObject.transform.position;

        deckLength = gm.playerDeck.Count;
        rows = (int)Math.Ceiling((deckLength/(double)cols));
        
        for (int i = 0; i < rows; i++)
        {

            // Create containers for the spells to live inside 
            spellRowName = "spellrow" + i.ToString();
            GameObject spellRow = Instantiate(rowRef);
            spellRow.name = spellRowName;
            spellRow.transform.SetParent(parent, false);
            if (i != 0)
            {
                spellRow.transform.position = new Vector3(spellRow.transform.position.x, spellRow.transform.position.y - 400*i, spellRow.transform.position.z);
            }
            for (int j = 0; j < cols; j++)
            {
                // Don't want to access Player Deck past the index
                if (spellIndex >= gm.playerDeck.Count)
                {
                    return;
                }
                else
                {
                    spellName = "spell" + spellIndex.ToString();
                    GameObject spellGO = Instantiate(spellRef);
                    //spellGO.AddComponent<Transform>();
                    // Place the created spell into the created current row
                    spellGO.transform.SetParent(spellRow.transform);
                    // Give it the name spell + index
                    spellGO.name = spellName;
                    SpellUI script = spellGO.GetComponent<SpellUI>();
                    script.spell = gm.playerDeck[spellIndex]; // Assign spell before accessing it
                    Spell spell = script.spell;
                    script.LoadSpell(script.spell);
                    spellGO.SetActive(true);
                    // Remove a spell from the deck
                    if (command == "removeatshop")
                    {
                        //button1.onClick.AddListener(() => PurchaseSpell(card1.spell, card1.gameObject, cardcost1));
                        Button button = spellGO.gameObject.GetComponent<Button>();
                        //button.onClick.AddListener(script.RemoveSpellAtShop);
                        button.onClick.AddListener(() => script.RemoveSpellAtShop(cost));
                        //button.onClick.AddListener(deck.SetActive(false));
                        // I'm instead setting it false in SpellUI.cs
                    }
                    if (command == "removeatevent")
                    {
                        Button button = spellGO.gameObject.GetComponent<Button>();
                        button.onClick.AddListener(() => RemoveSpellFromEvent(spell));
                    }

                    //Vector3 position = transform.position;
                    GameObject ds0 = spellRow.transform.Find("DeckSlot0").gameObject;
                    GameObject ds1 = spellRow.transform.Find("DeckSlot1").gameObject;
                    GameObject ds2 = spellRow.transform.Find("DeckSlot2").gameObject;
                    GameObject ds3 = spellRow.transform.Find("DeckSlot3").gameObject;
                    switch (j)
                    {
                        case 0:
                            spellGO.transform.position = ds0.transform.position;
                            break;
                        case 1:
                            spellGO.transform.position = ds1.transform.position;
                            break;
                        case 2:
                            spellGO.transform.position = ds2.transform.position;
                            break;
                        case 3:
                            spellGO.transform.position = ds3.transform.position;
                            break;
                    }
                    spellIndex++;
                }
            }
        }
    }
}
