using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionalUpdate : IUpdate
{
    private IUpdateCondition updateCondition;
    private IUpdate updateTrue;
    private IUpdate updateFalse;

    ConditionalUpdate(IUpdateCondition updateCondition, IUpdate updateTrue, IUpdate updateFalse)
    {
        this.updateCondition = updateCondition;
        this.updateTrue = updateTrue;
        this.updateFalse = updateFalse;
    }

    public ConditionalUpdate(IUpdateCondition updateCondition, IUpdate updateTrue) : this(updateCondition, updateTrue, new UpdateDummy()){}

    void IUpdate.Update()
    {
        if(updateCondition.IsTrue())
            updateTrue.Update();
        else
            updateFalse.Update();
    }
}
