using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RaycastData
{
    [SerializeField] private LayerMask layerMask = 0 << 1;
    [SerializeField][Min(0)]private float maxDistance = 20;
    [SerializeField][Min(0)] private float maxPoints = 15;
    [SerializeField][Min(0)] private float shootSpeed = 10;

    public LayerMask LayerMask => layerMask;
    public float MaxDistance => maxDistance;
    public float MaxPoints => maxPoints;
    public float ShootSpeed => shootSpeed;
}
