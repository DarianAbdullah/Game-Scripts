using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class ComboUI : MonoBehaviour
{
    public ComboUI combo1;
    public ComboUI combo2;
    public ComboUI combo3;
    public ComboUI combo4;
    public Spell spell;
	public TMP_Text spellTitle;
    public Image spellIcon;
    public Tuple<Spell, Spell, Spell> combo;
    //public GameObject discardEffect;
    CombineManager cm;
    GameManager gm;
    BattleManager bm;
    SceneMan sm;
    //Animator animator;
    private void Awake()
    {
        cm = FindObjectOfType<CombineManager>();
        gm = FindObjectOfType<GameManager>();
        bm = FindObjectOfType<BattleManager>();
        sm = FindObjectOfType<SceneMan>();
        //animator = GetComponent<Animator>();
    }

    public void LoadSpell(Spell _spell)
    {
        spell = _spell;
        gameObject.GetComponent<RectTransform>().localScale=new Vector3(1,1,1);
        spellTitle.text = spell.spellName;
        spellIcon.sprite = spell.spellIcon;
    }
    public void AddAndRemove()
    {
        gm.playerDeck.Add(spell);
        bm.discardPile.Add(spell);
        for (int i = 0; i < cm.combos.Count; i++)
        {
            if (cm.combos[i].Item1.spellName == spellTitle.text)
            {
                gm.playerDeck.Remove(cm.combos[i].Item2);
                gm.playerDeck.Remove(cm.combos[i].Item3);
                cm.combos.RemoveAt(i);
            }
        }
        sm.ObtainedCombo();
    }
}
