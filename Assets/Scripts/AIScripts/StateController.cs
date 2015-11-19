using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class StateController : MonoBehaviour {

    protected List<State> States { get; private set; }

    protected State CurrentState { get; private set; }

    public StateController()
    {
        States = new List<State>(); 
    }

    protected virtual void Update()
    {
        if (CurrentState != null)
        {
            print(CurrentState.GetKey());
            CurrentState.Update(Time.deltaTime); // update the state
        }
    }

    public void ChangeState(string state)
    {
        foreach (State s in States)
        {
            if (s.GetKey() == state)
            {
                CurrentState = s;
                break;
            }
        }
    }

    public void Print(string output) // help for debugging
    {
        print(output);
    }
}

    

