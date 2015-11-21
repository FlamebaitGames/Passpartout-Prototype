using UnityEngine;
using System.Collections;

public class PlayState : State {

    public override void Enter()
    {
        menuPanels.SetPanelsToShow(MenuPanels.Panel.MONEY_PANEL | MenuPanels.Panel.PAINTING_CANVAS);
    }

    public override string ToString()
    {
        return "Play State";
    }
}
