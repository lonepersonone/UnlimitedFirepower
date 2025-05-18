using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableComposite : IEnable
{
    private IEnable[] elements;

    public EnableComposite(params IEnable[] elements)
    {
        this.elements = elements;
    }

    public void Enable()
    {
        foreach(var element in elements)
        {
            element.Enable();
        }
    }

}
