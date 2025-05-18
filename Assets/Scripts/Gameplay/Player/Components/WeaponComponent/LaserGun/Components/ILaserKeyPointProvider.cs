using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILaserKeyPointProvider 
{
    //访问器
    public Vector2 this[int index] { get; }

    //碰撞点数量
    public int Count { get; }
}
