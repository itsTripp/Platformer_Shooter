using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChoosePerk : MonoBehaviour
{
    public static ChoosePerk choosePerk;

    public GameObject perkCard;
    public PerkCards _scriptablePerkCard;
    public int perkCost;

    private PerkSystem _perkSystem;
    // Start is called before the first frame update
    void Start()
    {
        _perkSystem = GameObject.Find("PerkSystem").GetComponent<PerkSystem>();
        if (_perkSystem == null)
        {
            Debug.Log("PerkSystem is Null in Choose Perk");
        }
        perkCost = _scriptablePerkCard.perkCost;
    }
}


