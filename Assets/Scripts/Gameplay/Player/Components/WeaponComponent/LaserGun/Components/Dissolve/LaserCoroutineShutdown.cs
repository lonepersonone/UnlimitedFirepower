using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 激光消失时模块线程处理
/// </summary>
public class LaserCoroutineShutdown : IEnable, IDisable
{
    private ICoroutineRunner coroutine;
    private LaserDissolve laserDissolve;
    private LaserLength laserLength;

    public LaserCoroutineShutdown(ICoroutineRunner coroutine, LaserDissolve laserDissolve, LaserLength laserLength)
    {
        this.coroutine = coroutine;
        this.laserLength = laserLength;
        this.laserDissolve = laserDissolve;
    }

    public void Enable()
    {
        coroutine.StopAllCoroutines();
    }

    public void Disable()
    {
        coroutine.StartCoroutine(laserDissolve.Dissaper());

        //laserDissolve.SetZero();
        //laserLength.SetLaserToZero();

    }

}
