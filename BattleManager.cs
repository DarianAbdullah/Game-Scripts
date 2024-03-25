using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class BattleManager : MonoBehaviour
{
    public Demo demo_script;
    GameManager gm;
    SceneMan sm;
    public Enemies enemyTemplate;
    RegularEnemies regEnemies;
    EliteEnemies eliteEnemies;
    RegularSpells regSpells;
    public Wizard wizard;
    public SpellUI spellUI1;
    public SpellUI spellUI2;
    public SpellUI spellUI3;
    public List<SpellUI> spellUIGO = new List<SpellUI>();
    [SerializeField] List<SpellUI> spellsInHand = new List<SpellUI>();
    public SpellActions spellActions;
    public TMP_Text deckSizeText;
    public TMP_Text discardPileSizeText;
    [Header("Enemies")]
    public List<Enemies> enemies = new List<Enemies>();
    Enemies enemy1;
    Enemies enemy2;
    private bool timeStop = false;

    [Header("Spells")]
    public List<Spell> deck;
    //public TextMeshProUGUI deckSizeText;
	public List<Spell> discardPile = new List<Spell>();
	//public TextMeshProUGUI discardPileSizeText;
    public List<Spell> hand = new List<Spell>();
    private List<string> spellType = new List<string>();

    [Header("UI")]
	public Transform[] spellSlots;
    public Transform[] enemySlots;
    public GameObject cursor;
    private float cursorYPos = 1000;

    public bool takeDamage = false;
    private int attackDamage = 0;

    public int cursorIndex = 0;
    Vector3 cursorPos;
    private int number;
    private int number2;
    private bool isDead = false;
    //public int goldDrop;
    public bool allDead = false;
    public bool battleStart = false;
    private bool card1 = false;
    private bool card2 = false;
    private bool card3 = false;
    private int index = 5;
    private Spell empty;
    private bool hand0empty = false;
    private bool hand1empty = false;
    private bool hand2empty = false;
    public bool elite = false;
    private bool doublecast = false;
    private bool surge = false;
    public float timeMultiplier;

    public string elementType;
    public bool mismatchColor;

    private Material brushMaterial;

    [Header("Stickers")]
    [HideInInspector]
    public bool bandage;
    [HideInInspector]
    public bool pencil;
    private int pencilCounter;

    private void Awake() 
    {
        bandage = false;
        pencil = false;
        // If adding/removing spell slots, use the BattleManager inspector Spells in Hand
        spellUIGO.Add(spellUI1);
        spellUIGO.Add(spellUI2);
        spellUIGO.Add(spellUI3);
        spellsInHand.Add(spellUI1);
        spellsInHand.Add(spellUI2);
        spellsInHand.Add(spellUI3);
        //demo_script = FindObjectOfType<Demo>();
        gm = FindObjectOfType<GameManager>();
        sm = FindObjectOfType<SceneMan>();
        regEnemies = FindObjectOfType<RegularEnemies>();
        eliteEnemies = FindObjectOfType<EliteEnemies>();
        regSpells = FindObjectOfType<RegularSpells>();
    }
    // This is also being called in SceneMan via bm.Shuffle()
    public void DrawSpell() 
    {
        if (index == 0 && deck.Count == 0)
        {
            hand.RemoveAt(index);
            hand.Insert(index, empty);
            hand0empty = true;
        }     
        if (index == 1 && deck.Count == 0)
        {
            hand.RemoveAt(index);
            hand.Insert(index, empty);
            hand1empty = true;
        }       
        if (index == 2 && deck.Count == 0)
        {
            hand.RemoveAt(index);
            hand.Insert(index, empty);
            hand2empty = true;
        }  
        if (card1 == false && deck.Count >= 1) 
        {
            Spell randSpell = deck[Random.Range(0, deck.Count)];
            // Post shuffling, slots empty
            if (hand.Count == 0)
            {
                hand.Add(randSpell);
            }
            // When a card is in the slot we need to replace it 
            else if (index == 0 && deck.Count >= 1)
            {
                hand.RemoveAt(index);
                hand.Insert(index, randSpell);
            }
            else
            {
                hand.Add(randSpell);
            }
            DisplayCard1(randSpell);
            deck.Remove(randSpell);
            deckSizeText.text = "Deck: " + deck.Count;
        } 
        if (card2 == false && deck.Count >= 1) 
        {
            Spell randSpell = deck[Random.Range(0, deck.Count)];
            if (hand.Count == 0)
            {
                hand.Add(randSpell);
            }
            else if (index == 1 && deck.Count >= 1)
            {
                hand.RemoveAt(index);
                hand.Insert(index, randSpell);
            }
            else
            {
                hand.Add(randSpell);
            }
            DisplayCard2(randSpell);
            deck.Remove(randSpell);
            deckSizeText.text = "Deck: " + deck.Count;
        } 
        if (card3 == false && deck.Count >= 1) 
        {
            Spell randSpell = deck[Random.Range(0, deck.Count)];
            if (hand.Count == 0)
            {
                hand.Add(randSpell);
            }
            else if (index == 2 && deck.Count >= 1)
            {
                hand.RemoveAt(index);
                hand.Insert(index, randSpell);
            }
            else
            {
                hand.Add(randSpell);
            }
            DisplayCard3(randSpell);
            deck.Remove(randSpell);
            deckSizeText.text = "Deck: " + deck.Count;
        }   
        // Fixes bug of not shuffling if hand size = 1 
        if (hand.Count == 1)
        {
            hand1empty = true;
            hand2empty = true;
        }
        // Fixes bug of not shuffling if hand size = 2
        else if (hand.Count == 2)
        {
            hand2empty = true;
        }        
        if (deck.Count == 0 && hand0empty == true && hand1empty == true && hand2empty == true)
        {
            hand0empty = false;
            hand1empty = false;
            hand2empty = false;
            index = 5;
            hand.Clear();
            Shuffle();
        }
        return;
    }
    public void DisplayCard1(Spell spell)
    {
        spellUI1.LoadSpell(spell);
        spellUI1.gameObject.SetActive(true);
        card1 = true;
    }
    public void DisplayCard2(Spell spell)
    {
        spellUI2.LoadSpell(spell);
        spellUI2.gameObject.SetActive(true);
        card2 = true;
    }
    public void DisplayCard3(Spell spell)
    {
        spellUI3.LoadSpell(spell);
        spellUI3.gameObject.SetActive(true);
        card3 = true;
    }
    public void Shuffle()
	{
        foreach (Spell spell in discardPile)
        {
            deck.Add(spell);
            deckSizeText.text = "Deck: " + deck.Count;
        }
        discardPile.Clear();
        discardPileSizeText.text = "Discard: " + discardPile.Count;
        DrawSpell();
	}
    public void SpawnEnemy()
    {    
        number = Random.Range(0, 10);
        number2 = Random.Range(0, 10);
        switch (number)
        {
            // Don't forget to add enemies to RegularEnemies
            case int n when (n <= 3):
                enemyTemplate.enemyData = regEnemies.regEnemyList[0];
                enemy1 = GameObject.Instantiate(enemyTemplate);
                enemies.Add(enemy1);
                break; 

            case int n when (n > 3 && n < 6):
                enemyTemplate.enemyData = regEnemies.regEnemyList[1];
                enemy1 = GameObject.Instantiate(enemyTemplate);
                enemies.Add(enemy1);
                break;       

            case int n when (n >= 6):
                enemyTemplate.enemyData = regEnemies.regEnemyList[2];
                enemy1 = GameObject.Instantiate(enemyTemplate);
                enemies.Add(enemy1);
                break;             

            default:
                Debug.LogError("No valid enemy type for number: " + number);
                break;
        }
        switch (number2)
        {
            case int n when (n <= 3):
                enemyTemplate.enemyData = regEnemies.regEnemyList[0];
                enemy2 = GameObject.Instantiate(enemyTemplate);
                enemies.Add(enemy2);
                break;
            case int n when (n > 3 && n < 6):
                enemyTemplate.enemyData = regEnemies.regEnemyList[1];
                enemy2 = GameObject.Instantiate(enemyTemplate);
                enemies.Add(enemy2);
                break;       
            case int n when (n >= 6):
                enemyTemplate.enemyData = regEnemies.regEnemyList[2];
                enemy2 = GameObject.Instantiate(enemyTemplate);
                enemies.Add(enemy2);
                break;                     
        }
        enemy1.transform.position = enemySlots[0].position;  
        enemy2.transform.position = enemySlots[1].position;  
        // Need to set active because it's inactive by default
        enemy1.gameObject.SetActive(true);
        enemy2.gameObject.SetActive(true);
        enemy1.LoadData();
        enemy2.LoadData();
        battleStart = true;
    }
    public void SpawnElite()
    {    
        enemyTemplate.enemyData = eliteEnemies.eliteEnemyList[0];
        enemy1 = GameObject.Instantiate(enemyTemplate);
        enemies.Add(enemy1);
        enemy1.transform.position = enemySlots[0].position;  
        elite = true;
        enemy1.gameObject.SetActive(true);
        enemy1.LoadData();
        battleStart = true;
    }
    public void BeginBattle(string enemyType) 
    {   
        demo_script.GetGestures(gm.playerDeck);
        // Cards come from SceneMan 
        Shuffle();
        // Spawning comes from SM
        cursor.gameObject.SetActive(true);
        cursorPos = new Vector3(enemySlots[0].position.x, cursorYPos, 0.0f);
        cursor.transform.position = cursorPos;
        spellUI1.gameObject.SetActive(true);
        spellUI2.gameObject.SetActive(true);
        spellUI3.gameObject.SetActive(true);
        if (enemyType == "regular")
        {
            SpawnEnemy();
        }
        else if (enemyType == "elite")
        {
            SpawnElite();
        }
        return;
    }
    void DestroyClones()
    {
        // Find objects with "Clone" in their name and without the tag "Map".
        var objectsToDestroy = FindObjectsOfType<GameObject>().Where(obj => !obj.tag.Equals("Map")).Where(obj => obj.name.Contains("Clone"));
        foreach (var obj in objectsToDestroy)
        {
            // Destroy the object
            Destroy(obj);
        }
    }
    public void ClearData() 
    {   
        pencilCounter = 0;
        DestroyClones();
        allDead = false;
        battleStart = false;
        cursor.gameObject.SetActive(false);
        deck.Clear();
        deckSizeText.text = "Deck: " + deck.Count;
        hand.Clear();
        discardPile.Clear();
        discardPileSizeText.text = "Discard: " + discardPile.Count;
        for (int i = 0; i < enemies.Count; i ++)
        {
            gm.AddGoldNumber(enemies[i].goldDrop);
        }
        cursor.gameObject.SetActive(false);
        spellUI1.gameObject.SetActive(false);
        spellUI2.gameObject.SetActive(false);
        spellUI3.gameObject.SetActive(false);

        cursorIndex = 0;
        index = 5;
        card1 = false;
        card2 = false;
        card3 = false;
        // CLEAR ENEMIES LIST OR IT WON'T WORK
        enemies= new List<Enemies>();
        if (elite == true)
        {
            Debug.Log("Elite slain");
            sm.EliteEnd();
        }
        else 
        {
            Debug.Log("Normal enemies slain");
            sm.BattleEnd();
        }
    }
    // For when we want to break out of update but still want to do the discard effects
    private void AddToDiscard(int i)
    {
        discardPile.Add(hand[i]);
        discardPileSizeText.text = "Discard: " + discardPile.Count;
        index = i;
        switch (i)
        {
            case 0:
                card1 = false;
                //Spell spellUI1 = spellsInHand[0];
                spellUI1.UnloadSpell(hand[i]);
                break;
            case 1:
                card2 = false;
                //SpellUI spellUI2 = spellsInHand[1];
                spellUI2.UnloadSpell(hand[i]);
                break;
            case 2:
                card3 = false;
                //SpellUI spellUI3 = spellsInHand[2];
                spellUI3.UnloadSpell(hand[i]);
                break;
        }
        DrawSpell(); 
    }
    public void Hiatus()
    {
        StartCoroutine(StopTimeForSeconds(7));
    }
    private IEnumerator StopTimeForSeconds(int seconds)
    {
        yield return new WaitForSecondsRealtime(seconds);
        for (int x = 0; x < enemies.Count; x++)
        {
            enemies[x].hiatus = false;
        }
    }
    private IEnumerator WaitAndDoSomething(float seconds)
    {
        float elapsedTime = 0f;

        while (elapsedTime < seconds)
        {
            elapsedTime += Time.deltaTime * timeMultiplier;  // Use Time.deltaTime to count up
            yield return null;  // Wait for the next frame
        }
    }
    private void Update()
    {
        //spellUI1.GetComponent<RectTransform>().localScale = new Vector3(0.3403766f, 0.3403766f, 1f);
        if (allDead == true)
        {
            ClearData();
            //sm.BattleEnd();
        }
        if (battleStart == true && allDead == false)
        //if (battleStart == true)
        {
            if (cursorIndex >= 0 && cursorIndex < enemies.Count && enemies[cursorIndex].health <= 0 && allDead == false)
            {
                allDead = true;
                for (int i = 0; i < enemies.Count; i++)
                {
                    if (enemies[i].health > 0)
                    {
                        cursorIndex = i;
                        cursorPos = new Vector3(enemySlots[cursorIndex].position.x, cursorYPos, 0.0f);
                        cursor.transform.position = cursorPos;
                        allDead = false;
                    }
                }
            }
            if (Input.GetKeyDown(KeyCode.A) && cursorIndex != 0 && enemies[cursorIndex - 1].health > 0)
            {   
                cursorIndex -= 1;
                cursorPos = new Vector3(enemySlots[cursorIndex].position.x, cursorYPos, 0.0f);
                cursor.transform.position = cursorPos;
            }
            if (Input.GetKeyDown(KeyCode.D) && cursorIndex != 1 && enemies[cursorIndex + 1].health > 0)
            {
                cursorIndex += 1;
                cursorPos = new Vector3(enemySlots[cursorIndex].position.x, cursorYPos, 0.0f);
                cursor.transform.position = cursorPos;
            }
            if (enemies[0].attack == true)
            {
                wizard.TakeDamage(enemies[cursorIndex].attackDamage);
                enemies[0].attack = false;
            }
            if (enemies.Count != 1 && enemies[1].attack == true)
            {
                wizard.TakeDamage(enemies[cursorIndex].attackDamage);
                enemies[0].attack = false;
            }      
            takeDamage = false;
            // Player submitted a drawing
            if (demo_script.recognized == true)
            {
                brushMaterial = demo_script.gestureOnScreenPrefab.GetComponent<Renderer>().sharedMaterial;
                for (int i = 0; i < hand.Count; i++) 
                {
                    if  (hand[i] != null && 
                        demo_script.drawing != null && 
                        brushMaterial != null &&
                        ((hand[i].name == demo_script.drawing) || 
                        (hand[i].altName != "" && demo_script.drawing == hand[i].altName)))
                    {
                        if (pencil == true)
                        {
                            pencilCounter++;
                            if (pencilCounter == 5)
                            {
                                for (int x = 0; x < enemies.Count; x++)
                                {
                                    if (enemies[x] != null)
                                    {
                                        if (enemies[x].health > 0)
                                        {
                                            isDead = enemies[x].TakeDamage(attackDamage, "Normal");   
                                        }
                                    }
                                }
                            }
                        }
                        if (bandage == true)
                        {
                            wizard.Heal(1);
                        }
                        if (!brushMaterial.name.ToLower().Contains(hand[i].spellElement.ToLower()) && !hand[i].hueShift)
                        {
                            mismatchColor = true;
                        } 
                        if (timeStop == true)
                        {
                            for (int x = 0; x < enemies.Count; x++)
                            {
                                enemies[x].timeStop = false;
                            }
                            timeStop = false;
                        }
                        if (hand[i].spellName == "Hiatus")
                        {
                            // We also stop time for enemies later in the code
                            Hiatus();
                        }
                        if (spellUIGO[i].isBurned == true)
                        {
                            wizard.TakeDamage(5);
                            spellUIGO[i].Unburned();
                        }
                        if (surge == true)
                        {
                            surge = false;
                            for (int x = 0; x < enemies.Count; x++)
                            {
                                if (enemies[x] != null)
                                {
                                    enemies[x].isSurge = true;
                                }
                            }                   
                        }
                        if (doublecast == true)
                        {
                            doublecast = false;
                        }
                        else
                        {
                            demo_script.drawing = "";
                        }
                        if (hand[i].hueShift == true)
                        {
                            elementType = brushMaterial.name.Split(' ')[0];
                            Debug.Log(elementType);
                        } 
                        else 
                        {
                            elementType = hand[i].spellElement;
                        }
                        if (deck.Count != 0 && hand[i].spellName == "Oracle's Eye")
                        {
                            if (hand[i].spellElement == deck[deck.Count-1].spellElement)
                            {
                                Debug.Log("Predicted");
                                for (int x = 0; x < enemies.Count; x++)
                                {
                                    if (enemies[x] != null)
                                    {
                                        enemies[x].isAllegiance = true;
                                    }
                                }  
                            }
                        }
                        if (hand[i].spellName == "Thunderstorm")
                        {
                            // Select a random enemy
                            int randomEnemyIndex = Random.Range(0, enemies.Count);
                            // Apply the damage over time to this enemy (DAMAGE PER SECOND IS MODIFIED)
                            StartCoroutine(enemies[randomEnemyIndex].TakeDamageOverTimeRandom(hand[i].spellDPS, hand[i].duration, elementType));
                            AddToDiscard(i);
                            // Immediately exit the Update function
                            return;
                        }
                        else if (hand[i].spellName == "Bee Swarm")
                        {
                            for (int x = 0; x < enemies.Count; x++)
                            {
                                if (enemies[x] != null)
                                {
                                    StartCoroutine(enemies[x].TakeDamageOverTime(hand[i].spellDPS, hand[i].duration, elementType));
                                }
                            }
                            AddToDiscard(i);
                            // Immediately exit the Update function
                            return;
                        }
                        if (hand[i].spellName == "Doublecast")
                        {
                            doublecast = true;
                        }
                        if (hand[i].spellSurge == true)
                        {
                            surge = true;
                        }
                        // Issue: Having Tornado and Fire Tornado, can't select which is which
                        //demo_script.drawing = "";
                        //spellType = spellActions.GetSpellType(spellUI.spell);
                        attackDamage = hand[i].SpellDamage;
                        spellActions.GetVFX(hand[i], cursorIndex); 

                        if (hand[i].spellName == "Telekinesis")
                        {
                            if (discardPile.Count > 0)
                            {
                                Spell telekinesis = hand[i];
                                hand[i] = discardPile[discardPile.Count - 1];
                                discardPile[discardPile.Count - 1] = telekinesis;
                                index = i;
                                switch (i)
                                {
                                    case 0:
                                        spellUI1.LoadSpell(hand[i]);
                                        break;
                                    case 1:
                                        spellUI2.LoadSpell(hand[i]);
                                        break;
                                    case 2:
                                        spellUI3.LoadSpell(hand[i]);
                                        break;
                                }
                            }
                            return;
                        }
                        if (hand[i].spellHeal != 0)
                        {
                            wizard.Heal(hand[i].spellHeal);
                        }

                        if (hand[i].spellName == "Chain Lightning")
                        {
                            isDead = enemies[cursorIndex].TakeDamage(attackDamage, elementType);
                            
                            int numberOfBounces = 4;
                            System.Random random = new System.Random(); // Create the Random object outside the loop

                            for (int j = 0; j < numberOfBounces; j++)
                            {
                                // If we have removed an enemy, and the new cursor index is still valid, select that enemy
                                if (cursorIndex < enemies.Count)
                                {
                                    // Assuming enemies are in a line or circle
                                    List<int> adjacentIndices = new List<int>();
                                    if (cursorIndex - 1 >= 0 && enemies[cursorIndex-1] != null) 
                                    {
                                        adjacentIndices.Add(cursorIndex - 1); // previous enemy
                                    }
                                    if (cursorIndex + 1 < enemies.Count && enemies[cursorIndex+1] != null) 
                                    {
                                        adjacentIndices.Add(cursorIndex + 1); // next enemy
                                    }
                                    if (adjacentIndices.Count == 0)
                                    {
                                        break; // exit if there are no more adjacent enemies
                                    }
                                    else
                                    {
                                        // select a random enemy from the list of adjacent enemies
                                        int randomIndex = random.Next(adjacentIndices.Count); // Corrected use of Random.Next()
                                        cursorIndex = adjacentIndices[randomIndex];
                                        //Debug.Log(cursorIndex);
                                        isDead = enemies[cursorIndex].TakeDamage(attackDamage, elementType);
                                    }
                                }
                                else
                                {
                                    break; // exit if there are no valid cursor index
                                }
                            }
                            return;
                        }

                        if (hand[i].spellTarget == Spell.SpellTarget.single)
                        {
                            isDead = enemies[cursorIndex].TakeDamage(attackDamage, elementType); 
                            
                            if (hand[i].spellName == "Doodle Dividend" && isDead == true)
                            {
                                gm.AddGoldNumber(25);
                            }
                            if (hand[i].spellName == "Vital Vengeance" && isDead == true)
                            {
                                wizard.AddHealth(5);
                            }
                            if (hand[i].spellName == "Amplifying Arcane" && isDead == true)
                            {
                                hand[i].SpellDamage += 2;
                            }
                        }
                        if (hand[i].spellTarget == Spell.SpellTarget.all)
                        {
                            for (int j = 0; j < enemies.Count; j++)
                            {
                                if (enemies[j] != null)
                                {
                                    if (enemies[j].health > 0)
                                    {
                                        isDead = enemies[j].TakeDamage(attackDamage, elementType);   
                                    }
                                    if (hand[i].spellStun != 0)
                                    {
                                        enemies[j].Stun(hand[i].spellStun);
                                    }
                                    if (hand[i].spellFreeze != 0)
                                    {
                                        enemies[j].Frozen(hand[i].spellFreeze);
                                    }
                                    if (hand[i].spellBurn != 0)
                                    {
                                        enemies[j].Burn(hand[i].spellBurn);
                                    }
                                    if (hand[i].spellPoison != 0)
                                    {
                                        if (j == cursorIndex && hand[i].spellName == "Contagion")
                                        {
                                            Debug.Log("Contagion");
                                            enemies[j].Poison(hand[i].spellPoison + 2);
                                        }
                                        else
                                        {
                                            enemies[j].Poison(hand[i].spellPoison);
                                        }
                                    } 
                                    if (hand[i].spellCurse != 0)
                                    {
                                        enemies[j].Curse(hand[i].spellCurse);
                                    } 
                                    if (hand[i].spellChill != 0)
                                    {
                                        enemies[j].Chill(hand[i].spellChill);
                                    } 
                                    if (hand[i].spellConcuss != 0)
                                    {
                                        enemies[j].Concuss(hand[i].spellConcuss);
                                    }
                                    if (hand[i].spellDouse == true)
                                    {
                                        enemies[j].Douse(true);
                                    }
                                    if (hand[i].spellName == "Chronostasis")
                                    {
                                        timeStop = true;
                                        enemies[j].timeStop = true;
                                    }
                                    if (hand[i].spellName == "Hiatus")
                                    {
                                        enemies[j].hiatus = true;
                                    }
                                }
                            }
                        }
                        if (hand[i].spellStun != 0 && hand[i].spellTarget != Spell.SpellTarget.all)
                        {
                            enemies[cursorIndex].Stun(hand[i].spellStun);
                            if (cursorIndex-1 >= 0)
                            {
                                if (enemies[cursorIndex-1] != null && enemies[cursorIndex-1].isDoused == true)
                                {
                                    enemies[cursorIndex-1].Stun(hand[i].spellStun);
                                    enemies[cursorIndex-1].RemoveDouse();
                                }
                            }
                            if (cursorIndex+1 < enemies.Count)
                            {
                                if (enemies[cursorIndex+1] != null && enemies[cursorIndex+1].isDoused == true)
                                {
                                    enemies[cursorIndex+1].Stun(hand[i].spellStun);
                                    enemies[cursorIndex+1].RemoveDouse();
                                }
                            }
                        }
                        if (hand[i].spellFreeze != 0 && hand[i].spellTarget != Spell.SpellTarget.all)
                        {
                            enemies[cursorIndex].Frozen(hand[i].spellFreeze);
                        }
                        if (hand[i].spellBurn != 0 && hand[i].spellTarget != Spell.SpellTarget.all)
                        {
                            enemies[cursorIndex].Burn(hand[i].spellBurn);
                        }
                        if (hand[i].spellPoison != 0 && hand[i].spellTarget != Spell.SpellTarget.all)
                        {
                            enemies[cursorIndex].Poison(hand[i].spellPoison);
                            if (cursorIndex-1 >= 0)
                            {
                                if (enemies[cursorIndex-1] != null && enemies[cursorIndex-1].isDoused == true)
                                {
                                    enemies[cursorIndex-1].Poison(hand[i].spellPoison);
                                    enemies[cursorIndex-1].RemoveDouse();
                                }
                            }
                            if (cursorIndex+1 < enemies.Count)
                            {
                                if (enemies[cursorIndex+1] != null && enemies[cursorIndex+1].isDoused == true)
                                {
                                    enemies[cursorIndex+1].Poison(hand[i].spellPoison);
                                    enemies[cursorIndex+1].RemoveDouse();
                                }
                            }
                        }
                        if (hand[i].spellCurse != 0 && hand[i].spellTarget != Spell.SpellTarget.all)
                        {
                            enemies[cursorIndex].Curse(hand[i].spellCurse);
                        }
                        if (hand[i].spellChill != 0 && hand[i].spellTarget != Spell.SpellTarget.all)
                        {
                            enemies[cursorIndex].Chill(hand[i].spellChill);
                        } 
                        if (hand[i].spellConcuss != 0 && hand[i].spellTarget != Spell.SpellTarget.all)
                        {
                            enemies[cursorIndex].Concuss(hand[i].spellConcuss);
                        }
                        if (hand[i].spellDouse == true && hand[i].spellTarget != Spell.SpellTarget.all)
                        {
                            enemies[cursorIndex].Douse(true);
                        }
                        if (hand[i].altName == "Magnitude")
                        {
                            string[] parts = hand[i].spellName.Split(' ');
                            // Get the number portion only
                            string numberPart = parts[1];
                            // Increment magnitude
                            int magNumber = int.Parse(numberPart) + 1;
                            string newString = "Magnitude " + magNumber;
                            Spell targetSpell = regSpells.regSpellList.FirstOrDefault(s => s.spellName == newString);
                            discardPile.Add(targetSpell);
                        }
                        if (hand[i].altName != "Magnitude")
                        {
                            discardPile.Add(hand[i]);
                        }
                        discardPileSizeText.text = "Discard: " + discardPile.Count;
                        index = i;
                        switch (i)
                        {
                            case 0:
                                card1 = false;
                                //Spell spellUI1 = spellsInHand[0];
                                spellUI1.UnloadSpell(hand[i]);
                                break;
                            case 1:
                                card2 = false;
                                //SpellUI spellUI2 = spellsInHand[1];
                                spellUI2.UnloadSpell(hand[i]);
                                break;
                            case 2:
                                card3 = false;
                                //SpellUI spellUI3 = spellsInHand[2];
                                spellUI3.UnloadSpell(hand[i]);
                                break;
                        }
                        mismatchColor = false;
                        DrawSpell();  
                    }
                }
            }
        }
    }
}