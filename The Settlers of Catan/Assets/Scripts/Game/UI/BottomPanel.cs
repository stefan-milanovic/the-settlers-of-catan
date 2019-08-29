﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BottomPanel : MonoBehaviour
{

    private enum PanelCode
    {
        RESOURCES,
        DEVELOPMENT,
        CONSTRUCTION,
        SPECIAL,
        TRADE,
        ROLL_DICE
    };

    PanelCode currentOpenPanel = PanelCode.RESOURCES;

    private bool tradePanelOpen = false;

    [SerializeField]
    private Button rollDiceButton;

    [SerializeField]
    private Button endTurnButton;

    [SerializeField]
    private GameObject tradePanel;


    // Start is called before the first frame update
    void Start()
    {
        DisplayPanel("ResourcePanel", true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool TradePanelOpen() { return tradePanelOpen; }
    public void OpenResourcesTab()
    {
        TogglePanel(PanelCode.RESOURCES);
    }

    public void OpenDevelopmentTab()
    {
        TogglePanel(PanelCode.DEVELOPMENT);
    }

    public void OpenConstructionTab()
    {
        TogglePanel(PanelCode.CONSTRUCTION);
    }

    public void OpenSpecialTab()
    {
        TogglePanel(PanelCode.SPECIAL);
    }

    public void OpenTradeTab()
    {
        // if (!alreadyTrading || !alreadyPending)

        GamePlayer localPlayer = GamePlayer.FindLocalPlayer();

        if (!localPlayer.IsMyTurn() || localPlayer.GetPhase() != GamePlayer.Phase.TRADE_BUILD_IDLE)
        {
            return;
        }

        TogglePanel(PanelCode.TRADE);
    }
    

    private void TogglePanel(PanelCode panelToOpen)
    {

        string prefix = FindPrefix(panelToOpen);

        string panelName = prefix + "Panel";
        string buttonTag = prefix + "Button";
        
        
        string oldPrefix = FindPrefix(currentOpenPanel);
        
        string oldPanelName = oldPrefix + "Panel";
        string oldButtonTag = oldPrefix + "Button";

        if (panelToOpen != PanelCode.TRADE)
        {
            DisplayPanel(oldPanelName, false);
            ToggleButton(oldButtonTag, true);

            DisplayPanel(panelName, true);
            ToggleButton(buttonTag, false);

            currentOpenPanel = panelToOpen;
        }
        else
        {
            if (!tradePanelOpen)
            {
                tradePanel.SetActive(tradePanelOpen = true);
            }
            else
            {
                tradePanel.SetActive(tradePanelOpen = false);
            }
        }

        

        
      
    }

    private string FindPrefix(PanelCode? panelToOpen)
    {
        switch (panelToOpen)
        {
            case PanelCode.RESOURCES: return "Resource";
            case PanelCode.DEVELOPMENT: return "Development";
            case PanelCode.CONSTRUCTION: return "Construction";
            case PanelCode.SPECIAL: return "Special";
            case PanelCode.TRADE: return "Trade";
        }

        // In case of invalid PanelCode - should never occur
        return "";
    }

    private void DisplayPanel(string panelName, bool visibility)
    {
        GameObject panel = GameObject.Find(panelName);

        panel.GetComponent<CanvasGroup>().alpha = (visibility) ? 1 : 0;

        // if it's a card panel, show/hide its cards

        CardPanel cardPanel = panel.GetComponent<CardPanel>();
        cardPanel.SetVisible(visibility);
    }

    private void ToggleButton(string buttonTag, bool interactable)
    {

        // Only toggles if it's a button from the main menu panel at the bottom of the screen.
        
        Button button = GameObject.FindGameObjectWithTag(buttonTag).GetComponent<Button>();
        
        ColorBlock colourBlock = button.colors;

        Color oldNormal = colourBlock.normalColor;
        colourBlock.normalColor = colourBlock.pressedColor;
        colourBlock.highlightedColor = oldNormal;
        colourBlock.pressedColor = oldNormal;
        
        button.colors = colourBlock;

        GameObject.FindGameObjectWithTag(buttonTag).GetComponent<Image>().color = colourBlock.normalColor;

        // Disable the button.
        button.interactable = interactable;

    }


    #region Bottom Right Buttons

    public void DisplayRollDiceButton()
    {
        rollDiceButton.gameObject.SetActive(true);
    }

    public void HideRollDiceButton()
    {
        rollDiceButton.gameObject.SetActive(false);
    }

    public void EnableRollDiceButton()
    {
        rollDiceButton.interactable = true;
    }

    public void DisableRollDiceButton()
    {
        rollDiceButton.interactable = false;
    }

    public void DisplayEndTurnButton()
    {
        endTurnButton.gameObject.SetActive(true);
    }

    public void HideEndTurnButton()
    {
        endTurnButton.gameObject.SetActive(false);
    }

    public void EnableEndTurnButton()
    {
        endTurnButton.interactable = true;
    }

    public void DisableEndTurnButton()
    {
        endTurnButton.interactable = false;
    }

    public void EnableRolling()
    {

        if (endTurnButton.IsActive())
        {
            HideEndTurnButton();
        }

        if (!rollDiceButton.IsActive())
        {
            DisplayRollDiceButton();
        }

        EnableRollDiceButton();


    }

    public void EnableEndingTurn()
    {
        if (rollDiceButton.IsActive())
        {
            HideRollDiceButton();
        }

        if (!endTurnButton.IsActive())
        {
            DisplayEndTurnButton();
        }

        EnableEndTurnButton();
    }

    public void DisableEndingTurn()
    {
        DisableEndTurnButton();
    }

    #endregion

}
