using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabWeapon : MonoBehaviour
{

    public Transform firePoint;
    public GameObject laserPrefab;

    private Animator anim;

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
         //   anim.SetTrigger("laser");
            Shoot();
        }
    }

    void Shoot()
    {
        Instantiate(laserPrefab, firePoint.position, firePoint.rotation);
    }
}
