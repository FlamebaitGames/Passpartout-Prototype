using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FSM {
    Stack<State> states = new Stack<State>();

	// Update is called once per frame
	public void Update (float dt) {
        State current = Peek();
        if (current != null)
        {
            current.Update(dt);
        }
	}

    public State Peek()
    {
        return (states.Count != 0) ? states.Peek() : null;
    }

    public void Push(State state)
    {
        State old = Peek();
        if (old != null)
        {
            old.Exit();
        }
        states.Push(state);
        state.Enter();
    }

    public State Pop()
    {
        if (states.Count > 0)
        {
            states.Peek().Exit();
            State s = states.Pop();
            states.Peek().Enter();
            return s;
        }
        else
        {
            return null;
        }
    }

    public void Clear()
    {
        states.Clear();
    }
}
