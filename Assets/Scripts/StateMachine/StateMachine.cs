using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class StateMachine
{
    private State _currentState;
    private List<Transition> _transitions = new List<Transition>();

    public void ChangeState(State newState)
    {
        _currentState?.OnExit();

        _currentState = newState;
        
        _currentState.OnEnter();
    }

    public void UpdateState()
    {
        if (_currentState is not null)
        {
            _currentState.OnUpdate();
        }
        else
        {
            Debug.LogWarning("No current state");
        }
        
        CheckTransition();
    }

    private void CheckTransition()
    {
        var possibleTransition = _transitions.FirstOrDefault(transition => transition._from == _currentState && transition._condition());
        ChangeState(possibleTransition._to);
    }

    public void AddTransition(State from, State to, Func<bool> condition)
    {
        if (from != null && to != null && condition != null)
        {
            _transitions.Add(new Transition(from, to, condition));
        }
    }
}

public class Transition
{
    public State _from;
    public State _to;
    public Func<bool> _condition;
    
    public Transition(State from, State to, Func<bool> condition)
    {
        _from = from;
        _to = to;
        _condition = condition;
    }
}
