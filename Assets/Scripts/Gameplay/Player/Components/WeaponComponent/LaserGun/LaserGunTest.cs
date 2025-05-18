using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserGunTest : MonoBehaviour
{
    [SerializeField] private Transform _turret;
    [SerializeField] private Transform _fastening;
    [SerializeField] private Transform _turretAnchor;
    [SerializeField] private Transform _laserPoint;
    [SerializeField] private Laser laser;

    private void Start()
    {

    }

    private void Update()
    {
        ManageLaser();
    }

    private void ManageLaser()
    {
        if (Input.GetMouseButtonDown(0))
        {
            laser = LaserManager.Instance.LaserPool.GetLaser(laser);
/*            GameObject bullet = Object.Instantiate(Resources.Load("LaserBullet")) as GameObject;
            laser = bullet.GetComponent<Laser>();*/
            laser.Enable(_laserPoint);
        }

        if (Input.GetMouseButtonUp(0))
        {
            laser.Disable();
        }
    }
}
