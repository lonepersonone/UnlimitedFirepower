using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserFactory: IFactory<Laser>
{
    private readonly ComponentFactory<Laser> componentFactory;
    private readonly Laser prefab;

    public LaserFactory(Laser prefab)
    {
        componentFactory = new ComponentFactory<Laser>(prefab);
        this.prefab = prefab;
    }

    public Laser Create()
    {
        Laser laser = componentFactory.Create();
        laser.BranchLaser(prefab);

        return laser;
    }


}
