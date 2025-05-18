using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILaserEntered 
{
    void OnLaserEntered(LaserBase laser, List<RaycastHit2D> hits);
}
