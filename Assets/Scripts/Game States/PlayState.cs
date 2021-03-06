﻿using UnityEngine;
using System.Collections;

public class PlayState : State {
    private Player player;

    private float spawnTicker = 0.0f;
    private float spawnTime { get { return Controller.customerSpawnInterval; } }
    private float spawnDeviation { get { return Controller.customerSpawnDeviation; } }
    private float maxLapTime { get { return Controller.maxCustomerLapTime; } }

    private string[] dayPhases = { "Morning", "Day", "Evening", "Night" };
    public PlayState()
    {
        player = GameObject.FindObjectOfType<Player>();
        spawnTicker = spawnTime;
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
            if(!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) {
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 50.0f))
                {
                    if (!hit.collider.gameObject.CompareTag("DoesNotDeselect"))
                    {
                        /*if (currentlySelected != null)
                        {
                            if (currentlySelected.activeSelf)
                                currentlySelected.SendMessage("OnDeselected", SendMessageOptions.DontRequireReceiver);
                            currentlySelected = null;
                        }
                        currentlySelected = hit.collider.gameObject;
                        currentlySelected.SendMessage("OnSelected", SendMessageOptions.DontRequireReceiver);*/
                        menuPanels.context.paintingSettingsPanel.Unlink();
                        Painting p;
                        if ((p = hit.collider.gameObject.GetComponent<Painting>()) != null)
                            menuPanels.context.paintingSettingsPanel.Link(p);
                    }
                
                }
                else
                {
                    /*if (currentlySelected != null && currentlySelected.activeSelf)
                    {
                        currentlySelected.SendMessage("OnDeselected", SendMessageOptions.DontRequireReceiver);
                        currentlySelected = null;
                    }*/
                    menuPanels.context.paintingSettingsPanel.Unlink();
                }
            }
            
        }
        menuPanels.context.timeOfDayText.text = dayPhases[(int)((Controller.currentTimeOfTheDay / Controller.dayInSeconds) * (dayPhases.Length-1))];
        spawnTicker -= Time.deltaTime;
        if (spawnTicker <= 0.0f)
        {
            spawnTicker = spawnTime + Random.Range(0.0f, spawnDeviation);
            SpawnCustomer();
        }
    }

    private void UpdateMoneyFameText(string money, string fame)
    {
        menuPanels.context.goldFameText.text = money + " Euro\n" + player.fameTitle;
    }
    public override string ToString()
    {
        return "Play State";
    }

    private void SpawnCustomer()
    {
        if (Controller.dayInSeconds - Controller.currentTimeOfTheDay > maxLapTime)
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
