using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralGeneration : MonoBehaviour
{
    [SerializeField] int width, height;
    [SerializeField] int firstHoleDistance, minHoleDistance, maxHoleDistance, minHoleWidth, maxHoleWidth;
    int holeDistance, holeWidth;
    int prevHole;
    public bool hole = false;
    private PGDetector pGDetector;
    float colDelay = 5f;
    [SerializeField] GameObject ground;

    void Awake() {
        pGDetector = GetComponentInChildren<PGDetector>();
        pGDetector.enabled = false;

        firstHoleDistance = Random.Range(23, 61);
        holeDistance = Random.Range(minHoleDistance, maxHoleDistance);
        holeWidth = Random.Range(minHoleWidth, maxHoleWidth);
        prevHole = firstHoleDistance + holeWidth - 1;
        Generate();
    }

    void FixedUpdate() {
        if (colDelay > 0f) {
            colDelay -= Time.deltaTime;
        }else {
            pGDetector.enabled = true;
        }
    }

    void Generate() {
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                if (x == firstHoleDistance || hole || x == holeDistance) {
                    if (holeWidth > 0) {
                        hole = true;
                        holeWidth -= 1;
                    }else {
                        prevHole = x;
                        holeWidth = Random.Range(minHoleWidth, maxHoleWidth);
                        holeDistance = prevHole + Random.Range(minHoleDistance, maxHoleDistance);
                        hole = false;
                    }
                }
                if (!hole) {
                    SpawnObject(ground, x, y);
                }
            }
        }
    }

    void SpawnObject(GameObject obj, int width, int height) {
        obj = Instantiate(obj, new Vector2(width, height), Quaternion.identity);
        obj.transform.parent = this.transform;
    }
}
