using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> : IFactory<T> where T : IPoolable
{
    public IFactory<T> factory;
    public Stack<T> pool = new Stack<T>();
    public List<T> activeObjectList = new List<T>();

    public ObjectPool(IFactory<T> factory)
    {
        this.factory = factory;
    }

    /// <summary>
    /// 将失活物体压进栈中
    /// </summary>
    /// <param name="prefab"></param>
    public void Push(T prefab)
    {
        pool.Push(prefab);
    }

    /// <summary>
    /// 创建新物体
    /// </summary>
    /// <returns></returns>
    public T Create()
    {
        T prefab = pool.Count == 0 ? factory.Create() : pool.Pop();
        prefab.Reset();
        activeObjectList.Add(prefab);
        return prefab;
    }

    public void Update()
    {
        for(int i = 0; i < activeObjectList.Count; i++)
        {
            if (activeObjectList[i].IsActive == false)
            {
                activeObjectList[i].ReturnToPool();
                pool.Push(activeObjectList[i]);
                activeObjectList.RemoveAt(i);
            }
        }
    }
}
