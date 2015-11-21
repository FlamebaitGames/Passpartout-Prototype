using UnityEngine;
using System.Collections;

[UsedInTweaker]
public class GameController : MonoBehaviour
{
    #region Time System
    [Range(1.0f, 1200.0f)]
    [ShowInTweaker]
    public float dayInSeconds = 120.0f; // Default: 10 Minutes
    [HideInInspector]
    public float currentTimeOfTheDay = 0.0f;
    #endregion
    FSM finiteStateMachine = new FSM();
    
    // Use this for initialization
    void Start()
    {
        State.Controller = this;
        State.menuPanels = GetComponentInChildren<MenuPanels>();
        Debug.Assert(State.menuPanels != null, "Menu Panels have not been added to the scene! Drag the canvas prefab into scene");
        finiteStateMachine.Push(new HUBState());
        Gallery g = FindObjectOfType<Gallery>();
        
        //StartCoroutine(TMP_CustomerSpawner());
        //StartCoroutine(TMP_AddPainting(g));

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

    private IEnumerator TMP_CustomerSpawner()
    {
        while (true)
        {
            BroadcastMessage("AddCustomer");
            yield return new WaitForSeconds(18.0f);
        }
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
        finiteStateMachine.Push(new PlayState());
    }

    private void StartDay()
    {
        finiteStateMachine.Pop();
        currentTimeOfTheDay = 0.0f;
    }

    private void EndDay()
    {
        finiteStateMachine.Push(new EndDayState());
    }
}
