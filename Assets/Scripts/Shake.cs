using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shake : MonoBehaviour
{
    public bool shaking = false;
    [SerializeField] private float shakeAmmount;

    private void FixedUpdate() {
        if (shaking) {
            Vector3 newPos = Random.insideUnitSphere * (Time.deltaTime * shakeAmmount);
            newPos.z = transform.position.z;
            newPos.y = transform.position.y;

            transform.position = newPos;
        }
    }

    public void StartShaking()
    {
        StartCoroutine("ShakeNow");
    }

    IEnumerator ShakeNow() {
        Vector3 originalPos = transform.position;

        if (!shaking) {
            shaking = true;
        }

        yield return new WaitForSeconds(0.25f);

        shaking = false;
        transform.position = originalPos;
    }
}
