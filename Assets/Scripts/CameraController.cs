using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    //Transform to follow
    public Transform p;

    //Y Height
    public float cameraHeight;

    private float maxZ;
    private float minZ;
    private float maxX;
    private float minX;
    // Start is called before the first frame update
    void Start()
    {
        //get min and max Z and X
        maxZ = 11.5f;
        minZ = -11.5f;
        maxX = 20;
        minX = -20;

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = new Vector3(p.transform.position.x, cameraHeight, p.transform.position.z);

        if(pos.x > maxX) {
            pos.x = maxX;
        }

        if(pos.x < minX) {
            pos.x = minX;
        }

        if(pos.z > maxZ) {
            pos.z = maxZ;
        }

        if(pos.z < minZ) {
            pos.z = minZ;
        }

        transform.position = pos;
    }
}
