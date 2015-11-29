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

    [TweakableField]
    public float maxCustomerLapTime = 40.0f;
    [TweakableField]
    public float customerSpawnInterval = 24.0f;
    [TweakableField, Range(0.0f, 20.0f)]
    public float customerSpawnDeviation = 3.0f;

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
        finiteStateMachine.Push(new HUBState());

        g.AddNewPainting(Painting.CreateSampleTexture2D());
        g.AddNewPainting(Painting.CreateSampleTexture2D());
        g.AddNewPainting(Painting.CreateSampleTexture2D());
        g.AddNewPainting(Painting.CreateSampleTexture2D());

        g.AddNewPainting(Painting.CreateSampleTexture2D());
        g.AddNewPainting(Painting.CreateSampleTexture2D());
        g.AddNewPainting(Painting.CreateSampleTexture2D());
        g.AddNewPainting(Painting.CreateSampleTexture2D());
        
        

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
        finiteStateMachine.Push(new PlayState(customerSpawnInterval, customerSpawnDeviation, maxCustomerLapTime));
        BroadcastMessage("OnStartGame");
        
    }

    private void StartDay()
    {
        finiteStateMachine.Pop();
        currentTimeOfTheDay = 0.0f;
    }

    private void EndDay()
    {
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
}
