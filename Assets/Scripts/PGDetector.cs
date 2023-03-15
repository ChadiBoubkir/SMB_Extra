using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PGDetector : MonoBehaviour
{
    private int width = 223;
    private ProceduralGeneration proceduralGeneration;
    private void Start() {
        proceduralGeneration = GetComponentInParent<ProceduralGeneration>();

        for (int x = 0; x < width; x++) {
            transform.position = new Vector3(x, transform.position.y, 0);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "BackgroundItem") {
            Debug.Log("Collision Registered.");
            if (proceduralGeneration.hole) {
                collision.gameObject.transform.position += new Vector3(1, 0, 0);
            }
        }
    }
}
