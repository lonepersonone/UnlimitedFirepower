using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPoolable 
{
    bool IsActive { get; } // 是否存活
    void Reset(); // 重置
    void ReturnToPool(); // 返回对象池
}
