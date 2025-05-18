using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
public abstract class LaserBase : PoolableMonoBehaviour, ICoroutineRunner
{
    //存储激光所有数据信息
    [SerializeField] protected LaserData laserData = new();
    public LaserData LaserData => laserData;

    //控制运行激光的生命周期
    protected LifeCycle lifeCycle;
    protected LaserDissolve Dissolve { get; private set; }

    protected readonly Switcher Switcher = new();

    public override bool IsActive => Dissolve.IsAnimating || Switcher.Enabled;

    public bool isActiveAndEnable => throw new NotImplementedException();

    void Start()
    {
        TryInitialize();
    }

    public void Update()
    {
        //Debug.Log("LaserBase Update");
        if (lifeCycle != null)
        {
            lifeCycle.Update();
        }
        
    }

    private void OnDisable()
    {
        Disable();
    }

    public void TryInitialize()
    {
        //Debug.Log("LaserBase SetDamage");
        if(lifeCycle == null && laserData != null)
        {
            Dissolve = new LaserDissolve(laserData.ViewData);
            lifeCycle = CreateLifeCycleFactory().Create();
            laserData.LaserMeshFilter.Initialize(GetComponent<MeshFilter>());
        }
 
    }

    public void Enable()
    {
        TryInitialize();
        if(Switcher.TryEnable())
            lifeCycle.Enable();
    }



    public void Disable()
    {
        TryInitialize();
        if(Switcher.TryDisable())
            lifeCycle.Disable();
    }

    protected abstract ILaserLifeCycleFactory CreateLifeCycleFactory();
}

