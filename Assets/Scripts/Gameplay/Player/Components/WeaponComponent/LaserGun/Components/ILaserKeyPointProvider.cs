using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILaserKeyPointProvider 
{
    //������
    public Vector2 this[int index] { get; }

    //��ײ������
    public int Count { get; }
}
