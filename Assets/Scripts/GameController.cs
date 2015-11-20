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
    }

    // Update is called once per frame
    void Update()
    {
        finiteStateMachine.Update(1.0f / 60.0f);
    }

    public void TickTime(float dt)
    {
        currentTimeOfTheDay += dt;
        if (currentTimeOfTheDay >= dayInSeconds) currentTimeOfTheDay -= currentTimeOfTheDay;
    }
}
