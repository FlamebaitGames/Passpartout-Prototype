using UnityEngine;
using System.Collections;
[TweakableClass]
public class Player : MonoBehaviour {

	public AudioSource sellSound;

    [TweakableField, SerializeField]
    private int startingMoney;
    [TweakableField, SerializeField]
    private int startingFame = 3;
    public int money { get; private set; }

    public int fame { get; private set; }
    private int famePoints;

    private int[] fameReq = {
                                2,
                                5,
                                9,
                                14,
                                20,
                                28,
                                38,
                                50,
                                80,
                                120
                            };


    void OnStartGame()
    {
        fame = startingFame;
        famePoints = fameReq[fame - 1];
        SetMoney(startingMoney);
        
    }


    public bool CanAfford(int cost)
    {
        return money - cost >= 0;
    }

    public void RemoveMoney(int cost)
    {
        //Debug.Assert(CanAfford(cost), "Can't afford to pay for this!");
        money -= cost;
        MenuPanels p = FindObjectOfType<MenuPanels>();
        p.context.moneyPopper.Play(-cost);
        SendMessage("OnRefreshUI");
        if (money < 0) SendMessage("OnLose");
        
    }

    public void AddMoney(int cost)
    {
		sellSound.Play ();
        money += cost;
        MenuPanels p = FindObjectOfType<MenuPanels>();
        p.context.moneyPopper.Play(cost);
        SendMessage("OnRefreshUI");
    }

    public void SetMoney(int money)
    {
        this.money = money;
        SendMessage("OnRefreshUI");
    }

    public void IncrementFamePoints()
    {
        famePoints++;
        if (famePoints >= fameReq[fame])
        {
            fame++;
            SendMessage("OnRefreshUI");
        }
    }

    public void DecrementFamePoints()
    {
        famePoints--;
        if (famePoints < fameReq[fame-1])
        {
            fame--;
            SendMessage("OnRefreshUI");
        }
    }
}
