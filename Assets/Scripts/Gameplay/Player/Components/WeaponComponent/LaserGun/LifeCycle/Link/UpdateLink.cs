using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateLink<T> : IUpdate
{
    private readonly IOutputUpdate<T> outputUpdate;
    private readonly IInputUpdate<T> inputUpdate;

    public UpdateLink(IOutputUpdate<T> outputUpdate, IInputUpdate<T> inputUpdate)
    {
        this.outputUpdate = outputUpdate;
        this.inputUpdate = inputUpdate;
    }


    public void Update()
    {
        T output = outputUpdate.Update();
        inputUpdate.Update(output);

    }
}
