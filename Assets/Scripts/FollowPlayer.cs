using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{

    [SerializeField] private Transform player;

    [SerializeField] private Transform playerCam;

    // private float xMargin = 2;
    // private float yMargin = 2;

    private float cameraZ;

    // Initializing
    void Start() {
        cameraZ = playerCam.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        // Vector3 differenceVector = player.position - camera.position;
        // differenceVector.z = 0; //we don't care about Z axis difference;
        // float difference = differenceVector.magnitude;

        Vector3 playerPos = player.position;
        playerPos.z = cameraZ;
        playerCam.position = playerPos;

    }
}
