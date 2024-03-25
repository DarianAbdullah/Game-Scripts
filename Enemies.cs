using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using System;

public class Enemies : MonoBehaviour
{
    BattleManager bm;
    GameManager gm;
    Demo demo;

    // Scriptable Object Data
    public EnemyObjects enemyData;

    public Sprite sprite;
    //[HideInInspector]
    public int health;
    //[HideInInspector]
    public float attackSpeed;
    //[HideInInspector]
    public int attackDamage;
    //[HideInInspector] 
    public int goldDrop;
    public float timeMultiplier;
    public bool timeStop = false;
    public bool hiatus = false;
    public Image healthBar;
    public Image actionBar;
    public GameObject dbBar;
    public GameObject dbBarText;
    public SpriteRenderer enemySprite;
    public TMP_Text enemyName;
    private int maxHealth;
    private float maxAttackSpeed;
    public TMP_Text healthUIText;
    public GameObject floatingText;
    public Canvas canvas;
    public Camera battlecam;
    public Canvas battleCanvas;
    public TMP_Text nextMoveText;
    public string nextMove;

    private int temp;
    [HideInInspector]
    public List <GameObject> debuffBar; 
    [HideInInspector]
    public List <GameObject> debuffBarText; 
    [HideInInspector]
    public float freeze = 0f;
    [HideInInspector]
    public float stun = 0f;
    [HideInInspector]
    public float burnTimer = 0f;
    [HideInInspector]
    public bool attack = false;
    [HideInInspector]
    public float poisonTimer = 0f;
    [HideInInspector]
    public List<int> freezeTimer = new List<int>();
    private Vector3 myPosition;
    private static System.Random random = new System.Random();
    private float chillTimer = 0f;
    private int chillPos = 0;
    private int counter = 0;
    private int firePos = 0;
    private int burnDamage = 0;
    private bool justApplied = false;
    private int poisonPos = 0;
    private int poisonDamage = 0;
    private int totalCurse = 0;
    private float curseTimer = 0f;
    private int frozenPos = 0;
    private int cursePos = 0;
    private int stunPos = 0;
    private float xPos = 0;
    private float concussTimer = 0f;
    private float concussEffect = 0.20f;
    private bool miss = false;
    private int concussPos = 0;
    public bool isDoused = false;
    [HideInInspector]
    public float douseTimer = 0f;
    private int dousePos = 0;
    GameObject debuff;
    GameObject debuffText;

    public bool isSurge = false;
    public bool isAllegiance = false;

