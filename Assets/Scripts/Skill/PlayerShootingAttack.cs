using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShootingAttack : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;
    public int skillNum = 0;

    private void Update()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        if (SwitchManager.Instance.abilities[skillNum])
        {
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        }
    }
}
