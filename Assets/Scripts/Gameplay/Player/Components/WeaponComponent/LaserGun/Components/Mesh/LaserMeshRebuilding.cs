using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserMeshRebuilding : IUpdate
{
    private readonly ILaserKeyPointProvider keyPointProvider;
    private readonly LaserRectLine laserMeshFilter;

    public LaserMeshRebuilding(ILaserKeyPointProvider laserKeyPointProvider, LaserRectLine laserMeshFilter)
    {
        this.keyPointProvider = laserKeyPointProvider;
        this.laserMeshFilter = laserMeshFilter;
    }

    public void Update()
    {
        //Debug.Log("LaserRectLineMesh Update");
        laserMeshFilter.ClearPoints();
        for(int i = 0; i < keyPointProvider.Count; i++)
        {
            laserMeshFilter.AddMeshLocalPoint(keyPointProvider[i]);
        }
        laserMeshFilter.GenerateFullRectMesh();
    }
}