    void Start()
    {
        demo = FindObjectOfType<Demo>();
        gm = FindObjectOfType<GameManager>();
        bm = FindObjectOfType<BattleManager>();
    }
    void Update()
    {
        if (nextMoveText.text == "")
        {
            nextMoveText.text = CalculateNextMove();
        }
        if (GameObject.Find("GestureOnScreen(Clone)") && timeStop != true && hiatus != true)
        {
            timeMultiplier = 1;
            bm.timeMultiplier = timeMultiplier;
            //Time.timeScale = 1.0f;
            //Debug.Log("Moving faster");
        }
        else if (timeStop != true && hiatus != true)
        {
            timeMultiplier = 0.5f;
            bm.timeMultiplier = timeMultiplier;
            //Time.timeScale = 0.0f;
            //Debug.Log("Moving slower");
        }
        else if (timeStop == true || hiatus == true)
        {
            timeMultiplier = 0.0f;
            bm.timeMultiplier = timeMultiplier;
        }
        float modifiedDeltaTime = Time.deltaTime;

        if (chillTimer > 0)
        {
            modifiedDeltaTime = modifiedDeltaTime * 0.75f * timeMultiplier;
            chillTimer -= Time.deltaTime * timeMultiplier;
            for (int i = 0; i < debuffBar.Count; i++)
            {
                // Convert the float time to int time, add 1 because 3.99 becomes 3 
                temp = (int)chillTimer + 1;
                int index = debuffBar.FindIndex(a => a.name == "Chill(Clone)");
                debuffBarText[index].gameObject.GetComponent<TMP_Text>().text = temp.ToString();
            }
            if (chillTimer <= 0)
            {
                counter--;
                UpdateDebuffs("remove", counter, chillPos);
                chillTimer = 0;
            }
        }
        /*
        if (douseTimer > 0)
        {
            douseTimer -= Time.deltaTime * timeMultiplier;
            for (int i = 0; i < debuffBar.Count; i++)
            {
                // Convert the float time to int time, add 1 because 3.99 becomes 3 
                temp = (int)douseTimer + 1;
                int index = debuffBar.FindIndex(a => a.name == "Douse(Clone)");
                debuffBarText[index].gameObject.GetComponent<TMP_Text>().text = temp.ToString();
            }
            if (douseTimer <= 0)
            {
                counter--;
                UpdateDebuffs("remove", counter, dousePos);
                douseTimer = 0;
            }
        }
        */
        if (burnTimer > 0)
        {
            burnTimer -= Time.deltaTime * timeMultiplier;
            for (int i = 0; i < debuffBar.Count; i++)
            {
                // Convert the float time to int time, add 1 because 3.99 becomes 3 
                temp = (int)burnTimer + 1;
                int index = debuffBar.FindIndex(a => a.name == "Burn(Clone)");
                debuffBarText[index].gameObject.GetComponent<TMP_Text>().text = temp.ToString();
            }
            // Only apply damage if burn wasn't just applied
            if (!justApplied && Math.Floor(burnTimer) != Math.Floor(burnTimer + (Time.deltaTime * timeMultiplier)))
            {
                DebuffDOT(burnDamage, "Fire");
            }
            justApplied = false;  // Set justApplied to false after the first frame
            if (burnTimer <= 0)
            {
                counter--;
                UpdateDebuffs("remove", counter, firePos);
                burnDamage = 0;
            }
        }

        if (poisonTimer > 0)
        {
            poisonTimer -= Time.deltaTime * timeMultiplier;
            for (int i = 0; i < debuffBar.Count; i++)
            {
                // Convert the float time to int time, add 1 because 3.99 becomes 3 
                temp = (int)poisonTimer + 1;
                int index = debuffBar.FindIndex(a => a.name == "Poison(Clone)");
                debuffBarText[index].gameObject.GetComponent<TMP_Text>().text = temp.ToString();
            }
            if (Math.Floor(poisonTimer) != Math.Floor(poisonTimer + (Time.deltaTime * timeMultiplier)))
            {
                poisonDamage--;
                TakeDamage(poisonDamage, "");
            }
            if (poisonTimer <= 0)
            {
                counter--;
                UpdateDebuffs("remove", counter, poisonPos);
            }
        }

        if (totalCurse != 0)
        {
            curseTimer += Time.deltaTime * timeMultiplier;
            if (curseTimer >= 1f)
            {
                curseTimer = curseTimer % 1f;
                TakeDamage(totalCurse, "");
            }
        }

        if (freeze > 0 && stun <= 0)
        {
            freeze -= Time.deltaTime * timeMultiplier;
        }
        else if (stun > 0)
        {
            stun -= Time.deltaTime * timeMultiplier;
            for (int i = 0; i < debuffBar.Count; i++)
            {
                // Convert the float time to int time, add 1 because 3.99 becomes 3 
                temp = (int)stun + 1;
                int index = debuffBar.FindIndex(a => a.name == "Stun(Clone)");
                debuffBarText[index].gameObject.GetComponent<TMP_Text>().text = temp.ToString();
            }
            if (stun <= 0)
            {
                counter--;
                UpdateDebuffs("remove", counter, stunPos);
            }
        }
        else
        {
            attackSpeed -= modifiedDeltaTime * timeMultiplier;
            actionBar.fillAmount = attackSpeed / maxAttackSpeed;
            attack = false;
        }
        if (concussTimer > 0)
        {
            concussTimer -= Time.deltaTime * timeMultiplier;
            for (int i = 0; i < debuffBar.Count; i++)
            {
                // Convert the float time to int time, add 1 because 3.99 becomes 3 
                temp = (int)concussTimer + 1;
                int index = debuffBar.FindIndex(a => a.name == "Concuss(Clone)");
                debuffBarText[index].gameObject.GetComponent<TMP_Text>().text = temp.ToString();
            }
            if (concussTimer <= 0)
            {
                counter--;
                UpdateDebuffs("remove", counter, concussPos);
                concussTimer = 0;
            }
        }
        // Enemy attacks
        if (attackSpeed <= 0)
        {
            if (concussTimer > 0)
            {
                //float newConcussEffect = concussEffect + (((float)Math.Floor(concussTimer/5))/10);
                //Debug.Log((((float)Math.Floor(concussTimer/5))/10));
                //float percentage = (float)(((int)concussTimer/5) * 0.05);
                //float newConcussEffect = concussEffect + percentage;
                float rounded = (int)concussTimer;
                float newConcussEffect = concussEffect + (float)(rounded * 0.01);
                float missChance = UnityEngine.Random.Range(0f, 1f);
                if (missChance <= newConcussEffect)
                {
                    miss = true;
                }
            }
            if (name == "Spicy Tako" && miss == false)
            {
                if (nextMove == "Burn")
                {
                    int firstNumber = random.Next(0, 3);
                    int secondNumber = GenerateDifferentRandomNumber(firstNumber);
                    bm.spellUIGO[firstNumber].BurnCard();
                    bm.spellUIGO[secondNumber].BurnCard();
                }
                else if (nextMove == "Attack")
                {
                    attack = true;
                }
                else
                {
                    Debug.Log("Error");
                }
                attackSpeed = maxAttackSpeed;
                actionBar.fillAmount = maxAttackSpeed;
                nextMoveText.text = "";
            }
            else if (name == "Gooseneck Kettle" && miss == false)
            {
                if (nextMove == "Douse")
                {
                    demo.RemoveGestureOnScreenObjects();
                }
                else if (nextMove == "Attack")
                {
                    attack = true;
                }
                else
                {
                    Debug.Log("Error");
                }
                attackSpeed = maxAttackSpeed;
                actionBar.fillAmount = maxAttackSpeed;
                nextMoveText.text = "";
            }
            else if (name == "Waterdemon" && miss == false)
            {
                attack = true;
                attackSpeed = maxAttackSpeed;
                actionBar.fillAmount = maxAttackSpeed;
            }
            //demo.isFrozen == true;
            /*
            if (name == "Yogi" && miss == false)
            {
                demo.Shrink();
                attack = true;
                attackSpeed = maxAttackSpeed;
                actionBar.fillAmount = maxAttackSpeed;
            }
            */
            else if (miss == true)
            {
                Debug.Log(name + " missed!");
                miss = false;
                attack = true;
                attackSpeed = maxAttackSpeed;
                actionBar.fillAmount = maxAttackSpeed;
            }
            else
            {
                //Debug.Log(this.gameObject);
                //Debug.Log("Error");
                /*
                attack = true;
                attackSpeed = maxAttackSpeed;
                actionBar.fillAmount = maxAttackSpeed;
                */
            }
        }

        // ...
    }

