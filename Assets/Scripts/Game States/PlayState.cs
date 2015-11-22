using UnityEngine;
using System.Collections;

public class PlayState : State {
    private GameObject currentlySelected;
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
        
    }
    public override string ToString()
    {
        return "Play State";
    }

    private void SpawnCustomer()
    {
        Controller.BroadcastMessage("AddCustomer");
    }
}
