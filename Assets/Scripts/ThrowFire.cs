using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowFire : MonoBehaviour
{
    public Transform firePoint;
    public GameObject fireballPrefab;

    public void Throw() {
        Instantiate(fireballPrefab, firePoint.position, firePoint.rotation);
    }
}
