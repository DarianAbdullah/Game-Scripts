using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff : MonoBehaviour
{

    [System.Serializable]
    public enum BuffType
    {
        strength,
        block,
        vulnerable,
        weak,
        ritual,
        enrage
    }
    public Sprite buffIcon;
    [Range(0,999)]
    public int buffValue;
}
