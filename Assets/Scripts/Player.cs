using UnityEngine;
using System.Collections;
[TweakableClass]
public class Player : MonoBehaviour {

    [TweakableField, SerializeField]
    private int startingMoney;

    public int money { get; private set; }

    public int fame = 2;

    void OnStartGame()
    {
        money = startingMoney;
    }

}
