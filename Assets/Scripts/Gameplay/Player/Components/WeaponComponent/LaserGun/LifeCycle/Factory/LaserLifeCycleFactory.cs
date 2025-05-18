using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserLifeCycleFactory : ILaserLifeCycleFactory
{
    private readonly IUpdateCondition updateCondition;
    private LaserComponents laserComponents;

    public LaserLifeCycleFactory(IUpdateCondition updateCondition, LaserComponents laserComponents)
    {
        this.updateCondition = updateCondition;
        this.laserComponents = laserComponents;
    }

    public LifeCycle Create()
    {
        IUpdate update = new UpdateComposite(
            laserComponents.transformMapper,
            laserComponents.laserRaycast,
            laserComponents.laserMeshRebuilding,
            laserComponents.laserLength,
            laserComponents.laserActiveHits,
            laserComponents.laserRaycastHitEvent,
            laserComponents.laserHitEffect,
            new UpdateLink<Dictionary<Collider2D, List<RaycastHit2D>>>(laserComponents.laserCollision, laserComponents.laserInteraction)            
            );

        IEnable enable = new EnableComposite(laserComponents.laserLength, laserComponents.laserDissolve, laserComponents.laserCoroutineShutdown);

        IDisable disable = new DisableComposite(laserComponents.laserHitEffect, laserComponents.laserInteraction, laserComponents.laserCoroutineShutdown);

        //通过条件控制生命循环周期
        IUpdate updateCycle = new UpdateComposite(new ConditionalUpdate(updateCondition, update), laserComponents.laserView);

        return new LifeCycle(enable, updateCycle, disable);
    }
}
