using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserPool 
{
    private readonly Dictionary<Guid, ObjectPool<Laser>> pool = new();

    public Laser GetLaser(Laser laser)
    {
        if (pool.ContainsKey(laser.Id) == false)
        {
            LaserFactory laserFactory = new LaserFactory(laser);
            pool[laser.Id] = new ObjectPool<Laser>(laserFactory);
            pool[laser.Id].Push(laser);
        }

        return pool[laser.Id].Create();
    }

    public void Update()
    {
        foreach (ObjectPool<Laser> laserPool in pool.Values)
        {
            laserPool.Update();
        }
    }
}
