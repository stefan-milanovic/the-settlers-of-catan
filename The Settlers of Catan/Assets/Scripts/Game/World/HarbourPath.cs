﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HarbourPath : WorldPath
{

    private const int HARBOUR_TEXT_CHILD_ID = 2;

    public enum HarbourBonus
    {
        TWO_TO_ONE_BRICK,
        TWO_TO_ONE_GRAIN,
        TWO_TO_ONE_LUMBER,
        TWO_TO_ONE_ORE,
        TWO_TO_ONE_WOOL,
        THREE_TO_ONE,
    }

    private HarbourBonus harbourBonus;
    

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetHarbourBonus(HarbourBonus bonus)
    {
        this.harbourBonus = bonus;
        SetHarbourText();
    }

    public HarbourBonus GetHarbourBonus()
    {
        return this.harbourBonus;
    }

    private void SetHarbourText()
    {
        string harbourText = "";

        switch (harbourBonus)
        {
            case HarbourBonus.THREE_TO_ONE:
                harbourText = "3:1";
                break;
            case HarbourBonus.TWO_TO_ONE_BRICK:
                harbourText = "2:1\n<color=red>Brick</color>";
                break;
            case HarbourBonus.TWO_TO_ONE_GRAIN:
                harbourText = "2:1\n<color=yellow>Grain</color>";
                break;
            case HarbourBonus.TWO_TO_ONE_LUMBER:
                harbourText = "2:1\n<color=green>Lumber</color>";
                break;
            case HarbourBonus.TWO_TO_ONE_ORE:
                harbourText = "2:1\n<color=blue>Ore</color>";
                break;
            case HarbourBonus.TWO_TO_ONE_WOOL:
                harbourText = "2:1\n<color=orange>Wool</color>";
                break;
        }

        gameObject.transform.GetChild(HARBOUR_TEXT_CHILD_ID).gameObject.SetActive(true);
        gameObject.transform.GetChild(HARBOUR_TEXT_CHILD_ID).GetComponent<TextMeshPro>().text = harbourText;
        
    }
}
