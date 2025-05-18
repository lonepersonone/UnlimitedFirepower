using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �ɷ��伤����ײ��
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
