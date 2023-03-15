using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireMushroom : MonoBehaviour
{
    private GameObject character;
    private Player player;
    private BlockHit blockHit;
    public GameObject fireFlower;
    public GameObject magicMushroom;

    void Awake() {
        character = GameObject.FindGameObjectWithTag("Player");
        player = character.GetComponent<Player>();
        blockHit = GetComponent<BlockHit>();
    }

    private void Update() {
        if (blockHit != null) {
            if (blockHit.item != null) {
                if (player.small) {
                    if (blockHit.item == fireFlower) {
                        blockHit.item = magicMushroom;
                    }
                }else if (player.big || player.fire) {
                    if (blockHit.item == magicMushroom) {
                        blockHit.item = fireFlower;
                    }
                }
            }
        }
    }
}
