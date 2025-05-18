using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : LaserBase
{
    //�洢����������Ҫ���
    private LaserComponents laserComponents;

    //����Laser��Ψһ��ʶ��
    public Guid Id { get; private set; } = Guid.NewGuid();

    protected override ILaserLifeCycleFactory CreateLifeCycleFactory()
    {
        laserComponents = new LaserComponents(this, laserData);
        laserData.LaserMeshFilter.Initialize(GetComponent<MeshFilter>());
        return new LaserLifeCycleFactory(Switcher, laserComponents);
    }

    public void BranchLaser(Laser parent)
    {
        TryInitialize();
        Id = parent.Id;
        laserComponents.laserRaycastHitEvent.InheritedEvent(parent.laserComponents.laserRaycastHitEvent);
    }

    public void Enable(Transform laserPoint)
    {       
        if (Switcher.Enabled == false)
        {
            Enable();
            laserComponents.transformMapper.SetSource(laserPoint);
        }
    }


}
