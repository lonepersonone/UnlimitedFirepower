using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-20)]
public class LaserManager : MonoBehaviour
{
    public readonly ReflectingColliders reflectingColliders = new();

    private readonly LaserPool laserPool = new();
    public LaserPool LaserPool { 
        get 
        { 
            if (laserPool == null)
            {
                throw new Exception("LaserPool wasn't initiated");
            }
            return laserPool; 
        } 
    }

    private static LaserManager instance;
    public static LaserManager Instance
    {
        get
        {
            if (instance == null)
                throw new Exception("LaserManager wasn't initiated");
            return instance;
        }
    }

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        laserPool.Update();
    }
}
