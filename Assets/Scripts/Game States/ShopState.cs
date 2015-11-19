using UnityEngine;
using System.Collections;

/// <summary>
/// Represents the state in which the user opens the shop and sells paintings, allowing customers in etc.
/// </summary>
public class ShopState : State {

    public override void Update(float dt)
    {
        base.Update(dt);
    }

    public override string ToString()
    {
        return "Shop State";
    }
}
