using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    public State current;

    public StateMachine(State state)
    {
        current = state;
        current.Enter();
    }

    public void Update()
    {
        Transition transition = current.CheckTransition();
        if (transition != null)
        {
            State newState = current.GetState(transition);
            DoTransition(transition, newState);
        }
        else
            current.Stay();
    }

    public void DoTransition(Transition transition, State newState)
    {
        if (current != newState)
        {
            current.Exit();
            transition.Fire();
            current = newState;
            current.Enter();
        }
    }
}

public class State
{
    public List<Action> enterActions = new List<Action>();
    public List<Action> stayActions = new List<Action>();
    public List<Action> exitActions = new List<Action>();

    private Dictionary<Transition, State> transitionToStates;

    public State()
    {
        transitionToStates = new Dictionary<Transition, State>();
    }

    public void AddTransition(Transition t, State s)
    {
        transitionToStates[t] = s;
    }

    public Transition CheckTransition()
    {
        foreach (Transition t in transitionToStates.Keys)
            if (t.condition()) return t;
        return null;
    }

    public State GetState(Transition t)
    {
        return transitionToStates[t];
    }

    public void Enter() { foreach (Action a in enterActions) a(); }
    public void Stay() { foreach (Action a in stayActions) a(); }
    public void Exit() { foreach (Action a in exitActions) a(); }
}

public delegate bool Condition();
public delegate void Action();

public class Transition
{

    public Condition condition;
    private List<Action> actions = new List<Action>();

    public Transition(Condition condition = null, Action[] actions = null)
    {
        this.condition = condition;
        if (actions != null) this.actions.AddRange(actions);
    }

    public void Fire()
    {
        if (actions != null) foreach (Action action in actions) action();
    }
}
