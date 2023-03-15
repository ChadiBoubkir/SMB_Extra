using UnityEngine;

public class SideScrolling : MonoBehaviour
{
    private Transform player;
    private GameObject boss;

    public float height = 6.5f;
    public float undergroundHeight = -9.5f;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player").transform;
        boss = GameObject.FindGameObjectWithTag("Boss");
    }


    private void LateUpdate()
    {
        if (boss != null) {
            if (boss.GetComponent<Venoomba>() != null) {
                if (!boss.GetComponent<Venoomba>().bossFight) {
                    Vector3 cameraPosition = transform.position;
                    cameraPosition.x = Mathf.Max(cameraPosition.x, player.position.x);
                    transform.position = cameraPosition;
                }
            }
        }else {
            Vector3 cameraPosition = transform.position;
            cameraPosition.x = Mathf.Max(cameraPosition.x, player.position.x);
            transform.position = cameraPosition;
        }

        
    }

    public void SetUnderground(bool underground)
    {
        Vector3 cameraPosition = transform.position;
        cameraPosition.y = underground ? undergroundHeight : height;
        transform.position = cameraPosition;
    }
}