    public static int GenerateDifferentRandomNumber(int differentFrom)
    {
        int randomNumber = random.Next(0,3);
        while (randomNumber == differentFrom)
        {
            randomNumber = random.Next(0,3);
        }
        return randomNumber;
    }
    public void UpdateDebuffs(string tag, int counter, int debuffPos)
    {
        xPos = counter * 35.0f;
        // Create a new debuff
        if (tag != "remove")
        {
            // Create the debuff
            debuff = GameObject.FindWithTag(tag);
            GameObject debuffChild = Instantiate(debuff);
            // Need this.transform because this is a gameobject and it needs to be a transform
            debuffChild.transform.SetParent(this.transform.Find("Canvas"));
            debuffBar.Add(debuffChild);
            // Have to change x and y position because by default myPosition is dead center of the GO
            debuffBar[counter].transform.position = myPosition + new Vector3(xPos-40, -50.0f, 0.0f);
            // Create the debuff text 
            // DBText lives inside the Debuff gameobject 
            debuffText = GameObject.FindWithTag("DBText");
            debuffBarText.Add(GameObject.Instantiate(debuffText));
            debuffBarText[counter].transform.position = myPosition + new Vector3(xPos-20, -40.0f, 0.0f);
        }
        else
        {
            // Remove the debuff
            // The debuff being removed is not at the end 
            for (int i = debuffPos; i < counter; i++)
            {   
                xPos = i * 20.0f;
                debuffBar[i].GetComponent<Image>().sprite = debuffBar[i+1].GetComponent<Image>().sprite;
                Debug.Log(debuffBar[i].GetComponent<Image>().sprite);
                string objectName = debuffBar[i+1].name;
                Debug.Log(objectName);
                // Shift all of the other debuffs down one 
                switch (objectName)
                {
                    case "Burn(Clone)":
                        firePos--;
                        continue;
                    case "Poison(Clone)":
                        poisonPos--;
                        continue;
                    case "Frozen(Clone)":
                        frozenPos--;
                        continue;
                    case "Stun(Clone)":
                        stunPos--;
                        continue;
                    case "Curse(Clone)":
                        cursePos--;
                        continue;
                    case "Chill(Clone)":
                        chillPos--;
                        continue;
                    case "Concuss(Clone)":
                        concussPos--;
                        continue;
                    case "Douse(Clone)":
                        dousePos--;
                        continue;
                }
            }
            for (int i = counter; i > debuffPos; i--)
            {   
                debuffBar[i].transform.position = debuffBar[i-1].transform.position;
                debuffBarText[i].transform.position = debuffBarText[i-1].transform.position;
            }
            if (debuffPos > counter)
            {
                Destroy(debuffBar[counter]);
                debuffBar.RemoveAt(counter);
                Destroy(debuffBarText[counter]);
                debuffBarText.RemoveAt(counter);
            }
            else
            {
                Destroy(debuffBar[debuffPos]);
                debuffBar.RemoveAt(debuffPos);
                Destroy(debuffBarText[debuffPos]);
                debuffBarText.RemoveAt(debuffPos);
            }
        }
    }

