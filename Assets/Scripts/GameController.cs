using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour
{
    #region Time System
    [Range(1.0f, 1200.0f)]
    public float dayInSeconds = 120.0f; // Default: 10 Minutes
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
        
        StartCoroutine(TMP_CustomerSpawner());
        StartCoroutine(TMP_AddPainting(g));

    }

    // Update is called once per frame
    void Update()
    {
        // Getting all the time delta errors...wat
        finiteStateMachine.Update(1.0f / 60.0f); // delta is seldom 1/60 here, only FixedUpdate does that
    }

    public void TickTime(float dt)
    {
        currentTimeOfTheDay += dt;
        if (currentTimeOfTheDay >= dayInSeconds) currentTimeOfTheDay -= currentTimeOfTheDay;
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
}
