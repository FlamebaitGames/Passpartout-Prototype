﻿using UnityEngine;
using System.Collections;

public class EndWeekState : State {

    public int rentExpense;
    public int foodExpense;
    public int baguettesExpense;
    public int wineExpense;
    public int unionExpense;

    public override void Enter()
    {
        menuPanels.SetPanelsToShow(MenuPanels.Panel.WEEKLY_PANEL | MenuPanels.Panel.MONEY_PANEL);
        rentExpense = Controller.rentCost;
        foodExpense = Controller.foodCost + Random.Range(0, Controller.foodCostDeviation);
        baguettesExpense = Controller.baguetteCost + Random.Range(0, Controller.baguetteDeviation);
        wineExpense = Controller.wineCost + Random.Range(0, Controller.wineCost);
        unionExpense = Controller.unionCost;

        Player player = Controller.GetComponent<Player>();
        player.RemoveMoney(rentExpense + foodExpense + baguettesExpense + wineExpense + unionExpense);

        SetWeeklyInfo(
            "Rent: " + rentExpense +
            "\nFood: " + foodExpense +
            "\nBaguettes: " + baguettesExpense +
            "\nWine: " + wineExpense + 
            "\nUnion: " + unionExpense + 
            "\n\nBalance: " + player.money);
    }

    public override void Update(float dt)
    {
        UnityEngine.EventSystems.RaycastResult r;
        
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

