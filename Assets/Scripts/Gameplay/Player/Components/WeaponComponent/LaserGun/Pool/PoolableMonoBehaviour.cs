using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PoolableMonoBehaviour : MonoBehaviour, IPoolable
{
    public abstract bool IsActive { get; }

    public void Reset()
    {
        gameObject.SetActive(true);
    }

    public void ReturnToPool()
    {
        gameObject.SetActive(false);
    }


}
