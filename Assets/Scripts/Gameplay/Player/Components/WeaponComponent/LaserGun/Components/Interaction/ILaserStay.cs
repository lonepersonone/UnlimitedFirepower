using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILaserStay
{
    void OnLaserStay(LaserBase laser, List<RaycastHit2D> hits);
}
