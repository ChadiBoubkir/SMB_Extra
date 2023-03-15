using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;

    private float yDifference = 0.1f;
    private int chosenNumber = 1;
    private Vector3 newPos;

    void Start()
    {
        yDifference = Random.Range(0.05f, 0.15f);
        chosenNumber = Random.Range(-2, 2);
        if (chosenNumber == 0 || chosenNumber == -2) {
            chosenNumber = -1;
        }else if (chosenNumber == 2) {
            chosenNumber = 1;
        }
        yDifference *= chosenNumber;
        newPos = firePoint.position;
        newPos.y = firePoint.position.y + yDifference;
    }

    void Update() {
        newPos = firePoint.position;
        newPos.y = firePoint.position.y + yDifference;
    }

    public void ShootBullet() {
        Instantiate(bulletPrefab, newPos, firePoint.rotation);

        yDifference = Random.Range(0.05f, 0.15f);
        chosenNumber = Random.Range(-2, 2);
        if (chosenNumber == 0 || chosenNumber == -2) {
            chosenNumber = -1;
        }else if (chosenNumber == 2) {
            chosenNumber = 1;
        }
        yDifference *= chosenNumber;
        newPos = firePoint.position;
        newPos.y = firePoint.position.y + yDifference;
    }
}
