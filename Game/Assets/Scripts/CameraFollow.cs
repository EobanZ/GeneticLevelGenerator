using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public float cameraDistance = 200.0f;

    void Awake () {
        GetComponent<UnityEngine.Camera>().orthographicSize = ((Screen.height / 2) / cameraDistance);
    }

    void FixedUpdate ()
    {
        transform.position = new Vector3(player.position.x, transform.position.y, transform.position.z);
    }
}
