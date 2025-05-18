using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformMapper : IUpdate
{
    private Transform target;
    public Transform Source { get; private set; }
    
    public TransformMapper(Transform target, Transform source = null)
    {
        this.target = target;
        this.Source = source;
    }

    void IUpdate.Update()
    {
        MapPosition();
        MapRotation();
    }

    public void SetSource(Transform source)
    {
        this.Source = source;
    }

    public void MapPosition()
    {
        if(Source != null)
            target.position = Source.position;
    }

    public void MapRotation()
    {
        if(target != null)
            target.rotation = Source.rotation;
    }

}
