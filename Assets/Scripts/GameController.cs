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
        finiteStateMachine.Push(new HUBState());
        FindObjectOfType<Gallery>().AddNewPainting(Painting.CreateSampleTexture2D());
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
}
