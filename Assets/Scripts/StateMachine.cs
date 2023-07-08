using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct State
{
    public string name;
    public System.Action OnEnter;
    public System.Action OnExit;
    //OnUpdate is a function that returns a State
    public System.Func<State> OnUpdate;

    public State(string name, System.Action OnEnter, System.Action OnExit, System.Func<State> OnUpdate)
    {
        this.name = name;
        this.OnEnter = OnEnter;
        this.OnExit = OnExit;
        this.OnUpdate = OnUpdate;
    }
}

public class StateMachine
{
    public State currentState;
    public StateMachine(State startingState)
    {
        currentState = startingState;
        currentState.OnEnter();
    }

    public void Update()
    {
        State nextState = currentState.OnUpdate();
        if (nextState.name != currentState.name)
        {
            currentState.OnExit();
            currentState = nextState;
            currentState.OnEnter();
        }
    }
}
