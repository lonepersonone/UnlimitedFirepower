using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectingColliders : MonoBehaviour
{
    public readonly HashSet<Collider2D> colliders;

    public void Add(IReadOnlyCollection<Collider2D> collider2s)
    {
        foreach(var collider in collider2s)
        {
            colliders.Add(collider);
        }
    }

    public bool Contains(Collider2D collider)
    {
        return colliders.Contains(collider);
    }
}
