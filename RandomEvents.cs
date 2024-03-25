using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RandomEvents : MonoBehaviour
{
    private int rng;
    private int eventrng;
    public TextMeshProUGUI eventName;
    public TextMeshProUGUI eventDescription;
    public Button button1;
    public Button button2;
    public Button button3;
    public Button button4;
    private Button leaveButton;
    public Spell brimstone;
    public Spell heal;
    public Spell blizzard;
    public Wizard wizard;
    SceneMan sm;
    public GameManager gm;
    private int numOptions = 0;
    public GameObject leaveEvent;
    public TextMeshProUGUI eventOption1;
    public TextMeshProUGUI eventOption2;
    public TextMeshProUGUI eventOption3;
    public TextMeshProUGUI eventOption4;
    public RareSpells rareSpells;
    public GameObject displayDeck;
    DeckManager dm;
    //public bool cardRemoved = false;
    public Spell cardToRemove;
    public string message;

    public void Start()
    {
        sm = FindObjectOfType<SceneMan>();
        dm = FindObjectOfType<DeckManager>();
    }
    public void RemoveButtonListeners()
    {
        if (button1 != null)
        {
            button1.onClick.RemoveAllListeners(); 
        }
        if (button2 != null)
        {
            button2.onClick.RemoveAllListeners(); 
        }
        if (button3 != null)
        {
            button3.onClick.RemoveAllListeners(); 
        }
        if (button4 != null)
        {
            button4.onClick.RemoveAllListeners(); 
        }
    }
    public void RemoveEventListeners()
    {
        if (leaveButton != null)
        {
            leaveButton.onClick.RemoveAllListeners();
        }
        if (leaveEvent != null)
        {
            leaveEvent.SetActive(false);
        }
        if (eventDescription != null)
        {
            eventDescription.gameObject.SetActive(false);
        }
    }
    public void ToggleButtons(int numOptions)
    {
        switch (numOptions)
        {
            case 1:
                button1.gameObject.SetActive(true);
                button2.gameObject.SetActive(false);
                button3.gameObject.SetActive(false);
                button4.gameObject.SetActive(false);
                break;
            case 2:
                button1.gameObject.SetActive(true);
                button2.gameObject.SetActive(true);
                button3.gameObject.SetActive(false);
                button4.gameObject.SetActive(false);
                break;
            case 3:
                button1.gameObject.SetActive(true);
                button2.gameObject.SetActive(true);
                button3.gameObject.SetActive(true);
                button4.gameObject.SetActive(false);
                break;
            case 4:
                button1.gameObject.SetActive(true);
                button2.gameObject.SetActive(true);
                button3.gameObject.SetActive(true);
                button4.gameObject.SetActive(true);
                break;
            default:
                Debug.Log("Error at ToggleButtons()");
                break;
        }
    }
    public void GetRandomEvent()
    {
        cardToRemove = null;
        message = "";
        eventName.gameObject.SetActive(true);
        eventDescription.gameObject.SetActive(true);
        numOptions = 0;
        rng = Random.Range(0,4);
        switch (rng)
        {


            case 0:
                eventName.text = "Necronomicon";
                eventDescription.text = "As you traverse the unpredictable landscape, a curious sight arrests your attention: an ancient tome, its cover eerily adorned with a contorted visage. You hear the tomb calling your name.";
            
                for (int i = 0; i < gm.playerDeck.Count; i++)
                {
                    if (gm.playerDeck[i].spellElement.ToLower() == "fire")
                    {
                        Debug.Log("Fire found");
                        numOptions = 3;
                    }
                }
                if (numOptions != 3)
                {
                    numOptions = 2;
                }
                ToggleButtons(numOptions);

                eventOption1.text = "Read the tomb";
                button1.onClick.AddListener(() =>
                { 
                    AddSpell(brimstone);
                    wizard.TakeDamage(20);
                    Selection("You have received Brimstone. You have lost 20hp.");
                });

                eventOption2.text = "Ignore the tomb";
                button2.onClick.AddListener(() =>
                {
                    Selection("You walk away and nothing happens.");
                });
                
                if (numOptions != 3)
                {
                    return;
                }
                eventOption3.text = "Burn the tomb";
                eventOption3.color = Color.red;
                button3.onClick.AddListener(() =>
                { 
                    AddSpell(heal); 
                    Selection("The tomb screams in agony. A pillar of light appears in front of you. You look closely and see a card. You have received Heal.");
                });     
                break; 


            case 1:
                eventName.text = "Art Gallery";
                eventDescription.text = "There's a mysterious tent on the side of the road. You decide to investigate. You walk up and there's a man at the door. He says \"Art Gallery! Tickets are 30g!\"";

                numOptions = 2;
                ToggleButtons(numOptions);

                eventOption1.text = "Purchase a ticket";
                button1.onClick.AddListener(() =>
                {
                    gm.SubtractGoldNumber(30);
                    button1.onClick.RemoveAllListeners();
                    button2.onClick.RemoveAllListeners(); 
                    eventOption1.text = "Copy a painting";
                    button1.onClick.AddListener(() =>
                    {
                        int i = Random.Range(0, rareSpells.rareSpellList.Count);
                        Spell randomSpell = rareSpells.rareSpellList[i];
                        AddSpell(randomSpell);
                        Selection("You get inspired from a cool painting on the wall. You copy one of the paintings on a blank Index Card. You have received " + randomSpell.spellName + ".");
                    });
                    eventOption2.text = "Display one of your cards";
                    // Then add the listener to your button's onClick event
                    button2.onClick.AddListener(() =>
                    {
                        RemoveCard();
                        message = "There's an empty spot on the wall. You take the liberty of placing one of your cards on the wall. You have lost ";
                        eventName.text = "";
                        eventDescription.text = "";
                        eventOption1.text = "";
                        eventOption2.text = "";
                        // Call selection via dm
                    });
                });
                eventOption2.text = "Leave";
                button2.onClick.AddListener(() =>
                {
                    Selection("You think to yourself \"30g is pretty steep...\" and you walk away.");
                });
                break;


            case 2:
                eventName.text = "Harsh Elements";
                eventDescription.text = "A sudden blizzard emerges. There are no cities nearby, the only form of shelter is a large tree.";
            
                for (int i = 0; i < gm.playerDeck.Count; i++)
                {
                    if (gm.playerDeck[i].spellElement.ToLower() == "fire")
                    {
                        Debug.Log("Fire found");
                        numOptions = 3;
                    }
                }
                if (numOptions != 3)
                {            
                    numOptions = 2;
                }
                ToggleButtons(numOptions);

                eventOption1.text = "Hide under the tree";
                button1.onClick.AddListener(() =>
                {
                    wizard.TakeDamage(15); 
                    Selection("The tree doesn't offer much protection. You lose 15hp.");
                });

                eventOption2.text = "Embrace the cold";
                button2.onClick.AddListener(() =>
                {
                    wizard.TakeDamage(30);
                    AddSpell(blizzard);
                    Selection("You lose 30hp. You gain inspiration from the harsh elements. You have received Blizzard.");
                });
                if (numOptions != 3)
                {
                    return;
                }
                eventOption3.text = "Light a fire";
                eventOption3.color = Color.red;
                button3.onClick.AddListener(() =>
                {
                    wizard.Heal(10); 
                    Selection("You cast a Fire spell and wait out the blizzard. You feel rested. Restored 10 hp.");
                });                        
                break;


            case 3:
                eventName.text = "Rock, Paper, Scissors!";
                eventDescription.text = "A child emerges from a nearby bush. Concerned, you offer to take the child with you to the nearest settlement. The child says \"I'm fine on my own! I bet 50g that I can beat you at Rock, Paper, Scissors!\"";
                    
                numOptions = 2;

                ToggleButtons(numOptions);

                eventOption1.text = "You're on!";
                button1.onClick.AddListener(() =>
                {
                    button1.onClick.RemoveAllListeners();
                    button2.onClick.RemoveAllListeners();
                    numOptions = 3;
                    ToggleButtons(numOptions);
                    eventrng = Random.Range(0,3);
                    string rps = "";
                    switch (eventrng)
                    {
                        case 0:
                            rps = "Rock";
                            break;
                        case 1:
                            rps = "Paper";
                            break;
                        case 2:
                            rps = "Scissors";
                            break;
                        default:
                            Debug.Log("error");
                            break;                                                        
                    }
                    eventDescription.text = "The child says \"Yay! 3... 2... 1.... " + rps + "!\"";
                    eventOption1.text = "Rock!";
                    button1.onClick.AddListener(() =>
                    {
                        if (rps == "Scissors")
                        {
                            gm.AddGoldNumber(50);
                            Selection("The child throws the money at you and runs back to their bush, crying. Do you feel better?");
                        }
                        else if (rps == "Rock")
                        {
                            Selection("Tie! You offer to play again. The child says \"I'm bored. I'm going to watch videos on my tablet now.\"");
                        }
                        else
                        {
                            gm.SubtractGoldNumber(50);
                            Selection("The child takes your money and runs.");
                        }
                    });  
                    eventOption2.text = "Paper!";
                    button2.onClick.AddListener(() =>
                    {
                        if (rps == "Rock")
                        {
                            gm.AddGoldNumber(50);
                            Selection("The child throws the money at you and runs back to their bush, crying. Do you feel better?");
                        }
                        else if (rps == "Paper")
                        {
                            Selection("Tie! You offer to play again. The child says \"I'm bored. I'm going to watch videos on my tablet now.\"");
                        }
                        else
                        {
                            gm.SubtractGoldNumber(50);
                            Selection("The child takes your money and runs.");
                        }
                    }); 
                    eventOption3.text = "Scissors!";
                    button3.onClick.AddListener(() =>
                    {
                        if (rps == "Paper")
                        {
                            gm.AddGoldNumber(50);
                            Selection("The child throws the money at you and runs back to their bush, crying. Do you feel better?");
                        }
                        else if (rps == "Scissors")
                        {
                            Selection("Tie! You offer to play again. The child says \"I'm bored. I'm going to watch videos on my tablet now.\"");
                        }
                        else
                        {
                            gm.SubtractGoldNumber(50);
                            Selection("The child takes your money and runs.");
                        }
                    }); 
                });
                eventOption2.text = "Insist to help the child";
                button2.onClick.AddListener(() =>
                {
                    Selection("\"Now's not the time for games!\" you say. The child, looking disappointed, goes back to their bush.");
                });                      
                break;


            default:
                Debug.Log("There's an error");
                break;
        }
    }
    public void AddSpell(Spell _spell)
    {
        Spell spell = _spell;
        gm.playerDeck.Add(spell);
        return;
    }
    public void Selection(string text)
    {
        // Print the received text
        //Debug.Log("Text received in Selection: " + text);

        // Checking the current text of eventDescription before we overwrite it
        //Debug.Log("eventDescription text before Selection: " + eventDescription.text);

        RemoveButtonListeners();

        eventDescription.text = text + " Press anywhere to continue.";
        
        // Checking the updated text of eventDescription
        //Debug.Log("eventDescription text after Selection: " + eventDescription.text);

        eventName.gameObject.SetActive(false);
        leaveEvent.SetActive(true);
        leaveButton = leaveEvent.GetComponent<Button>();
        leaveButton.onClick.AddListener(() => sm.LeaveEvent());

        switch(numOptions)
        {
            case 1:
                button1.gameObject.SetActive(false);
                break;
            case 2:
                button1.gameObject.SetActive(false);
                button2.gameObject.SetActive(false);
                break;
            case 3:
                button1.gameObject.SetActive(false);
                button2.gameObject.SetActive(false);
                button3.gameObject.SetActive(false);
                break;  
            case 4:
                button1.gameObject.SetActive(false);
                button2.gameObject.SetActive(false);
                button3.gameObject.SetActive(false);
                button4.gameObject.SetActive(false);
                break;                
        }
    }

    public void RemoveCard()
    {
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
            // display the deck and attempt to remove a card
            dm.DisplayDeck("removeatevent", 0);
        }
    }
    public void CardRemoved(Spell spell)
    {
        // Checking the received spell
        //Debug.Log("Spell received in CardRemoved: " + spell.spellName);

        // Checking the message before it's used
        //Debug.Log("Message before combining with spell name: " + message);

        // Creating the full message and printing to console
        string fullMessage = message + spell.spellName.ToString() + ".";
        // Debug.Log("Full message: " + fullMessage);
        
        // Call Selection
        Selection(fullMessage);
    }

}
// Simple template
/*
                eventName.text = "Rock, Paper, Scissors!";
                eventDescription.text = "A child emerges from a nearby bush. Concerned, you offer to take the child with you to the nearest settlement. The child says \"I'm fine on my own! I bet 50g that I can beat you at Rock, Paper, Scissors!\"";
                    
                numOptions = 2;

                ToggleButtons(numOptions);

                eventOption1.text = "You're on!";
                button1.onClick.AddListener(() =>
                {
                    wizard.TakeDamage(15); 
                    Selection("The tree doesn't offer much protection. You lose 15hp.");
                });

                eventOption2.text = "Insist to help the child";
                button2.onClick.AddListener(() =>
                {
                    Selection("\"Now's not the time for games!\" you say. The child, looking disappointed, goes back to their bush.");
                });                      
                break;
*/