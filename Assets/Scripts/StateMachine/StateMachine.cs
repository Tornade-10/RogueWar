using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class StateMachine
{
    private IState _currentState;
    private List<Transition> _transitions = new List<Transition>();

    public void ChangeState(IState newState)
    {
        _currentState?.OnExit();

        _currentState = newState;
        
        _currentState.OnEnter();
    }

    public void UpdateState()
    {
        if (_currentState is not null)
        {
            Debug.Log("current state : " + _currentState.GetType().Name);
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
        if (possibleTransition is not null)
        {
            ChangeState(possibleTransition._to);
        }
        
    }

    public void AddTransition(IState from, IState to, Func<bool> condition)
    {
        if (from != null && to != null && condition != null)
        {
            _transitions.Add(new Transition(from, to, condition));
        }
    }
}

public class Transition
{
    public IState _from;
    public IState _to;
    public Func<bool> _condition;
    
    public Transition(IState from, IState to, Func<bool> condition)
    {
        _from = from;
        _to = to;
        _condition = condition;
    }
}
