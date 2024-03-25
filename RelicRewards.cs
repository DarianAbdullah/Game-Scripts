using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelicRewards : MonoBehaviour
{
    [SerializeField] RegularRelics regularRelics;
    [SerializeField] RegularRelic relic1;
    [SerializeField] RegularRelic relic2;
    private List<int> randomNumbers = new List<int>();
    private int rng;
    private int sizeoflist;
    //private rand = new Random();

    public void GetRelics()
    {
        sizeoflist = regularRelics.regRelicList.Count;
        for (int i = 0; i < sizeoflist; i++)
        {   
            // Change this to size of list
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
        relic1.LoadRelic(regularRelics.regRelicList[randomNumbers[0]]);
        relic2.LoadRelic(regularRelics.regRelicList[randomNumbers[1]]);
        // Clear or else inf loop
        randomNumbers.Clear();
    }
}
