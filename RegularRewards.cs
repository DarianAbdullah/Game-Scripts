using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RegularRewards : MonoBehaviour
{
    [SerializeField] RegularSpells regularSpells;
    [SerializeField] SpellUI card1;
    [SerializeField] SpellUI card2;
    [SerializeField] SpellUI card3;
    private List<int> randomNumbers = new List<int>();
    private int rng;
    private int sizeoflist;
    //private rand = new Random();

    public void GetRewards()
    {
        sizeoflist = regularSpells.regSpellList.Count;
        for (int i = 0; i < 3; i++)
        {   
            rng = Random.Range(0,sizeoflist);
            if (i != 0 && randomNumbers.Contains(rng))
            {
                i--;
            }
            else
            {
                randomNumbers.Add(rng);
            }
        }
        card1.LoadSpell(regularSpells.regSpellList[randomNumbers[0]]);
        card2.LoadSpell(regularSpells.regSpellList[randomNumbers[1]]);
        card3.LoadSpell(regularSpells.regSpellList[randomNumbers[2]]);
        // Clear or else inf loop
        randomNumbers.Clear();
    }
}
