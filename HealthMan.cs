using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HealthMan : MonoBehaviour
{
    [SerializeField] BattleManager bm;
    public Image healthBar;
    public float healthAmount = 100f; 
    private float totalHealth;
    // Start is called before the first frame update
    void Start()
    {
        totalHealth = healthAmount;
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (gm.playerDamage == true)
        {
            TakeDamage(20);
        }
        */
    }
    public void TakeDamage(float damage)
    {
        healthAmount -= damage;
        healthBar.fillAmount = healthAmount / totalHealth;
        if (healthAmount <= 0)
        {
            //Destroy(gameObject);
            SceneManager.LoadScene("GameOver");
        }
    }
    public void Heal(float healingAmount)
    {
        healthAmount += healingAmount;
        healthAmount = Mathf.Clamp(healthAmount, 0, 100);
        healthBar.fillAmount = healthAmount / totalHealth;
    }
}
