using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMovement : MonoBehaviour
{
    private Camera cam;
    
    void Start()
    {
        cam = Camera.main;
    }

    
    void LateUpdate()
    {
        Vector3 newPosition = transform.position;
        newPosition.x = Mathf.Max(newPosition.x, cam.transform.position.x);
        transform.position = newPosition;
    }
}
