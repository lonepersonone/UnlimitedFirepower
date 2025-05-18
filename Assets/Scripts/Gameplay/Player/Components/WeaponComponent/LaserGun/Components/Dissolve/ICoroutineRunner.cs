using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICoroutineRunner 
{
    void StopAllCoroutines();
    Coroutine StartCoroutine(IEnumerator routine);
    bool isActiveAndEnable { get; }
}
