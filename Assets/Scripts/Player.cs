using UnityEngine;
using System.Collections;
[TweakableClass]
public class Player : MonoBehaviour {

    [TweakableField, SerializeField]
    private int startingMoney;

    public int money { get; private set; }

    public int fame = 2;

    void OnStartGame()
    {
        money = startingMoney;
    }


    public bool CanAfford(int cost)
    {
        return money - cost >= 0;
    }

    public void RemoveMoney(int cost)
    {
        Debug.Assert(CanAfford(cost), "Can't afford to pay for this!");
        money -= cost;
    }

    public void AddMoney(int cost)
    {
        money += cost;
    }

    public void SetMoney(int money)
    {
        this.money = money;
    }
}
