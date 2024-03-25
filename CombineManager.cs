using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;

public class CombineManager : MonoBehaviour
{
    GameManager gm;
    SceneMan sm;
    public Transform parent; // Parent transform for combo UI elements
    public GameObject comboRef; // Prefab for combo UI element
    public GameObject rowRef;
    public GameObject deckSlot0;
    public GameObject deckSlot1;
    public GameObject deckSlot2;
    public GameObject deckSlot3;

    private int cols = 4;
    public List<Spell> list;
    public List<Spell> newList;
    public List<Tuple<Spell, Spell, Spell>> combos = new List<Tuple<Spell, Spell, Spell>>();
    public Spell fireTornado;
    public Spell concentratedPoison;

    void Start()
    {
        gm = FindObjectOfType<GameManager>();
        sm = FindObjectOfType<SceneMan>();
    }

    void Update()
    {
        //gm.PlayerDeck
    }
    public string ShowCost(Spell spell1, Spell spell2)
    {
        return ("Cost: " + spell1.spellName + " + " + spell2.spellName);
    }
    public void ExecuteCombo(Tuple<Spell, Spell, Spell> combo)
    {
        gm.playerDeck.Add(combo.Item1);  // Add the combo spell to the deck
        gm.playerDeck.Remove(combo.Item2);  // Remove the original spells from the deck
        gm.playerDeck.Remove(combo.Item3);
        sm.ObtainedCombo();

    }
    public void SearchForCombos()
    {
        Debug.Log("Searching for combos");
        // Fire Tornado
        list = gm.playerDeck.FindAll(IDtoFind => IDtoFind.spellElement == "Fire");
        newList = gm.playerDeck.FindAll(IDtoFind => IDtoFind.spellName == "Tornado");
        if (list.Any() && newList.Any())
        {
            combos.Add(new Tuple<Spell, Spell, Spell>(fireTornado, list[0], newList[0]));
        }
        list.Clear();
        newList.Clear();
        
        // Concentrated Poison
        list = gm.playerDeck.FindAll(IDtoFind => IDtoFind.spellName == "Poison");
        newList = gm.playerDeck.FindAll(IDtoFind => IDtoFind.spellName == "Poison");
        if (list.Any() && newList.Any())
        {
            combos.Add(new Tuple<Spell, Spell, Spell>(concentratedPoison, list[0], newList[0]));
        }
        list.Clear();
        newList.Clear();

        // Calculate rows and cols like DeckManager
        int rows = (int)Math.Ceiling((combos.Count / (double)cols));
        int comboIndex = 0;

        for (int i = 0; i < rows; i++)
        {
            // Create row for combos to live inside 
            string comboRowName = "comboRow" + i.ToString();
            GameObject comboRow = Instantiate(rowRef);
            comboRow.name = comboRowName;
            comboRow.transform.SetParent(parent, false);
            if (i != 0)
            {
                comboRow.transform.position = new Vector3(comboRow.transform.position.x, comboRow.transform.position.y - 400*i, comboRow.transform.position.z);
            }

            for (int j = 0; j < cols; j++)
            {
                if (comboIndex >= combos.Count)
                {
                    return;
                }
                else
                {
                    string comboName = "spell" + comboIndex.ToString();
                    GameObject comboGO = Instantiate(comboRef);
                    //spellGO.AddComponent<Transform>();
                    // Place the created spell into the created current row
                    comboGO.transform.SetParent(comboRow.transform);
                    // Give it the name spell + index
                    comboGO.name = comboName;
                    SpellUI script = comboGO.GetComponent<SpellUI>();
                    script.spell = combos[comboIndex].Item1;
                    Spell spell = script.spell;
                    script.LoadSpell(script.spell);
                    script.spellCost.text = ShowCost(combos[comboIndex].Item2, combos[comboIndex].Item3);
                    script.spellCost.gameObject.SetActive(true);
                    comboGO.SetActive(true);
                    int tempIndex = comboIndex;
                    // Add combo to deck
                    comboGO.GetComponent<Button>().onClick.AddListener(() => ExecuteCombo(combos[tempIndex]));
                    //Vector3 position = transform.position;
                    GameObject ds0 = comboRow.transform.Find("DeckSlot0").gameObject;
                    GameObject ds1 = comboRow.transform.Find("DeckSlot1").gameObject;
                    GameObject ds2 = comboRow.transform.Find("DeckSlot2").gameObject;
                    GameObject ds3 = comboRow.transform.Find("DeckSlot3").gameObject;
                    switch (j)
                    {
                        case 0:
                            comboGO.transform.position = ds0.transform.position;
                            break;
                        case 1:
                            comboGO.transform.position = ds1.transform.position;
                            break;
                        case 2:
                            comboGO.transform.position = ds2.transform.position;
                            break;
                        case 3:
                            comboGO.transform.position = ds3.transform.position;
                            break;
                    }
                    comboIndex++;
                }
            }
        }
    }
}
