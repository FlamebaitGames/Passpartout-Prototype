using UnityEngine;
using System.Collections;

public class EndDayState : State {

    public override void Enter()
    {
        menuPanels.SetPanelsToShow(MenuPanels.Panel.WEEKLY_PANEL | MenuPanels.Panel.MONEY_PANEL);
    }

    public override string ToString()
    {
        return "End Day State";
    }
}
