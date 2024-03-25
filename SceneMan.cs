using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneMan : MonoBehaviour
{
	public GameObject battleScene;
    public GameObject titleScene;
    public GameObject betweenBattle;
    public GameObject mainCamera;
    public GameObject battleCamera;
    public GameObject mapCamera;
    public GameObject mapCanvas;
    public GameObject mapPlayerTracker;
    public GameObject regularRewardsGO;
    public RegularRewards regularRewards;
    public GameObject relicRewardsGO;
    public GameObject restGO;
    public Shop shop;
    public GameObject shopParent;
    CombineManager cm;
    public RelicRewards relicRewards;
    GameObject outerMapParent;
    public GameObject randomEvent;
    public RandomEvents randomEventScript;
    private Vector3 mapPos;
    private bool movedMap = false;
    Demo demo_script;
    GameManager gm;
    BattleManager bm;

    private void Awake()
    {
        cm = FindObjectOfType<CombineManager>();
        gm = FindObjectOfType<GameManager>();
        bm = FindObjectOfType<BattleManager>();
        demo_script = FindObjectOfType<Demo>();

        // Put this in play button
        //bm.discardPile.AddRange(gm.playerDeck);
        //bm.Shuffle();
        titleScene.SetActive(false);
        mainCamera.SetActive(false);
        relicRewardsGO.SetActive(false);
        battleScene.SetActive(false);
        regularRewardsGO.SetActive(false);
    }
    public void PlayButton()
    {
        //GameObject dupe = GameObject.Instantiate(battleDupe);
        // COMMENT BATTLEEND
        //BattleEnd();
        //relicRewardsGO.SetActive(true);
        /*
        bm.discardPile.AddRange(gm.playerDeck);
        bm.Shuffle();
        titleScene.SetActive(false);
        battleCamera.SetActive(true);
        mainCamera.SetActive(false);
        battleScene.SetActive(true);
        bm.BeginBattle();
        bm.battleStart = true;
        */
    }
    public void RegularBattle()
    {
        battleCamera.SetActive(true);
        DisableMap();
        battleScene.SetActive(true);
        bm.discardPile.AddRange(gm.playerDeck);
        //bm.battleStart = true;
        mainCamera.SetActive(false);
        //bm.SpawnEnemy();
        //demo_script.GetGestures(gm.playerDeck);
        bm.BeginBattle("regular");
    }
    public void BattleEnd()
    {
        battleCamera.SetActive(false);
        mainCamera.SetActive(true);
        battleScene.SetActive(false);
        regularRewardsGO.SetActive(true);
        regularRewards.GetRewards();
    }
    public void ObtainedReward()
    {
        regularRewardsGO.SetActive(false);
        BackToMap();
    }
    public void EliteBattle()
    {
        battleCamera.SetActive(true);
        DisableMap();
        battleScene.SetActive(true);
        bm.discardPile.AddRange(gm.playerDeck);
        //bm.battleStart = true;
        mainCamera.SetActive(false);
        //bm.SpawnElite();
        bm.BeginBattle("elite");
    }
    public void EliteEnd()
    {
        battleCamera.SetActive(false);
        mainCamera.SetActive(true);
        relicRewardsGO.SetActive(true);
        relicRewards.GetRelics();
    }
    public void ObtainedRelic()
    {
        relicRewardsGO.SetActive(false);
        BackToMap();
    }
    public void RestSite()
    {
        mainCamera.SetActive(true);
        DisableMap();
        restGO.SetActive(true);
        cm.SearchForCombos();
    }
    public void ObtainedCombo()
    {
        restGO.SetActive(false);
        BackToMap();
    }
    public void VisitShop()
    {
        mainCamera.SetActive(true);
        DisableMap();
        shopParent.SetActive(true);
        shop.GetShop();
    }
    public void LeaveShop()
    {
        shop.RemoveShopListeners();
        shopParent.SetActive(false);
        BackToMap();
    }
    public void GenerateEvent()
    {
        mainCamera.SetActive(true);
        DisableMap();
        // Set active first
        randomEvent.SetActive(true);
        randomEventScript.GetRandomEvent();
    }
    public void LeaveEvent()
    {
        randomEventScript.RemoveEventListeners();
        randomEvent.SetActive(false);
        BackToMap();
    }
    public void DisableMap()
    {
        mapCamera.SetActive(false);
        mapCanvas.SetActive(false);
        outerMapParent = GameObject.Find("OuterMapParent");
        MoveOuterMap();
        mapPlayerTracker.SetActive(false);
    }
    public void BackToMap()
    {
        mapPlayerTracker.SetActive(true);    
        mapCanvas.SetActive(true);
        MoveOuterMap();
        battleScene.SetActive(false);
        battleCamera.SetActive(false);
        mainCamera.SetActive(false);
        mapCamera.SetActive(true);   
    }
    // I want to move OuterMapParent out of the way
    public void MoveOuterMap()
    {
        if (movedMap == false)
        {
            mapPos = outerMapParent.transform.position;
            movedMap = true;
        }
        if (outerMapParent.transform.position.x == 5000)
        {
            outerMapParent.transform.position = mapPos;
            Debug.Log("x=5000");
            Debug.Log(mapPos);
        }
        else 
        {
            outerMapParent.transform.position = new Vector3(5000,5000,0);
        }
    }
}
