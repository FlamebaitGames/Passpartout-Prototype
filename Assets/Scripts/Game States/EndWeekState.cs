using UnityEngine;
using System.Collections;

public class EndWeekState : State {

    public override void Enter()
    {
        menuPanels.SetPanelsToShow(MenuPanels.Panel.WEEKLY_PANEL | MenuPanels.Panel.MONEY_PANEL);
        SetWeeklyInfo("Rent: 10\nFood: 133");
    }

    public override void Update(float dt)
    {
        
    }

    public override void Exit()
    {
        
    }

    private void SetWeeklyInfo(string info)
    {
        menuPanels.context.weeklyExpensesText.text =
            "Weekly Expenses\n------------------\n" + info;
    }
}

