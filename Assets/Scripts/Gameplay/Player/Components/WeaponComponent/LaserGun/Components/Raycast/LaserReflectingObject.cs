using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ¿É·´Éä¼¤¹âÅö×²Æ÷
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class LaserReflectingObject : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Collider2D[] colliders = GetComponents<Collider2D>();
        LaserManager.Instance.reflectingColliders.Add(colliders);
    }


}
