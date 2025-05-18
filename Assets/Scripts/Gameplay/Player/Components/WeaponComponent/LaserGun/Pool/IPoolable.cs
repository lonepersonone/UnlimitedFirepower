using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPoolable 
{
    bool IsActive { get; } // �Ƿ���
    void Reset(); // ����
    void ReturnToPool(); // ���ض����
}
