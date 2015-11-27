using UnityEngine;
using System.Collections;

public class PlayState : State {
    private GameObject currentlySelected;
    private Player player;

    private string[] dayPhases = { "Morning", "Day", "Evening", "Night" };
    public PlayState()
    {
        player = GameObject.FindObjectOfType<Player>();
    }
    public override void Enter()
    {
        menuPanels.SetPanelsToShow(MenuPanels.Panel.MONEY_PANEL | MenuPanels.Panel.PAINTING_CANVAS);
    }
    public override void Update(float dt)
    {
        base.Update(dt);

        if (Input.GetMouseButtonDown(0))
        {
            Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 50.0f))
            {
                if (!hit.collider.gameObject.CompareTag("DoesNotDeselect"))
                {
                    if (currentlySelected != null)
                    {
                        if (currentlySelected.activeSelf)
                            currentlySelected.SendMessage("OnDeselected", SendMessageOptions.DontRequireReceiver);
                        currentlySelected = null;
                    }
                    currentlySelected = hit.collider.gameObject;
                    currentlySelected.SendMessage("OnSelected", SendMessageOptions.DontRequireReceiver);
                }
                
            }
            else
            {
                if (currentlySelected != null && currentlySelected.activeSelf)
                {
                    currentlySelected.SendMessage("OnDeselected", SendMessageOptions.DontRequireReceiver);
                    currentlySelected = null;
                }
            }
        }
        menuPanels.context.timeOfDayText.text = dayPhases[(int)((Controller.currentTimeOfTheDay / Controller.dayInSeconds) * (dayPhases.Length-1))];
        
        
    }

    private void UpdateMoneyFameText(string money, string fame)
    {
        menuPanels.context.goldFameText.text = money + " Euro\n" + "Fame: " + fame;
    }
    public override string ToString()
    {
        return "Play State";
    }

    private void SpawnCustomer()
    {
        Controller.BroadcastMessage("AddCustomer");
    }

    public override void RefreshUI()
    {
        if (player != null)
        {
            UpdateMoneyFameText(player.money.ToString(), player.fame.ToString());
        }
        else
        {
            Debug.LogError("Player not linked!");
            Debug.Break();
        }
    }
}
