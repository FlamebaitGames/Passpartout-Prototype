using UnityEngine;
using System.Collections;

public class Game : MonoBehaviour {
    FSM finiteStateMachine = new FSM();

	// Use this for initialization
	void Start () {
        finiteStateMachine.Push(new HUBState());
	}
	
	// Update is called once per frame
	void Update () {
        finiteStateMachine.Update(Time.deltaTime);
	}
}
