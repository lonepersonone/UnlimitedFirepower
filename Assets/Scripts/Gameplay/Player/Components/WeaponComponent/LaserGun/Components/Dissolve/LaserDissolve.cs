using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 激光消失动画
/// </summary>
public class LaserDissolve:IEnable
{
    private readonly ViewData viewData;
    public float Value { get; private set; }
    public bool IsAnimating { get; private set; }

    public LaserDissolve(ViewData viewData)
    {
        this.viewData = viewData;
    }

    public void SetZero()
    {
        Value = 0;
        IsAnimating = false;
    }

    /// <summary>
    /// IsAnimating 条件判定   
    /// </summary>
    /// <returns></returns>
    public IEnumerator Dissaper()
    {
        float dissolveTime = viewData.DissolveTime;
        float startTime = dissolveTime;
        IsAnimating = true;

        while(dissolveTime > 0)
        {
            dissolveTime -= Time.deltaTime;
            Value = 1 - dissolveTime / startTime;
            yield return null;
        }

        IsAnimating = false;
    }

    public void Enable()
    {
        SetZero();
    }
}
