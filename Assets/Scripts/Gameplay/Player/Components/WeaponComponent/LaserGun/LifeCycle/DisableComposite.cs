using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableComposite : IDisable
{
    private IDisable[] elements;

    public DisableComposite(params IDisable[] elements)
    {
        this.elements = elements;
    }

    public void Disable()
    {
        foreach(var element in elements)
        {
            element.Disable();
        }
    }

}
