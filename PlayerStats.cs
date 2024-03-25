using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerStats : MonoBehaviour
{
	public TMP_Text health;
    public TMP_Text gold;
    public TMP_Text floor;
	public Transform relicParent;
	public GameObject relicPrefab;
	public GameObject playerStats;
    GameManager gm;
	BattleManager bm;
    
    private void Awake()
    {
        gm = FindObjectOfType<GameManager>();
		bm = FindObjectOfType<BattleManager>();
    }
	public void DisplayRelics()
	{
		foreach(Transform c in relicParent)
			Destroy(c.gameObject);

		foreach(Relic r in gm.relics)
			Instantiate(relicPrefab, relicParent).GetComponent<Image>().sprite = r.relicIcon;
	}
	public void UpdateHealth(float currentHealth, float maxHealth)
	{
		health.text = "HP: " + currentHealth.ToString() + "/" + maxHealth.ToString();
	}
}