    public void Stun(float time)
    {
        if (stun > 0)
        {
            stun += time;
            temp = (int)stun + 1;
            // Find the position and then edit the text
            int index = debuffBar.FindIndex(a => a.name == "Stun(Clone)");
            debuffBarText[index].gameObject.GetComponent<TMP_Text>().text = temp.ToString();
            return;
        }
        UpdateDebuffs("Stun", counter, stunPos);
        stunPos = counter;
        counter++;
        stun += time;
    }
    public void Burn(int burn)
    {
        // Check if a debuff is already active
        if (burnTimer > 0)
        {
            burnTimer += burn;
            burnDamage += burn;
            temp = (int)burn + 1;
            int index = debuffBar.FindIndex(a => a.name == "Burn(Clone)");
            debuffBarText[index].gameObject.GetComponent<TMP_Text>().text = temp.ToString();
            return;
        }
        UpdateDebuffs("Burn", counter, firePos);
        firePos = counter;
        counter++;
        burnTimer += burn;
        burnDamage += burn;
        // Prevents burn from happening immediately
        justApplied = true;
    }
    public void Poison(int poison)
    {
        // Check if a debuff is already active
        if (poisonTimer > 0)
        {
            poisonTimer += poison;
            poisonDamage += poison;
            temp = (int)poison + 1;
            int index = debuffBar.FindIndex(a => a.name == "Poison(Clone)");
            debuffBarText[index].gameObject.GetComponent<TMP_Text>().text = temp.ToString();
            return;
        }
        
        UpdateDebuffs("Poison", counter, poisonPos);
        poisonPos = counter;
        counter++;
        poisonTimer += poison;
        // Inflict damage now as well
        poisonDamage += poison;
        TakeDamage(poisonDamage, "");
    }
    public void Frozen(float time)
    {
        if (freezeTimer.Count != 0)
        {
            freezeTimer[0] += (int)time;
            temp = freezeTimer[0];
            int index = debuffBar.FindIndex(a => a.name == "Frozen(Clone)");
            debuffBarText[index].gameObject.GetComponent<TMP_Text>().text = temp.ToString();
            return;
        }
        UpdateDebuffs("Frozen", counter, frozenPos);
        frozenPos = counter;
        counter++;
        freeze = time;
        freezeTimer.Add((int)time + 1);
        StartCoroutine(Freezing());
        IEnumerator Freezing()
        {
        while (freezeTimer.Count > 0) //&& stun <= 0)
            {
                for (int i = 0; i < freezeTimer.Count; i++)
                {
                    temp = freezeTimer[i];
                    //freezeText = freezeTimer[i].ToString();
                    freezeTimer[i]--;
                    temp--;
                    for (int j = 0; j < debuffBar.Count; j++)
                    {
                        if (debuffBar[j].name == "Frozen(Clone)")
                        {
                            debuffBarText[j].gameObject.GetComponent<TMP_Text>().text = temp.ToString();
                        }
                    }
                }
                freezeTimer.RemoveAll(i => i == 0);
                if (temp == 0)
                {
                    counter--;
                    UpdateDebuffs("remove", counter, frozenPos);
                }
                yield return new WaitForSeconds(1f);
            }
        }
    }
    public void Curse(int curse)
    {
        // Check if target hasn't been cursed yet
        if (totalCurse == 0)
        {
            UpdateDebuffs("Curse", counter, 0);
            counter++;            
            //cursePos = counter;
            totalCurse += curse;
            int index = debuffBar.FindIndex(a => a.name == "Curse(Clone)");
            debuffBarText[index].gameObject.GetComponent<TMP_Text>().text = totalCurse.ToString();
        }
        else 
        {
            totalCurse += curse;
            int index = debuffBar.FindIndex(a => a.name == "Curse(Clone)");
            debuffBarText[index].gameObject.GetComponent<TMP_Text>().text = totalCurse.ToString();
        }
        
    }
    public void Chill(float chill)
    {
        // Check if a debuff is already active
        if (chillTimer > 0)
        {
            chillTimer += chill;
            temp = (int)chill + 1;
            int index = debuffBar.FindIndex(a => a.name == "Chill(Clone)");
            debuffBarText[index].gameObject.GetComponent<TMP_Text>().text = temp.ToString();
            // Because of return no need for else
            return;
        }
        UpdateDebuffs("Chill", counter, chillPos);
        chillPos = counter;
        counter++;
        chillTimer += chill;        
    }
    public void Concuss(float concuss)
    {
        Debug.Log("concussing");
        // Check if a debuff is already active
        if (concussTimer > 0)
        {
            concussTimer += concuss;
            temp = (int)concuss + 1;
            int index = debuffBar.FindIndex(a => a.name == "Concuss(Clone)");
            debuffBarText[index].gameObject.GetComponent<TMP_Text>().text = temp.ToString();
            // Because of return no need for else
            return;
        }
        UpdateDebuffs("Concuss", counter, concussPos);
        concussPos = counter;
        counter++;
        concussTimer += concuss;        
    }
    /*
    public void Douse(int douse)
    {
        Debug.Log("dousing");
        // Check if a debuff is already active
        if (douseTimer > 0)
        {
            douseTimer += douse;
            temp = (int)douse + 1;
            int index = debuffBar.FindIndex(a => a.name == "Douse(Clone)");
            debuffBarText[index].gameObject.GetComponent<TMP_Text>().text = temp.ToString();
            // Because of return no need for else
            return;
        }
        UpdateDebuffs("Douse", counter, dousePos);
        dousePos = counter;
        counter++;
        douseTimer += douse;        
    }
    */
    public void Douse(bool douse)
    {
        if (isDoused == false)
        {
            UpdateDebuffs("Douse", counter, dousePos);
            dousePos = counter;
            counter++;    
            isDoused = true;
        }
    }
    public void RemoveDouse()
    {
        counter--;   
        UpdateDebuffs("remove", counter, dousePos);   
        isDoused = false;
    }
    void ShowFloatingText(int damage)
    {
        Vector2 screenPosition = battlecam.WorldToScreenPoint(transform.position);
        var go = Instantiate(floatingText, screenPosition, Quaternion.identity, battleCanvas.transform);
        go.GetComponent<TextMeshProUGUI>().text = damage.ToString();
    }

