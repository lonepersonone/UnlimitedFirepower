using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switcher : IUpdateCondition
{
    public bool Enabled { get; private set; }

    public bool TryEnable()
    {
        return TrySetState(true);
    }

    public bool TryDisable()
    {
        return TrySetState(false);
    }

    public bool TrySetState(bool targetState)
    {
        bool stateChanged = Enabled != targetState;
        if (Enabled != targetState)
            Enabled = targetState;
        return stateChanged;
    }

    public bool IsTrue() => Enabled;
}
