using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellActions : MonoBehaviour
{
    Spell spell;
    BattleManager bm;
    Enemies enemies;
    public AudioSource audioSource;
    //public float freezeAmount = 0f;
    int enemyIndex;
    private void Awake()
    {
        bm = FindObjectOfType<BattleManager>();
        enemies = FindObjectOfType<Enemies>();
    }
	//public void PerformAction(Spell _spell)
    public void GetVFX(Spell _spell, int index)
    {
        spell = _spell;
        if (spell.vfx != null)
        {
          GameObject vfx = Instantiate(spell.vfx, bm.enemySlots[index].position, Quaternion.identity) as GameObject;
          Destroy(vfx, 1); 
        }
        else
        {
            Debug.Log("This spell doesn't have a VFX.");
        }
        if (spell.clip != null)
        {
            AudioClip myClip = spell.clip;
            audioSource.clip = myClip;
            audioSource.Play();
        }



        /*
        spell = _spell;
        //spell = _spell.GetComponent<Spell>();
        enemyIndex = bm.cursorIndex;
        // IF TARGET = NOBODY IT'S A BUFF
        switch (spell.spellName)
        {
            case "Thunder":
                GameObject thunder = Instantiate(spell.vfx, bm.enemySlots[enemyIndex].position, Quaternion.identity) as GameObject;
                Destroy(thunder, 1);
                break;
            case "Fire":
                GameObject fire = Instantiate(spell.vfx, bm.enemySlots[enemyIndex].position, Quaternion.identity) as GameObject;
                AudioClip myClip = spell.clip;
                audioSource.clip = myClip;
                audioSource.Play();
                Destroy(fire, 1);            
                break;
            case "Tornado":
                GameObject tornado = Instantiate(spell.vfx, bm.enemySlots[enemyIndex].position, Quaternion.identity) as GameObject;
                Destroy(tornado, 1);            
                break;
            case "Brimstone":
                GameObject brimstone = Instantiate(spell.vfx, bm.enemySlots[enemyIndex].position, Quaternion.identity) as GameObject;
                Destroy(brimstone, 1);            
                break;
            case "Shooting Star":
                GameObject shootingStar = Instantiate(spell.vfx, bm.enemySlots[enemyIndex].position, Quaternion.identity) as GameObject;
                Destroy(shootingStar, 1);            
                break;
            case "Thunderstorm":
                GameObject thunderstorm = Instantiate(spell.vfx, bm.enemySlots[enemyIndex].position, Quaternion.identity) as GameObject;
                Destroy(thunderstorm, 1);            
                break;
            case "Freeze":
                //spell.source.PlayOneShot(spell.clip, 1.0f);
                //GameObject freeze = spell.source.PlayOneShot(spell.clip, 1.0f);
                GameObject freeze = Instantiate(spell.vfx, bm.enemySlots[enemyIndex].position, Quaternion.identity) as GameObject;
                Destroy(freeze, 1);  
                //Debug.Log(spell.source);
                //Debug.Log(spell.clip);
                break;
            case "Poison":
                GameObject poison = Instantiate(spell.vfx, bm.enemySlots[enemyIndex].position, Quaternion.identity) as GameObject;
                Destroy(poison, 1);
                break;
            default:
                Debug.Log("theres an issue");
                return;
        }
        */
    }
}