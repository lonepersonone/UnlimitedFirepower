using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CollisionData 
{
    //´©Í¸Öµ
    [SerializeField] [Range(0, 0.5f)] private float penetration = 0.15f;
    [SerializeField] [Range(0, 2f)] private float width = 0.5f;
    [SerializeField] private LayerMask layerMask = 0 << 1;

    public float Penetration => penetration;
    public float Width => width;
    public LayerMask LayerMask => layerMask;
}
