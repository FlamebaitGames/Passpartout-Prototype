using UnityEngine;
using System.Collections;

// This is not a abstract-class, reason  is I want logic to be run in the base-class.
public class State {
    public static GameController Controller { get; set; }
    public static MenuPanels menuPanels { get; set; }
    public bool Finished { get; set; }

    
    public virtual void Enter()
    {
        menuPanels.SetPanelsToShow(MenuPanels.Panel.NONE);
    }

    public virtual void Exit()
    {

    }

    public virtual void Update(float dt)
    {
        // Tick the world time here, if a state do not wish to do so (HUB-state for example) just don't call State.Update(dt) in child Update-method.
        Controller.TickTime(dt);
    }

    public virtual void RefreshUI()
    {

    }
}
