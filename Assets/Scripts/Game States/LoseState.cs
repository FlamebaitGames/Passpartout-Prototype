using UnityEngine;
using System.Collections;

public class LoseState : State {

    public override void Enter()
    {
        menuPanels.SetPanelsToShow(MenuPanels.Panel.LOSE_PANEL | MenuPanels.Panel.MONEY_PANEL);
        menuPanels.context.weeklyExpensesText.text = "You have lost";
    }

    public override void Update(float dt)
    {

    }

    
}
