using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicScrollView : MonoBehaviour
{
    CombineManager cm;
    [SerializeField]
    private Transform scrollViewContent;
    [SerializeField]
    public GameObject spellRow;



    void Start()
    {
        cm = FindObjectOfType<CombineManager>();
    }
}
