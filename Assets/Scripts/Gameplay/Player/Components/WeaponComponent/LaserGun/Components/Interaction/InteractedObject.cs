using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InteractedObject 
{
    private readonly IEnumerable<ILaserEntered> onEntered;
    private readonly IEnumerable<ILaserStay> onStay;
    private readonly IEnumerable<ILaserExited> onExited;
    private readonly LaserBase laser;

    public InteractedObject(LaserBase laser, Transform contex)
    {
        this.laser = laser;
        onEntered = GetComponents<ILaserEntered>(contex);
        onStay = GetComponents<ILaserStay>(contex);
        onExited = GetComponents<ILaserExited>(contex);
    }

    /// <summary>
    /// 封装Entered接口
    /// </summary>
    /// <param name="hits"></param>
    public void OnEntered(List<RaycastHit2D> hits)
    {
        foreach (ILaserEntered enter in onEntered)
            enter?.OnLaserEntered(laser, hits);
    }

    public void OnStay(List<RaycastHit2D> hits)
    {
        foreach(ILaserStay stay in onStay)
            stay?.OnLaserStay(laser, hits);
    }

    public void OnExited()
    {
        foreach (ILaserExited exit in onExited)
            exit?.OnLaserExited(laser);
    }

    /// <summary>
    /// 找到monoBehaviour特定数据类型对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="contex"></param>
    /// <returns></returns>
    private IEnumerable<T> GetComponents<T>(Transform contex)
    {
        MonoBehaviour[] monoBehaviours = contex.GetComponents<MonoBehaviour>();
        return monoBehaviours.OfType<T>();
     }
}
