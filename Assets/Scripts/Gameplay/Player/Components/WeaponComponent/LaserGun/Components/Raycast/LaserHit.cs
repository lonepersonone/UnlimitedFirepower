using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserHit : MonoBehaviour
{
    public Collider2D HitObject { get; private set; }
    public Vector2 Normal { get; private set; }
    public Vector2 Direction { get; private set; }
    public Vector2 ReflectedDirection => Vector2.Reflect(Direction, Normal);
    public Vector2 HitPoint { get; private set; }
    public float Distance { get; private set; }

    public void SetValue(Collider2D hitCollider, Vector2 normal, Vector2 direction, Vector2 hitPoint, float distance)
    {
        this.HitObject = hitCollider;
        this.Normal = normal;
        this.Direction = direction;
        this.HitPoint = hitPoint;
        this.Distance = distance;
    }

    public void UpdateValue(LaserHit hit)
    {
        SetValue(hit.HitObject, hit.Normal, hit.Direction,hit.HitPoint, hit.Distance);
    }
}
