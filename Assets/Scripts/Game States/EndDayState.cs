using UnityEngine;
using System.Collections;

public class EndDayState : State {

    public string quote
    {
        get
        {
            if (Controller.endDayQuotes.Length == 0) return "ERR: No Quotes!";
            return Controller.endDayQuotes[Random.Range(0, Controller.endDayQuotes.Length)];
        }
    }
    public override void Enter()
    {
        menuPanels.SetPanelsToShow(MenuPanels.Panel.WEEKLY_PANEL | MenuPanels.Panel.MONEY_PANEL);
        
        WriteDailyTip();
    }

    public override void Update(float dt)
    {
        
    }

    public override string ToString()
    {
        return "End Day State";
    }

    private void WriteDailyTip()
    {
        menuPanels.context.weeklyExpensesText.text = "Day is over\n" + quote;
    }
    
}
