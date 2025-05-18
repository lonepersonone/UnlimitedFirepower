using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentFactory<T> : IFactory<T> where T : Component
{
    private readonly T prefab;
    private int count;

    public ComponentFactory(T prefab)
    {
        this.prefab = prefab;
    }

    public T Create()
    {
        //Debug.Log("CreatLaser");
        if(prefab != null)
        {
            T result = Object.Instantiate(prefab, LaserManager.Instance.transform);
            result.transform.position = Vector3.zero;
            result.transform.rotation = Quaternion.identity;
            result.gameObject.name = $"{result.gameObject.name} {++count}";
            return result;
        }
        return null;
    }
}
