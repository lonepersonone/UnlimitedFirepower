using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeCycle
{
    private readonly IEnable enable;
    private readonly IUpdate update;
    private readonly IDisable disable;

    public LifeCycle(IEnable enable, IUpdate update, IDisable disable)
    {
        this.enable = enable;
        this.update = update;
        this.disable = disable;
    }

    public void Enable()
    {
        enable.Enable();
    }

    public void Update()
    {
        update.Update();
    }

    public void Disable()
    {
        disable.Disable();
    }
}