    void ShowFloatingTextColor(int damage, float r, float g, float b)
    {
        Vector2 screenPosition = battlecam.WorldToScreenPoint(transform.position);
        var go = Instantiate(floatingText, screenPosition, Quaternion.identity, battleCanvas.transform);
        go.GetComponent<TextMeshProUGUI>().text = damage.ToString();
        go.GetComponent<TextMeshProUGUI>().color = new Color(r, g, b, 1);
    }
    public enum Element
    {
        Fire,
        Water,
        Earth,
        Wind,
        Thunder,
    }
    Dictionary<string, string> strongAgainst = new Dictionary<string, string>()
    {
        { "Fire", "Wind" },
        { "Wind", "Earth" },
        { "Earth", "Thunder" },
        { "Thunder", "Water" },
        { "Water", "Fire" },
    };

    Dictionary<string, string> weakAgainst = new Dictionary<string, string>()
    {
        { "Wind", "Fire" },
        { "Earth", "Wind" },
        { "Thunder", "Earth" },
        { "Water", "Thunder" },
        { "Fire", "Water" }
    };
    public bool DebuffDOT(int damage, string spellElement)
    {
        if (strongAgainst.TryGetValue(enemyData.element, out string strongElement) && strongElement == spellElement)
        {
            // Enemy is resistant to this
            damage = damage / 2;
        }
        else if (weakAgainst.TryGetValue(enemyData.element, out string weakElement) && weakElement == spellElement)
        {
            // Enemy is weak against
            damage = damage * 2;
        }
        Debug.Log(damage);
        health -= damage;
        healthBar.fillAmount = (float)health / maxHealth;
        healthUIText.text = health.ToString();
        if (floatingText)
        {
            switch (spellElement)
            {
                case "Fire":
                    ShowFloatingTextColor(damage, 227/255f, 94/255f, 87/255f);
                    break;
                default:
                    Debug.Log("Error");
                    break;
            }
        }
        if (health <= 0)
        {
            Destroy(gameObject);
            // No need to destroy debuff bar because it's a child now
            for (int i = 0; i < debuffBar.Count; i++)
            {
                Debug.Log("Trying to destroy debuffs");
                // No need to destroy debuff bar because it's a child now
                // Destroy(debuffBar[i]);
                // Still need to destroy Text because it doesn't work on the canvas for some reason
                Destroy(debuffBarText[i]);
            }
            //gm.UpdateGoldNumber(10);  
            return true;
        }
        return false;
    }
    public bool TakeDamage(int damage, string spellElement)
    {
        if (strongAgainst.TryGetValue(enemyData.element, out string strongElement) && strongElement == spellElement)
        {
            // Enemy is resistant to this
            damage = damage / 2;
        }
        else if (weakAgainst.TryGetValue(enemyData.element, out string weakElement) && weakElement == spellElement)
        {
            // Enemy is weak against
            damage = damage * 2;
        }
        if (isSurge == true)
        {
            double tempDamage = damage * (1 + 0.25);
            damage = Mathf.CeilToInt((float)tempDamage);
        }
        if (isAllegiance == true)
        {
            damage = damage * 2;
        }
        if (bm.mismatchColor == true)
        {
            Debug.Log("Mismatched color");
            damage = damage / 2;
        }
        isSurge = false;
        isAllegiance = false;
        Debug.Log(damage);
        health -= damage;
        healthBar.fillAmount = (float)health / maxHealth;
        healthUIText.text = health.ToString();
        if (floatingText)
        {
            ShowFloatingText(damage);
        }
        if (health <= 0)
        {
            Destroy(gameObject);
            // No need to destroy debuff bar because it's a child now
            for (int i = 0; i < debuffBar.Count; i++)
            {
                Debug.Log("Trying to destroy debuffs");
                // No need to destroy debuff bar because it's a child now
                // Destroy(debuffBar[i]);
                // Still need to destroy Text because it doesn't work on the canvas for some reason
                Destroy(debuffBarText[i]);
            }
            //gm.UpdateGoldNumber(10);  
            return true;
        }
        return false;
    }
    public IEnumerator TakeDamageOverTimeRandom(int damage, float duration, string spellElement)
    {
        float timer = 0;
        int damageInstances = 0;
        while (timer < duration)
        {
            timer += Time.deltaTime * timeMultiplier;
            // Every second, inflict damage on a random enemy
            if (timer >= damageInstances + 1)
            {
                // Create a list of alive enemies
                List<Enemies> aliveEnemies = new List<Enemies>();
                foreach(Enemies enemy in bm.enemies)
                {
                    if(enemy.health > 0)
                    {
                        aliveEnemies.Add(enemy);
                    }
                }
                // If there are no alive enemies, break out of the loop
                if (aliveEnemies.Count == 0)
                {
                    break;
                }
                // Get a random enemy from the alive enemies
                int randomIndex = UnityEngine.Random.Range(0, aliveEnemies.Count);
                Enemies randomEnemy = aliveEnemies[randomIndex];
                randomEnemy.TakeDamage(damage, spellElement);
                damageInstances++;
            }
            yield return null;
        }
    }
    public IEnumerator TakeDamageOverTime(int damage, float duration, string spellElement)
    {
        bool isDead = false;
        float timer = 0;
        int damageInstances = 0;
        while (timer < duration)
        {
            timer += Time.deltaTime * timeMultiplier;
            if (isDead == false && timer >= damageInstances + 1)
            {
                isDead = this.TakeDamage(damage, spellElement);
                damageInstances++;
            }
            yield return null;
        }
    }
    public void LoadData()
    {
        name = enemyData.name;
        //this.gameObject.Sprite = enemyData.sprite;
        health = enemyData.health;
        attackSpeed = enemyData.attackSpeed;
        attackDamage = enemyData.damage;
        goldDrop = enemyData.gold;

        maxAttackSpeed = attackSpeed;
        maxHealth = health;
        //SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        //spriteRenderer.sprite = enemyData.sprite;
        enemySprite.sprite = enemyData.sprite;
        myPosition = transform.position;
        enemyName.text = name;
        healthUIText.text = health.ToString();
        healthUIText.outlineWidth = 0.2f;
        healthUIText.outlineColor = new Color32(255, 255, 255, 255);
    }
    public string CalculateNextMove()
    {
        if (name == "Spicy Tako")
        {
            int randomNumber = random.Next(0,2);
            Debug.Log(randomNumber);
            switch (randomNumber)
            {
                case 0:
                    nextMove = "Burn";
                    break;
                case 1:
                    nextMove = "Attack";
                    break;
                default:
                    Debug.Log("Error");
                    break;
            }
        }
        else if (name == "Gooseneck Kettle")
        {
            int randomNumber = random.Next(0,2);
            switch (randomNumber)
            {
                case 0:
                    nextMove = "Douse";
                    break;
                case 1:
                    nextMove = "Attack";
                    break;
                default:
                    Debug.Log("Error");
                    break;
            }
        }
        else if (name == "Waterdemon")
        {
            nextMove = "Attack";
        }
        else
        {
            // Causes every monster to say attack at first
            //nextMove = "Attack";
        }
        return nextMove;
    }
}