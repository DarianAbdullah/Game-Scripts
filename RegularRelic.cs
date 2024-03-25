using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RegularRelic : MonoBehaviour
{
    GameManager gm;
    BattleManager bm;
    SceneMan sm;
    RegularRelics regularRelics;
    public Relic relic;
	public TMP_Text relicName;
    public Image relicIcon;
    public TMP_Text relicDescription;

    private void Awake()
    {
        regularRelics = FindObjectOfType<RegularRelics>();
        gm = FindObjectOfType<GameManager>();
        bm = FindObjectOfType<BattleManager>();
        sm = FindObjectOfType<SceneMan>();
    }
    public void LoadRelic(Relic _relic)
    {
        relic = _relic;
        relicName.text = relic.relicName;
        relicDescription.text = relic.relicDescription;
        relicIcon.sprite = relic.relicIcon;
        //GetComponent<Image>().sprite = relic.relicIcon;
    }
    public void AddRelic()
    {
        gm.relics.Add(relic);
        gm.ActivateRelic();
        sm.ObtainedRelic();
    }

}
