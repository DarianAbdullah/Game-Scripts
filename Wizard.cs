using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Wizard : MonoBehaviour
{
    [SerializeField] BattleManager bm;
    [SerializeField] PlayerStats stats;
    [Header("Buffs")]
    public Buff vulnerable;
    public Buff weak;
    public Buff strength;
    public Image healthBar;
    public float healthAmount = 100; 
    public float totalHealth;

    Enemies enemy;
    private void Awake()
    {
        enemy = GetComponent<Enemies>();
        Debug.Log("setting up stats in awake");
        stats.UpdateHealth(healthAmount, totalHealth);
    }
    void Start()
    {
        totalHealth = healthAmount;
        Debug.Log("setting up stats");
        stats.UpdateHealth(healthAmount, totalHealth);
    }
    public void TakeDamage(float damage)
    {
        healthAmount -= damage;
        healthBar.fillAmount = healthAmount / totalHealth;
        stats.UpdateHealth(healthAmount, totalHealth);
        if (healthAmount <= 0)
        {
            //Destroy(gameObject);
            SceneManager.LoadScene("GameOver");
        }
    }
    public void Heal(float healingAmount)
    {
        healthAmount += healingAmount;
        healthAmount = Mathf.Clamp(healthAmount, 0, totalHealth);
        healthBar.fillAmount = healthAmount / totalHealth;
        stats.UpdateHealth(healthAmount, totalHealth);
    }
    public void AddHealth(float amount)
    {
        healthAmount += amount;
        totalHealth += amount;
        healthAmount = Mathf.Clamp(healthAmount, 0, totalHealth);
        healthBar.fillAmount = healthAmount / totalHealth;
        stats.UpdateHealth(healthAmount, totalHealth);
    }
}
