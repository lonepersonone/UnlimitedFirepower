using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFactory<out T>
{
    T Create(); // ����
}
