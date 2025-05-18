using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateComposite : IUpdate
{
    private IUpdate[] elements;
 
    public UpdateComposite(params IUpdate[] elements)
    {
        this.elements = elements;
    }

    public void Update()
    {
        foreach(var element in elements)
        {
            element.Update();
        }
    }
}
