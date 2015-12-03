using UnityEngine;
using System.Collections;

[TweakableClass]
public class GameController : MonoBehaviour
{
    #region Time System
    [Range(1.0f, 1200.0f)]
    [TweakableField]
    public float dayInSeconds = 120.0f; // Default: 10 Minutes
    [HideInInspector]
    public float currentTimeOfTheDay = 0.0f;
    #endregion
    FSM finiteStateMachine = new FSM();

    // PlayState Fields
    [TweakableField]
    public float maxCustomerLapTime = 40.0f;
    [TweakableField]
    public float customerSpawnInterval = 24.0f;
    [TweakableField, Range(0.0f, 20.0f)]
    public float customerSpawnDeviation = 3.0f;

    // EndDayState Fields
    [TweakableField]
    public string[] endDayQuotes;

    // EndWeekState Fields
    [TweakableField]
    public int rentCost = 120;
    [TweakableField]
    public int foodCost = 60;
    [TweakableField]
    public int foodCostDeviation = 10;
    [TweakableField]
    public int baguetteCost = 15;
    [TweakableField]
    public int baguetteDeviation = 5;
    [TweakableField]
    public int wineCost = 25;
    [TweakableField]
    public int wineCostDeviation = 10;
    [TweakableField]
    public int unionCost = 5;


    public int day { get; private set; }
    public int week { get; private set; }
    // Use this for initialization
    void Start()
    {
        State.Controller = this;
        State.menuPanels = GetComponentInChildren<MenuPanels>();
        Gallery g = FindObjectOfType<Gallery>();
        //StartCoroutine(TMP_AddPainting(g));
        Debug.Assert(State.menuPanels != null, "Menu Panels have not been added to the scene! Drag the canvas prefab into scene");
        
        Reset();
        

    }

    // Update is called once per frame
    void Update()
    {
        finiteStateMachine.Update(Time.deltaTime);
    }

    public void TickTime(float dt)
    {
        currentTimeOfTheDay += dt;
        if (currentTimeOfTheDay >= dayInSeconds) EndDay();
    }

    private IEnumerator TMP_AddPainting(Gallery g)
    {
        while (true)
        {
            g.AddNewPainting(Painting.CreateSampleTexture2D());
            yield return new WaitForSeconds(5.0f);
        }
    }


    //
    //      EVENTS BELOW
    //
    private void StartGame()
    {
        day = 1;
        week = 1;
        finiteStateMachine.Push(new PlayState());
        BroadcastMessage("OnStartGame");
        
    }

    private void StartDay()
    {
        finiteStateMachine.Pop();
        currentTimeOfTheDay = 0.0f;
    }

    private void EndDay()
    {
        BroadcastMessage("OnEndDay", SendMessageOptions.DontRequireReceiver);
        if (day >= 7)
        {
            week++;
            day = 1;
            finiteStateMachine.Push(new EndWeekState());
        }
        else
        {
            day++;
            finiteStateMachine.Push(new EndDayState());
            
        }
        
    }

    private void Reset()
    {
        BroadcastMessage("OnReset", SendMessageOptions.DontRequireReceiver);
        finiteStateMachine.Clear();
        finiteStateMachine.Push(new HUBState());
    }

    private void OnLose()
    {
        

        finiteStateMachine.Push(new LoseState());
    }

    private void OnRefreshUI()
    {
        finiteStateMachine.Peek().RefreshUI();
    }
}
