using UnityEngine;
using System.Collections;

/// <summary>
/// Represents the state in which the user makes paintings (including, naming, setting base price etc)
/// </summary>
public class PaintState : State {

    public override void Update(float dt)
    {
        base.Update(dt);
    }

    public override string ToString()
    {
        return "Paint State";
    }
}
