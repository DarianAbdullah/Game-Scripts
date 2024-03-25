using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;

public class Shop : MonoBehaviour
{
    [SerializeField] RegularSpells regularSpells;
    [SerializeField] SpellUI card1;
    [SerializeField] SpellUI card2;
    [SerializeField] SpellUI card3;
    [SerializeField] RegularRelics regularRelics;
    [SerializeField] RegularRelic relic1;
    [SerializeField] RegularRelic relic2;
    public GameObject removeCard;
    public GameObject displayDeck;
    DeckManager dm;
    GameManager gm;
    private List<int> randomNumbers = new List<int>();
    private int rng;
    private int sizeofspells;
    private int sizeofrelics;
    private int cardcost1;
    private int cardcost2;
    private int cardcost3;
    private int reliccost1;
    private int reliccost2;
    public Button button1;
    public Button button2;
    public Button button3;
    public Button relicbutton1;
    public Button relicbutton2;
    public TextMeshProUGUI cardcost1text;
    public TextMeshProUGUI cardcost2text;
    public TextMeshProUGUI cardcost3text;

    void Start()
    {
        dm = FindObjectOfType<DeckManager>();
        gm = FindObjectOfType<GameManager>();
        cardcost1 = 10;
        cardcost2 = 20;
        cardcost3 = 30;
        reliccost1 = 25;
        reliccost2 = 25;
        cardcost1text.text = "Price: " + cardcost1.ToString() + " gold";
        cardcost2text.text = "Price: " + cardcost2.ToString() + " gold";
        cardcost3text.text = "Price: " + cardcost3.ToString() + " gold";
    }
    public void GetShop()
    {
        // Re-activate the remove card button that is disabled on button click
        removeCard.SetActive(true);
        card1.gameObject.SetActive(true);
        card2.gameObject.SetActive(true);
        card3.gameObject.SetActive(true);
        relic1.gameObject.SetActive(true);
        relic2.gameObject.SetActive(true);

        button1 = card1.gameObject.GetComponent<Button>();
        button2 = card2.gameObject.GetComponent<Button>();
        button3 = card3.gameObject.GetComponent<Button>();
        relicbutton1 = relic1.gameObject.GetComponent<Button>();
        relicbutton2 = relic2.gameObject.GetComponent<Button>();
        sizeofspells = regularSpells.regSpellList.Count;

        // Generate a list of sequential integers from 0 to regularSpells.regSpellList.Count - 1
        List<int> numbers = Enumerable.Range(0, regularSpells.regSpellList.Count).ToList();
        // Randomly shuffle the list
        System.Random rand = new System.Random();
        numbers = numbers.OrderBy(x => rand.Next()).ToList();
        // Take the first three numbers from the shuffled list
        List<int> randomNumbers = numbers.Take(3).ToList();
        card1.LoadSpell(regularSpells.regSpellList[randomNumbers[0]]);
        card2.LoadSpell(regularSpells.regSpellList[randomNumbers[1]]);
        card3.LoadSpell(regularSpells.regSpellList[randomNumbers[2]]);
        button1.onClick.AddListener(() => PurchaseSpell(card1.spell, card1.gameObject, cardcost1));
        button2.onClick.AddListener(() => PurchaseSpell(card2.spell, card2.gameObject, cardcost2));
        button3.onClick.AddListener(() => PurchaseSpell(card3.spell, card3.gameObject, cardcost3));
        
        sizeofrelics = regularRelics.regRelicList.Count;
        // Generate a list of sequential integers from 0 to sizeofrelics - 1
        List<int> relicNumbers = Enumerable.Range(0, sizeofrelics).ToList();
        // Randomly shuffle the list
        relicNumbers = relicNumbers.OrderBy(x => rand.Next()).ToList();
        // Take the first two numbers from the shuffled list
        List<int> relicRandomNumbers = relicNumbers.Take(2).ToList();
        relic1.LoadRelic(regularRelics.regRelicList[relicRandomNumbers[0]]);
        relic2.LoadRelic(regularRelics.regRelicList[relicRandomNumbers[1]]);

        // Clear or else inf loop
        numbers.Clear();
        randomNumbers.Clear();

        relicbutton1.onClick.AddListener(() => PurchaseRelic(relic1.relic, relic1.gameObject, reliccost1));
        relicbutton2.onClick.AddListener(() => PurchaseRelic(relic2.relic, relic2.gameObject, reliccost2));
    }
    public void PurchaseSpell(Spell _spell, GameObject card, int cost)
    {
        Spell spell = _spell;
        Debug.Log(spell);
        if (gm.goldAmount >= cost)
        {
            gm.playerDeck.Add(spell);
            gm.SubtractGoldNumber(cost);
            card.SetActive(false);
        }
        return;
    }
    public void PurchaseRelic(Relic relic, GameObject relicGO, int cost)
    {
        if (gm.goldAmount >= cost)
        {
            gm.relics.Add(relic);
            gm.ActivateRelic();
            gm.SubtractGoldNumber(cost);
            relicGO.SetActive(false);
        }
        return;
    }
    public void RemoveShopListeners()
    {
        button1.onClick.RemoveAllListeners();
        button2.onClick.RemoveAllListeners();
        button3.onClick.RemoveAllListeners();
        relicbutton1.onClick.RemoveAllListeners();
        relicbutton2.onClick.RemoveAllListeners();
    }
    public void RemoveCard()
    {
        if (gm.goldAmount >= 50)
        {
            if (removeCard == null)
            {
                Debug.LogError("removeCard is null");
            }
            else
            {
                removeCard.SetActive(false);
            }

            if (displayDeck == null)
            {
                Debug.LogError("displayDeck is null");
            }
            else
            {
                displayDeck.SetActive(true);
            }
            
            if (dm == null)
            {
                Debug.LogError("DeckManager is null");
            }
            else
            {
                dm.DisplayDeck("removeatshop", 50);
            }
            
            if (this.gameObject == null)
            {
                Debug.LogError("gameObject is null");
            }
            else
            {
                this.gameObject.SetActive(false);
            }
        }
    }
}
