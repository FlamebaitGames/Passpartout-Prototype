using UnityEngine;
using System.Collections;
[TweakableClass]
public class Player : MonoBehaviour {

    [TweakableField, SerializeField]
    private int startingMoney;

    public int money { get; private set; }

    void OnStartGame()
    {
        money = startingMoney;
    }

}
