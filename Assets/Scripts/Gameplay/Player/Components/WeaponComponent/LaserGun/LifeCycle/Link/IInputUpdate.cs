using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInputUpdate <in TInput>
{
    void Update(TInput input);
}
