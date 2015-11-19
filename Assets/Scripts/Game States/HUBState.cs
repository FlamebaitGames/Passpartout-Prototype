using UnityEngine;
using System.Collections;

/// <summary>
/// Represents the state in which the player chooses the task for the day.
/// </summary>
public class HUBState : State {

    public override void Update(float dt)
    {
        // base.Update(dt); Don't tick time when in HUB State.
    }

    public override string ToString()
    {
        return "HUB State";
    }
}
